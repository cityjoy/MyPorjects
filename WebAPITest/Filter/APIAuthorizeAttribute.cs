using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace WebAPITest.Filter
{
    /// <summary>
    /// 自定义实现令牌校验功能
    /// </summary>
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写基类的验证方式，加入我们自定义的Ticket验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //url获取token
            var token = actionContext.Request.Headers.Authorization.Parameter;

            if (!string.IsNullOrEmpty(token))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                if (ValidateTicket(token))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous)
                {
                    base.OnAuthorization(actionContext);
                }
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //校验用户名密码（对Session匹配，或数据库数据匹配）
        private bool ValidateTicket(string encryptToken)
        {
            //解密Ticket
            var strTicket = FormsAuthentication.Decrypt(encryptToken).UserData;

            //从Ticket里面获取用户名和密码
            var index = strTicket.IndexOf("&");
            string userName = strTicket.Substring(0, index);
            string password = strTicket.Substring(index + 1);
            //取得session，不通过说明用户退出，或者session已经过期
            //var token = HttpContext.Current.Session[userName];
            var token = MembercacheHelper.GetCache(userName);

            if (token == null)
            {
                return false;
            }
            //对比session中的令牌
            if (token.ToString() == encryptToken)
            {
                return true;
            }

            return false;

        }
    }
}