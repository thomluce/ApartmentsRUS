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
            // this method is a 'constructor' and is called when a new context is created
            // the base attribute says which connection string to use
        }
        // Include each object here. The value inside <> is the name of the class,
        // the value outside should generally be the plural of the class name
        // and is the name used to reference the entity in code
        public DbSet<Apartment> apartment { get; set; }
        public DbSet<Building> building { get; set; }
        public DbSet<Investor> investor { get; set; }
        public DbSet<Lease> lease { get; set; }
        public DbSet<Owner> owner { get; set; }
        public DbSet<Renter> renter { get; set; }

    }
}