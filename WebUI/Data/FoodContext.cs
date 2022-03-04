using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebUI.Models;

    public class FoodContext : DbContext
    {
        public FoodContext (DbContextOptions<FoodContext> options)
            : base(options)
        {
        }

        public DbSet<WebUI.Models.Order> Order { get; set; }
    }
