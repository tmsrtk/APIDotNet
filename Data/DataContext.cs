using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDotNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIDotNet.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<NationalPark> NationalParks { get; set; }
    }
}