using System.Collections.Generic;
using System.Reflection;
using GameLogic;
using Sproto;
using SprotoType;
using TEngine;
using Unity.Android.Gradle.Manifest;
#pragma warning disable CS0436


/// <summary>
/// 游戏App。
/// </summary>
public partial class GameApp
{
    private static List<Assembly> _hotfixAssembly;
    private static AClient clientA;
    /// <summary>
    /// 热更域App主入口。
    /// </summary>
    /// <param name="objects"></param>
    public static void Entrance(object[] objects)
    {
        GameEventHelper.Init();
        _hotfixAssembly = (List<Assembly>)objects[0];
        Log.Warning("======= 看到此条日志代表你成功运行了热更新代码 =======");
        Log.Warning("======= Entrance GameApp =======");
        Utility.Unity.AddDestroyListener(Release);

#if false
        clientA = GameModule.Network.CreateNetworkClient(NetworkType.TCP, 4096, new TapGoNetPackageEncoder(), new TapGoNetPackageDecoder());
        clientA.Connect("43.199.74.135", 8888, (e)=> {
            Log.Debug($"连接服务器结果：{e}");
            if (e == System.Net.Sockets.SocketError.Success)
            {
                ClientSprotoType.login.request loginRequest = new ClientSprotoType.login.request();
                loginRequest.appver = "1.1.18";
                loginRequest.account_type = "fastlogin";
                loginRequest.openid = "robot_1203123";
                loginRequest.token = "";
                var extra = Utility.Json.ToJson(new
                {
                    ts = "1544616066645",
                    playerLevel = "1",
                    playerSSign = "",
                    nickname = "blabla",
                    reconnect = false,
                    reconnect_token = "9KAGNoHE",
                });
                loginRequest.extra = extra;
                loginRequest.device_info = Utility.Json.ToJson(new
                {
                    os_type = 3, //android
                    phone_number = "1501899000",
                    ChannelId = "GF0SN10000",
                    androidid = "androidid0000123",
                    idfa = "idfa12222222223",
                });

                //SprotoRpc client = new SprotoRpc();
                SprotoRpc.RpcRequest clientRequest = service.Attach(ClientProtocol.Instance);
                byte[] req = clientRequest.Invoke<ClientProtocol.login>(loginRequest, 1);
                //service.Dispatch(req);
                TapGoNetPackage package = new TapGoNetPackage();
                package.BodyBytes = req;
                clientA.SendPackage(package);
            }
        }); 
#endif

        MsgModule.Instance.Connect("43.199.74.135", 8888, (e) => {
            Log.Debug($"连接服务器结果：{e}");
            if (e == System.Net.Sockets.SocketError.Success)
            {
                ClientSprotoType.login.request loginRequest = new ClientSprotoType.login.request();
                loginRequest.appver = "1.1.18";
                loginRequest.account_type = "fastlogin";
                loginRequest.openid = "robot_1203123";
                loginRequest.token = "";
                var extra = Utility.Json.ToJson(new
                {
                    ts = "1544616066645",
                    playerLevel = "1",
                    playerSSign = "",
                    nickname = "blabla",
                    reconnect = false,
                    reconnect_token = "9KAGNoHE",
                });
                loginRequest.extra = extra;
                loginRequest.device_info = Utility.Json.ToJson(new
                {
                    os_type = 3, //android
                    phone_number = "1501899000",
                    ChannelId = "GF0SN10000",
                    androidid = "androidid0000123",
                    idfa = "idfa12222222223",
                });
                
                MsgModule.Instance.Send<ClientProtocol.login>(loginRequest, (response) =>
                {
                    ClientSprotoType.login.response loginResponse = response as ClientSprotoType.login.response;
                    Log.Debug($"Login Result {loginResponse.error_code} {loginResponse.error_msg} {loginResponse.account}");
                });

            }
        });
        StartGameLogic();
    }

#if false
function CMD.login(openid)
    local param = {
        appver = "1.1.18",
        account_type = "fastlogin",
		openid = openid or "robot_1203123",
        token = "",
        extra = json.encode({
            ts = "1544616066645",
            playerLevel = "1",
            playerSSign = "",
            nickname = 'blabla',
            --unionid = 'unionid3',
            reconnect = false,
            reconnect_token='9KAGNoHE',
        }),
        device_info = json.encode({
            os_type = 3, --android
            phone_number = "1501899000",
            ChannelId = "GF0SN10000",
            androidid = "androidid0000123",
            idfa = "idfa12222222223",
        })
    }

    send_request("login", param)
end
#endif

    private static void StartGameLogic()
    {
        GameEvent.Get<ILoginUI>().ShowLoginUI();
        GameModule.UI.ShowUIAsync<BattleMainUI>();
        GameModule.UpdateDriver.AddUpdateListener(Update);
    }
    
    private static void Release()
    {
        SingletonSystem.Release();
        Log.Warning("======= Release GameApp =======");
    }

    private static SprotoRpc service = new SprotoRpc(ClientProtocol.Instance);
    private static SprotoPack recvPack = new SprotoPack();

    private static void Update()
    {
#if false
        TapGoNetPackage package = clientA.PickPackage() as TapGoNetPackage;
        if (package != null)
        {
            try
            {
                var rpcInfo = service.Dispatch(package.BodyBytes);
                Log.Debug($"GameApp pick a package {package} {rpcInfo.tag} {rpcInfo.type} {rpcInfo.responseObj}");
                if (rpcInfo.type == SprotoRpc.RpcType.RESPONSE)
                {
                    ClientSprotoType.login.response loginResponse = rpcInfo.responseObj as ClientSprotoType.login.response;
                    Log.Debug($"Login Result {loginResponse.error_code} {loginResponse.user_info}");
                }
#if false
            byte[] data = recvPack.unpack(package.BodyBytes);
            SprotoType.Package pkg = new SprotoType.Package();
            int offset = pkg.init(data);

            int tag = (int)pkg.type;
            long session = (long)pkg.session;
#endif
            }
            catch (System.Exception e)
            {
                Log.Error($"GameApp Dispatch error {e.Message}");
            }
            finally
            {
            }
        }
#endif
    }
}