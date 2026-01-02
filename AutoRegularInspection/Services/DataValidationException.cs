using System;
using System.Collections.Generic;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 提供数据校验错误的集合。
    /// </summary>
    public class DataValidationException : Exception
    {
        public List<string> Errors { get; }

        public DataValidationException(List<string> errors)
            : base("数据校验失败")
        {
            Errors = errors ?? new List<string>();
        }
    }
}
