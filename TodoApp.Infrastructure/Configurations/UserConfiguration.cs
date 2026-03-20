using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { UserId = 1, Username = "john_doe", Email = "john.doe@example.com", CreatedAt = DateTime.Now },
                new User { UserId = 2, Username = "jane_smith", Email = "jane.smith@example.com", CreatedAt = DateTime.Now }
            );
        }
    }
}
