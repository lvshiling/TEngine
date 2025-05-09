using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using TEngine;

namespace GameLogic.Module
{
    /// <summary>
    /// 网络通信模块，支持 sproto 协议
    /// </summary>
    public class NetworkModule : Singleton<NetworkModule>, IUpdate
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private bool _isConnected;

        public void OnCreate()
        {
            Log.Info("NetworkModule OnCreate");
        }

        public void OnUpdate()
        {
            // 网络模块更新逻辑
        }

        public void OnDestroy()
        {
            Disconnect();
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="port">服务器端口</param>
        public async Task ConnectAsync(string host, int port)
        {
            try
            {
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(host, port);
                _networkStream = _tcpClient.GetStream();
                _isConnected = true;
                Log.Info($"Connected to {host}:{port}");
            }
            catch (Exception ex)
            {
                Log.Error($"Connection failed: {ex.Message}");
                _isConnected = false;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (_networkStream != null)
            {
                _networkStream.Close();
                _networkStream = null;
            }
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }
            _isConnected = false;
            Log.Info("Disconnected");
        }

        /// <summary>
        /// 发送数据（支持 sproto 协议）
        /// </summary>
        /// <param name="data">要发送的数据</param>
        public async Task SendAsync(byte[] data)
        {
            if (!_isConnected || _networkStream == null)
            {
                Log.Error("Not connected");
                return;
            }
            try
            {
                await _networkStream.WriteAsync(data, 0, data.Length);
                Log.Info("Data sent successfully");
            }
            catch (Exception ex)
            {
                Log.Error($"Send failed: {ex.Message}");
            }
        }

        /// <summary>
        /// 接收数据（支持 sproto 协议）
        /// </summary>
        /// <param name="bufferSize">接收缓冲区大小</param>
        /// <returns>接收到的数据</returns>
        public async Task<byte[]> ReceiveAsync(int bufferSize = 1024)
        {
            if (!_isConnected || _networkStream == null)
            {
                Log.Error("Not connected");
                return null;
            }
            try
            {
                byte[] buffer = new byte[bufferSize];
                int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length);
                byte[] data = new byte[bytesRead];
                Array.Copy(buffer, data, bytesRead);
                Log.Info($"Received {bytesRead} bytes");
                return data;
            }
            catch (Exception ex)
            {
                Log.Error($"Receive failed: {ex.Message}");
                return null;
            }
        }
    }
} 