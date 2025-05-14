using System;
using System.Collections.Generic;

namespace TEngine
{
    /// <summary>
    /// 网络包解码器。
    /// </summary>
    public class TapGoNetPackageDecoder : INetPackageDecoder
    {
        private HandleErrorDelegate _handleErrorCallback;
        //private const int HeaderMsgIDFiledSize = 4; //包头里的协议ID（int类型）。
        private const int HeaderMsgBodyLengthFiledSize = 2; //包头里的包体长度（int类型）。

        /// <summary>
        /// 获取包头的尺寸。
        /// </summary>
        public int GetPackageHeaderSize()
        {
            return HeaderMsgBodyLengthFiledSize;
        }

        /// <summary>
        /// 注册异常错误回调方法。
        /// </summary>
        public void RegisterHandleErrorCallback(HandleErrorDelegate callback)
        {
            _handleErrorCallback = callback;
        }

        /// <summary>
        /// 网络消息解码。
        /// </summary>
        /// <param name="packageBodyMaxSize">包体的最大尺寸。</param>
        /// <param name="ringBuffer">解码需要的字节缓冲区。</param>
        /// <param name="outputPackages">接收的包裹列表。</param>
        public void Decode(int packageBodyMaxSize, RingBuffer ringBuffer, List<INetPackage> outputPackages)
        {
            // 循环解包。
            while (true)
            {
                // 如果数据不够一个包头。
                if (ringBuffer.ReadableBytes < GetPackageHeaderSize())
                    break;
                ringBuffer.MarkReaderIndex();

                // 读取包头数据。
                //int msgID = ringBuffer.ReadInt();
                //int msgBodyLength = ringBuffer.ReadInt();
                int msgBodyLength = ringBuffer.ReadUShort();
                byte[] bodyLengthBytes = BitConverter.GetBytes(msgBodyLength);

                // 如果系统是小端序，则需要反转字节顺序。
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bodyLengthBytes);
                    msgBodyLength = BitConverter.ToUInt16(bodyLengthBytes, 0);
                }
                

                // 如果剩余可读数据小于包体长度。
                if (ringBuffer.ReadableBytes < msgBodyLength)
                {
                    ringBuffer.ResetReaderIndex();
                    break; //需要退出读够数据再解包。
                }

                TapGoNetPackage package = new TapGoNetPackage();
                package.MsgLen = msgBodyLength;

                // 检测包体长度。
                if (msgBodyLength > packageBodyMaxSize)
                {
                    _handleErrorCallback(true, $"The decode package body size is larger than {packageBodyMaxSize} !");
                    break;
                }

                // 读取包体。
                {
                    package.BodyBytes = ringBuffer.ReadBytes(msgBodyLength);
                    outputPackages.Add(package);
                }
            }

            // 注意：将剩余数据移至起始。
            ringBuffer.DiscardReadBytes();
        }
    }
}