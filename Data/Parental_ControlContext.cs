using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parental_Control.Models;

namespace Parental_Control.Data
{
    public class Parental_ControlContext : DbContext
    {
        public Parental_ControlContext (DbContextOptions<Parental_ControlContext> options)
            : base(options)
        {
        }

        public DbSet<Parental_Control.Models.Users>? Users { get; set; }
    }
}
