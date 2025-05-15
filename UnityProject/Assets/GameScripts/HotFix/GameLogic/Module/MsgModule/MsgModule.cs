using ClientSprotoType;
using GameLogic;
using Sproto;
using SprotoType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TEngine;

namespace GameLogic
{
    public delegate SprotoTypeBase RpcReqHandler(SprotoTypeBase rpcReq);
    public delegate void RpcRspHandler(SprotoTypeBase rpcRsp);

    /// <summary>
    /// 消息管理模块
    /// </summary>
    public sealed class MsgModule : Singleton<MsgModule>, IUpdate
    {

        private static AClient clientA;
 
        private long session;

        private  Dictionary<long, RpcRspHandler> rpcRspHandlerDict;
        private static Dictionary<int, RpcReqHandler> rpcReqHandlerDict;

        private static ProtocolFunctionDictionary clientProtocol = ClientProtocol.Instance.Protocol;
        private static ProtocolFunctionDictionary serverProtocol = ServerProtocol.Instance.Protocol;

        private static Dictionary<long, ProtocolFunctionDictionary.typeFunc> sessionDict;

        private static SprotoStream sendStream = new SprotoStream();
        private static SprotoStream recvStream = new SprotoStream();

        private static SprotoPack sendPack = new SprotoPack();
        private static SprotoPack recvPack = new SprotoPack();

        protected override void OnInit()
        {
            base.OnInit();
            clientA = GameModule.Network.CreateNetworkClient(NetworkType.TCP, 4096, new TapGoNetPackageEncoder(), 
                new TapGoNetPackageDecoder());
            sessionDict = new Dictionary<long, ProtocolFunctionDictionary.typeFunc>();
            rpcRspHandlerDict = new Dictionary<long, RpcRspHandler>();
            rpcReqHandlerDict = new Dictionary<int, RpcReqHandler>();
        }

        public void OnUpdate()
        {
            // 在这里实现每帧更新的逻辑
            TapGoNetPackage package = clientA.PickPackage() as TapGoNetPackage;
            if (package != null) 
            {
                Package pkg = new Package();
                byte[] data = recvPack.unpack(package.BodyBytes);
                int offset = pkg.init(data);

                int tag = (int)pkg.type;
                long session = (long)pkg.session;

                if (pkg.HasType)
                {
                    RpcReqHandler rpcReqHandler = GetServerReqHandler(tag);
                    if (rpcReqHandler != null)
                    {
                        SprotoTypeBase rpcRsp = rpcReqHandler(serverProtocol.GenRequest(tag, data, offset));
                        if (pkg.HasSession) Send(rpcRsp, session, tag);
                    }
                }
                else
                {
                    RpcRspHandler rpcRspHandler = GetServerRspHandler(session);
                    if (rpcRspHandler != null)
                    {
                        ProtocolFunctionDictionary.typeFunc GenResponse;
                        sessionDict.TryGetValue(session, out GenResponse);
                        rpcRspHandler(GenResponse(data, offset));
                    }
                }
            }

        }

        public override void Release()
        {
            base.Release();
        }

        public void Connect(string ip, int port, Action<SocketError> callback)
        { 
            clientA.Connect(ip, port, (e) =>
            {
                callback?.Invoke(e);
            });
        }

        public void Send<T>(SprotoTypeBase rpcReq = null, RpcRspHandler rpcRspHandler = null)
        {
            if (rpcRspHandler != null)
            {
                session++;
                AddServerRspHandler(session, rpcRspHandler);
                SendMsg<T>(rpcReq, session);
            }
            else
            {
                SendMsg<T>(rpcReq);
            }
        }

        public static void SendMsg<T>(SprotoTypeBase rpc = null, long? session = null)
        {
            Send(rpc, session, clientProtocol[typeof(T)]);
        }

        private static void Send(SprotoTypeBase rpc, long? session, int tag)
        {

            Package pkg = new Package();
            pkg.type = tag;

            if (session != null)
            {
                pkg.session = (long)session;
                sessionDict.Add((long)session, clientProtocol[tag].Response.Value);
            }

            sendStream.Seek(0, SeekOrigin.Begin);
            int len = pkg.encode(sendStream);
            if (rpc != null) len += rpc.encode(sendStream);

            byte[] data = sendPack.pack(sendStream.Buffer, len);
            TapGoNetPackage package = new TapGoNetPackage();
            package.BodyBytes = data;
            clientA.SendPackage(package);
        }

        private void AddServerRspHandler(long session, RpcRspHandler rpcRspHandler)
        {
            rpcRspHandlerDict.Add(session, rpcRspHandler);
        }

        private void RemoveServerRspHandler(long session)
        {
            if (rpcRspHandlerDict.ContainsKey(session))
            {
                rpcRspHandlerDict.Remove(session);
            }
        }

        public RpcRspHandler GetServerRspHandler(long session)
        {
            RpcRspHandler rpcRspHandler;
            rpcRspHandlerDict.TryGetValue(session, out rpcRspHandler);
            RemoveServerRspHandler(session);
            return rpcRspHandler;
        }

        public static void AddServerReqHandler(int tag, RpcReqHandler rpcReqHandler)
        {
            rpcReqHandlerDict.Add(tag, rpcReqHandler);
        }

        public static int AddServerReqHandler<T>(RpcReqHandler rpcReqHandler)
        {
            int tag = serverProtocol[typeof(T)];
            AddServerReqHandler(tag, rpcReqHandler);
            return tag;
        }

        public static void RemoveServerReqHandler(int tag)
        {
            if (rpcReqHandlerDict.ContainsKey(tag))
            {
                rpcReqHandlerDict.Remove(tag);
            }
        }

        public static void RemoveServerReqHandler<T>()
        {
            RemoveServerReqHandler(serverProtocol[typeof(T)]);
        }

        public static RpcReqHandler GetServerReqHandler(int tag)
        {
            RpcReqHandler rpcReqHandler;
            rpcReqHandlerDict.TryGetValue(tag, out rpcReqHandler);
            return rpcReqHandler;
        }

        public static RpcReqHandler GetServerReqHandler<T>()
        {
            return GetServerReqHandler(serverProtocol[typeof(T)]);
        }

    }
} 