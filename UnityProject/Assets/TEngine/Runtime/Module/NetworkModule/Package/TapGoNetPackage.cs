namespace TEngine
{
    /// <summary>
    /// 默认网络数据包实现。
    /// 用于在网络通信中封装基础消息结构。
    /// </summary>
    /// <remarks>
    /// 实现 <see cref="INetPackage"/> 接口，提供消息ID与二进制载荷的标准容器。
    /// 通常配合编解码器（Encoder/Decoder）进行序列化与反序列化操作。
    /// </remarks>
    public class TapGoNetPackage : INetPackage
    {
        /// <summary>
        /// 消息Length, big endien
        /// <para>整型消息Length，用于：</para>
        /// </summary>
        public ushort MsgLen { set; get; }

        /// <summary>
        /// 消息体原始字节数据。
        /// </summary>
        public byte[] BodyBytes { set; get; }
    }
}