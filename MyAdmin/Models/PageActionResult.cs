using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAdmin.Models
{
    public class PageActionResult
    {
        public PageActionResult()
        {
        }

        public PageActionResult(PageActionResultType resultType)
        {
            this.Result = resultType;
            this.ErrorCode = 0;
            this.Message = string.Empty;
            this.Keywords = string.Empty;
        }

        public PageActionResult(PageActionResultType resultType, object data)
        {
            this.Result = resultType;
            this.ErrorCode = 0;
            this.Message = string.Empty;
            this.Keywords = string.Empty;
            this.Data = data;
        }

        public PageActionResult(PageActionResultType resultType, string message, int errorCode)
        {
            this.Result = resultType;
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Keywords = string.Empty;
        }

        public PageActionResult(PageActionResultType resultType, string message, int errorCode, string keywords)
        {
            this.Result = resultType;
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Keywords = keywords;
        }

        public PageActionResult(PageActionResultType resultType, string message, int errorCode, string keywords, object data)
        {
            this.Result = resultType;
            this.ErrorCode = errorCode;
            this.Message = message;
            this.Keywords = keywords;
            this.Data = data;
        }

        public virtual object Data { get; set; }

        public virtual int ErrorCode { get; set; }

        public virtual string Keywords { get; set; }

        public virtual string Message { get; set; }

        public virtual PageActionResultType Result { get; set; }
    }
    public enum PageActionResultType
    {
        Default = 0,
        Failed = -1,
        Success = 1
    }
}