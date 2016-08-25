using System.Collections.Generic;
using System.Web.Http;
using WebAPITest.Models.Entities;

namespace WebAPITest.Controllers
{
    public class OrderController : ApiController
    {
        //
        // GET: /Order/
        /// <summary>
        /// 获取订单列表,需要授权
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public List<Order> GetList()
        {
            return Order.CreateOrders();
        }
        /// <summary>
        /// 根据用户名称获取订单,需要授权
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public Order Get(string name)
        {
            return Order.CreateOrders().Find(o=>o.CustomerName==name);
        }
    }
}