using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using WebAPITest.Models;
using WebAPITest.Models.Entities;

namespace WebAPITest.Controllers
{

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly AuthRepository _authRepository = null;

        public AccountController()
        {
            _authRepository = new AuthRepository();
        }

        // POST api/Account/Register
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _authRepository.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //api/Account/Register
        [HttpGet]
        public TokenData Login(string userName, string password)
        {
            //实际场景应该到数据库进行校验
            if (userName == "123" && password == "123")
            {


                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, userName, DateTime.Now,
                                DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", userName, password),
                                FormsAuthentication.FormsCookiePath);
                //返回登录结果、用户信息、用户验证票据信息
                var token = FormsAuthentication.Encrypt(ticket);
                //将身份信息保存在session中，验证当前请求是否是有效请求
                //HttpContext.Current.Session[userName] = token;

                //将身份信息保存在Membercache中，验证当前请求是否是有效请求
                MembercacheHelper.AddCache(userName, token, 10);
                try
                {
                    var m = MembercacheHelper.GetCache(userName).ToString();
                }
                catch(Exception  ex)
                {
                    TokenData tokendata1 = new TokenData() { ErrorCode = -1, Message = ex.ToString(), Token = "" };

                return tokendata1;
                }

                //写入cooike
                HttpCookie tokenCookie = new HttpCookie("Token");
                tokenCookie.Value = token;
                tokenCookie.Expires = DateTime.Now.Add(new TimeSpan(24, 0, 0));
                tokenCookie.Path = "/";
                HttpContext.Current.Response.AppendCookie(tokenCookie);

                TokenData tokendata = new TokenData() { ErrorCode = 0, Message = "登录成功", Token = token };

                return tokendata;
            }
            else
            {
                TokenData tokendata = new TokenData() { ErrorCode = -1, Message = "用户名或密码错误", Token = "" };

                return tokendata;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
