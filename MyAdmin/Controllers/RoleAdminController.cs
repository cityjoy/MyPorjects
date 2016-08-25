using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using MyAdmin.Models;
using System.ComponentModel.DataAnnotations;

namespace MyAdmin.Controllers
{
    public class RoleAdminController : Controller
    {
        //
        // GET: /RoleAdmin/

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(ApplicationRole role)
        {
            PageActionResult actionResult = new PageActionResult();
            if (ModelState.IsValid)
            {
                IdentityResult result
                   = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    actionResult.Result = PageActionResultType.Success;
                    actionResult.Message = "添加成功";
                }
                else
                {

                    actionResult.Result = PageActionResultType.Failed;
                    actionResult.Message = "添加失败";
                    AddErrorsFromResult(result, actionResult);
                }
            }
            else
            {
                actionResult.Result = PageActionResultType.Failed;
                actionResult.Message = "添加失败,参数错误";
            }
            return Json(actionResult);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(string id)
        {
            PageActionResult actionResult = new PageActionResult();

            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    actionResult.Result = PageActionResultType.Success;
                    actionResult.Message = "添加成功";
                }
                else
                {
                    actionResult.Result = PageActionResultType.Failed;
                    actionResult.Message = "添加失败";
                    AddErrorsFromResult(result, actionResult);
                }
            }
            else
            {
                actionResult.Result = PageActionResultType.Failed;
                actionResult.Message = "添加失败,不存在";
            }
            return Json(actionResult);
        }
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<ApplicationUser> members
                    = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId,
                       model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
        }
        private void AddErrorsFromResult(IdentityResult result, PageActionResult actionResult)
        {
            //foreach (string error in result.Errors) {

            //     ModelState.AddModelError("", error);
            //}
            foreach (string error in result.Errors)
            {

                actionResult.Message += error + "|";
            }
        }


        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

    }
}
