using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yavin.Model.Common
{
    /// <summary>
    /// 返回码定义
    /// </summary>
    public class CodeDefine
    {
        /// <summary>
        /// 执行成功返回码
        /// </summary>
        public const string SUCCESS = "200";

        /// <summary>
        /// 被捕获错误
        /// </summary>
        public const string ERROR = "400";

        /// <summary>
        /// 必须显示错误信息的捕获错误
        /// </summary>
        public const string ERROR_WITH_MESSAGE = "501";

        /// <summary>
        /// 用户未登录错误
        /// </summary>
        public const string USER_LOGIN_FAIL = "401";

        /// <summary>
        /// 用户权限不足错误
        /// </summary>
        public const string USER_AUTHOR_FAIL = "403";

        /// <summary>
        /// 未知错误
        /// </summary>
        public const string SERVER_ERROR = "520";
    }

    public class Param
    {
		/// <summary>
		/// 默认字符串分隔符
		/// </summary>
		public const char DEFAULT_SPLITOR = '|';

		/// <summary>
		/// 用来传递JSON序列化参数的head键名
		/// </summary>
		public const string JSON_HEADER_NAME = "Yavin_Serialize_Param";
    }
}
