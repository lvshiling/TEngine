using System;

namespace GameLogic.GameProto.SprotoLib
{
    /// <summary>
    /// sproto 示例消息类，由 sproto 生成
    /// </summary>
    public class SprotoExample : SprotoMessage
    {
        public override int MessageId => 1001; // 示例消息ID

        public string Content { get; set; }

        public override byte[] Serialize()
        {
            // 示例序列化逻辑，实际应由 sproto 生成
            return System.Text.Encoding.UTF8.GetBytes(Content);
        }

        public override void Deserialize(byte[] data)
        {
            // 示例反序列化逻辑，实际应由 sproto 生成
            Content = System.Text.Encoding.UTF8.GetString(data);
        }
    }
} 