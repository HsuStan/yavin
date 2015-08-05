using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yavin.Model.Common
{
    /// <summary>
    /// 表示一个通用返回结构
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// 执行成功返回
        /// </summary>
        public static Result SUCCESS
        {
            get
            {
                var model = new Result();
                model.Code = CodeDefine.SUCCESS;
                model.Message = "执行成功";
                return model;
            }
        }
    }
}
