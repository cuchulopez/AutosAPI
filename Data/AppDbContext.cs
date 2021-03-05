using _1eraAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Data
{
    public class AppDbContext : DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
       {
        }
       public DbSet<Marcas> Marcas { get; set; }
       public DbSet<Modelos> Modelos { get; set; }
       public DbSet<Usuarios> Usuarios { get; set; }
    }
}
 