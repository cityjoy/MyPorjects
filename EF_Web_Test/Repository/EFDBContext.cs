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
        public new DbSet<TEntity> Set<TEntity>() where TEntity :class
        {
            return base.Set<TEntity>();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SubjectMap());
            modelBuilder.Configurations.Add(new SubjectCommentMap());
            modelBuilder.Configurations.Add(new TopicArticleMap());  
        }
    }
}