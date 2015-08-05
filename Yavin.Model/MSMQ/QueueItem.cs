using System;

namespace Yavin.Model.MSMQ
{
    /// <summary>
    /// 表示一个MSMQ消息数据
    /// </summary>
    public class QueueItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
    }
}
