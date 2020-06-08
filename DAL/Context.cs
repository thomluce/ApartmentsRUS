using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ApartmentsRUS.Models;

namespace ApartmentsRUS.DAL
{
    public class Context: DbContext
    {
        public Context() : base("name=DefaultConnection")
        {
        }
        public DbSet<Apartment> apartment { get; set; }
        public DbSet<Building> building { get; set; }
        public DbSet<Investor> investor { get; set; }
        public DbSet<Lease> lease { get; set; }
        public DbSet<Owner> owner { get; set; }
        public DbSet<Renter> renter { get; set; }
        public DbSet<RegisteredUser> registeredUsers { get; set; }

    }
}