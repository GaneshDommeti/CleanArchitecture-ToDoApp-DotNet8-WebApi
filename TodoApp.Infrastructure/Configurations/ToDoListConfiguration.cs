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
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
    {
        public void Configure(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasData(
                new ToDoList { ToDoListId = 1, UserId = 1, Title = "John's Personal Tasks", Description = "John's personal task list", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now},
                new ToDoList { ToDoListId = 2, UserId = 2, Title = "Jane's Work Tasks", Description = "Jane's work-related tasks", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );
        }
    }
}
