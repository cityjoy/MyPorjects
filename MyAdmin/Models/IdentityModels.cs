//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System.Data.Entity;

//namespace MyAdmin.Models
//{
//    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
//    public class ApplicationUser : IdentityUser
//    {
//        public string FirstName { get; set; }
//        public string LastName { get; set; }
//        public int Age { get; set; }
//        public string City { get; set; }
//        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
//        {
//            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
//            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
//            // 在此处添加自定义用户声明
//            return userIdentity;
//        }
//    }

//    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
//    {
//        public ApplicationDbContext()
//            : base("DefaultConnection", throwIfV1Schema: false)
//        {
//            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库
//            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
//        }
//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // 默认表名是AspNetUsers,我们可以把它改成任意我们想要的
//            modelBuilder.Entity<IdentityUser>()
//                .ToTable("Users");
//            modelBuilder.Entity<ApplicationUser>()
//                .ToTable("Users");
//        }
//        public static ApplicationDbContext Create()
//        {
//            return new ApplicationDbContext();
//        }
//    }

//    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
//    {
//        // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库
//        protected override void Seed(ApplicationDbContext context)
//        {
//            InitializeIdentityForEF(context);
//            base.Seed(context);
//        }

//        //创建用户名为admin@123.com，密码为“Admin@123456”并把该用户添加到角色组"Admin"中
//        public static void InitializeIdentityForEF(ApplicationDbContext context)
//        {
//            var roleStore = new RoleStore<IdentityRole>(context);
//            var roleManager = new RoleManager<IdentityRole>(roleStore);
//            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

//            const string name = "admin@123.com";//用户名
//            const string password = "Admin@123456";//密码
//            const string roleName = "Admin";//用户要添加到的角色组

//            //如果没有Admin用户组则创建该组
//            if (!roleManager.RoleExists(roleName))
//            {
//                var IdRoleResult = roleManager.Create(new IdentityRole { Name = roleName });
//            }
//            //如果没有admin@123.com用户则创建该用户
//            var appUser = new ApplicationUser { UserName = name, Email = name };
//            var IdUserResult = userManager.Create(appUser, password);

//            // 把用户admin@123.com添加到用户组Admin中
//            if (!userManager.IsInRole(userManager.FindByEmail(name).Id, roleName))
//            {
//                IdUserResult = userManager.AddToRole(userManager.FindByEmail(name).Id, roleName);
//            }
//        }
//    }
//}