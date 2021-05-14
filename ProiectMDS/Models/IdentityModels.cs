﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProiectMDS.Models;
using Task = ProiectMDS.Models.Task;

namespace ProiectMDS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public IEnumerable<SelectListItem> AllRoles { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, ProiectMDS.Migrations.Configuration>("DefaultConnection"));
		}

		public DbSet<Task> Tasks { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Comment> Comments { get; set; }
        public DbSet<Apartine> Apartines { get; set; }
        public DbSet<ApartineT> ApartineTs { get; set; }

		public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}