namespace MicwayTechTest.Migrations
{
    using MicwayTechTest.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MicwayTechTest.Context.DriverContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MicwayTechTest.Context.DriverContext context)
        {
            //Generating fake data to populate the Driver table
            //Commands on Package Manager Console:
            //Enable-Migrations
            //Add-Migration Initial
            //Update - Database
            context.Drivers.AddOrUpdate(x => x.Id,
                new Driver() { Id = 1, FirstName = "Christian", LastName = "Cage", DateOfBirth = new DateTime(1982,10,03), Email = "christiancage@email.com" },
                new Driver() { Id = 1, FirstName = "Eduardo", LastName = "Santos", DateOfBirth = new DateTime(1980, 01, 01), Email = "eduardosantos@email.com" },
                new Driver() { Id = 1, FirstName = "Luke", LastName = "Hold", DateOfBirth = new DateTime(1972, 11, 11), Email = "lukehold@email.com" },
                new Driver() { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(1990, 12, 03), Email = "christiancage@email.com" }
                );
        }
    }
}
