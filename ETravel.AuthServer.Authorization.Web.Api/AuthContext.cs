using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ETravel.AuthServer.Authorization.Web.Api
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {

        public AuthContext() : base("AuthContext") {}

        public DbSet<Client> Clients { get; set; }
       
    }
}