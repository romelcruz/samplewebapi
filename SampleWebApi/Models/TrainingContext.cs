using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Models
{
    public class TrainingContext : DbContext
    {

        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options)
        {

        }

        public DbSet<Training> Trainings { get; set; }
    }
}
