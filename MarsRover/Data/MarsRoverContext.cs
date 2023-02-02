using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MarsRover.Models;

namespace MarsRover.Data
{
    public class MarsRoverContext : DbContext
    {
        public MarsRoverContext (DbContextOptions<MarsRoverContext> options)
            : base(options)
        {
        }

        public DbSet<MarsRover.Models.Rover> Rover { get; set; } = default!;
    }
}
