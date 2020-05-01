using DrinkAndGo.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkAndGo.Data
{
    public static class DbInitializer
    {
      
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                 new Category()
                 {
                     CategoryId = 1,
                     CategoryName = "Alcoholic",
                     Description = "All alcoholic drinks"
                 },
                 new Category()
                 {
                     CategoryId = 2,
                     CategoryName = "Non-alcoholic",
                     Description = "All non-alcoholic drinks"
                 }
              );
        }
    }
}
