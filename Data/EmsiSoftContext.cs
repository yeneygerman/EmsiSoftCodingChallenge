using EmsiSoft.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EmsiSoftContextDB : DbContext
    {
        public EmsiSoftContextDB(DbContextOptions<EmsiSoftContextDB> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Hashes> Hashes { get; set; }
    }
}
