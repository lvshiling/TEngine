using System;

namespace GameLogic.GameProto.SprotoLib
{
    /// <summary>
    /// sproto 消息基类，所有 sproto 生成的类应继承此类
    /// </summary>
    public abstract class SprotoMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public abstract int MessageId { get; }

        /// <summary>
        /// 序列化消息为字节数组
        /// </summary>
        /// <returns>序列化后的字节数组</returns>
        public abstract byte[] Serialize();

        /// <summary>
        /// 反序列化字节数组为消息对象
        /// </summary>
        /// <param name="data">要反序列化的字节数组</param>
        public abstract void Deserialize(byte[] data);
    }
} 