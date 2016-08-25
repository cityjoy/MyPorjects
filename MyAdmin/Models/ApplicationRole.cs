using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MyAdmin.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationRole : IdentityRole
    {
        [Display(Name = "描述")]
        [Required(ErrorMessage="描述信息不能为空")]
        public string Description { get; set; }

        public ApplicationRole() : base() {}

        public ApplicationRole(string name) : base(name) { 
        
        }

    }

}