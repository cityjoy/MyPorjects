using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPITest.Models.Entities
{
    public class TokenData
    {

        public int ErrorCode { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}