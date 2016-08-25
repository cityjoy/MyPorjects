﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebAPITest.Models.Entities;

namespace WebAPITest.Models
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext():base("AuthContext")
        {
            
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}