using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebAPITest.Models
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 对第三方应用进行认证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();//企业自己的应用，所以略了颁发appKey和appSecrect的环节，认为是合法的
            //string client_id;
            //string client_secret;
            //context.TryGetFormCredentials(out client_id, out client_secret);
            //if (client_id == "llj" && client_secret == "123456")
            //{
            //    context.Validated(client_id);
            //}
            //else
            //{
            //    //context.Response.StatusCode = Convert.ToInt32(HttpStatusCode.OK);
            //    context.SetError("invalid_client", "client is not valid");
            //}
            return base.ValidateClientAuthentication(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, "App"));
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);//获取用户信息,这里使用

                if (user == null)
                {
                    context.SetError("invalid_grant", "无效的用户名或密码");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            var properties = new AuthenticationProperties(new Dictionary<string, string>//返回给客户端的额外信息放这里
                {
                    { 
                        "client_id", context.ClientId ?? string.Empty
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}