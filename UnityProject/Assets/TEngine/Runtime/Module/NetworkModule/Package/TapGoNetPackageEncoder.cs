using System;

namespace TEngine
{
    /// <summary>
    /// 网络包编码器。
    /// </summary>
    public class TapGoNetPackageEncoder : INetPackageEncoder
    {
        private HandleErrorDelegate _handleErrorCallback;
        private const int HeaderMsgLengthFiledSize = 2; //包头里的包体长度（short类型）。

        /// <summary>
        /// 获取包头的尺寸。
        /// </summary>
        public int GetPackageHeaderSize()
        {
            return HeaderMsgLengthFiledSize;
        }

        /// <summary>
        /// 注册异常错误回调方法。
        /// </summary>
        public void RegisterHandleErrorCallback(HandleErrorDelegate callback)
        {
            _handleErrorCallback = callback;
        }

        /// <summary>
        /// 编码。
        /// </summary>
        /// <param name="packageBodyMaxSize">包体的最大尺寸。</param>
        /// <param name="ringBuffer">编码填充的字节缓冲区。</param>
        /// <param name="encodePackage">发送的包裹。</param>
        public void Encode(int packageBodyMaxSize, RingBuffer ringBuffer, INetPackage encodePackage)
        {
            if (encodePackage == null)
            {
                _handleErrorCallback(false, "The encode package object is null");
                return;
            }

            TapGoNetPackage package = (TapGoNetPackage)encodePackage;
            if (package == null)
            {
                _handleErrorCallback(false, $"The encode package object is invalid : {encodePackage.GetType()}");
                return;
            }

            // 检测逻辑是否合法。
            if (package.BodyBytes == null)
            {
                _handleErrorCallback(false, $"The encode package BodyBytes field is null : {encodePackage.GetType()}");
                return;
            }

            // 获取包体数据。
            byte[] bodyData = package.BodyBytes;

            // 检测包体长度.
            if (bodyData.Length > packageBodyMaxSize)
            {
                _handleErrorCallback(false, $"The encode package  body size is larger than {packageBodyMaxSize}");
                return;
            }

            ushort bodyLength = (ushort)bodyData.Length;
            byte[] bodyLengthBytes = BitConverter.GetBytes(bodyLength);

            // 如果系统是小端序，则需要反转字节顺序。
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bodyLengthBytes);
            }

            // 写入大端序的包体长度。
            ringBuffer.WriteBytes(bodyLengthBytes, 0, bodyLengthBytes.Length);

            // 写入包体。
            ringBuffer.WriteBytes(bodyData, 0, bodyData.Length);
        }
    }
}