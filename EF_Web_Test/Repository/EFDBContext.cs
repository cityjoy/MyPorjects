using EF_Web_Test.Models;
using EF_Web_Test.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EF_Web_Test.Repository
{
    public class EFDBContext : DbContext
    {
        public EFDBContext()
            : base("name=DefaultConnection")
        {

        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new SubjectMap());
            //modelBuilder.Configurations.Add(new SubjectCommentMap());
            //modelBuilder.Configurations.Add(new TopicArticleMap());
  
            //使用反射将程序集中所有的EntityTypeConfiguration<>一次性添加到modelBuilder.Configurations集合中,
            //以后添加新的实体映射只需要添加新的继承自EntityTypeConfiguration<>的XXXMap类而不需要修改OnModelCreating方法。
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                                 .Where(type => !String.IsNullOrEmpty(type.Namespace))
                                 .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
        }
    }
}