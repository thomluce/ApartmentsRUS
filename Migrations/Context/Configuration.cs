namespace ApartmentsRUS.Migrations.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApartmentsRUS.DAL.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\Context";
            ContextKey = "ApartmentsRUS.DAL.Context";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApartmentsRUS.DAL.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
