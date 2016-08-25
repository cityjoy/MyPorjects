using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MyAdmin.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 默认表名是AspNetUsers,我们可以把它改成任意我们想要的
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");
            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles");
            modelBuilder.Entity<ApplicationRole>()
                .ToTable("Roles");
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }


}