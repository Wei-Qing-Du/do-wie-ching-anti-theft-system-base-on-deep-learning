using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anti_thief.Models
{
    public class RecordbContext : DbContext
    {
        public RecordbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Record> Recorder { get; set; }
    }
}
