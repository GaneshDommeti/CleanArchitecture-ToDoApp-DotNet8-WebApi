using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using TodoApp.WebAPI.Filters;

namespace TodoApp.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var useInMemoryDB = builder.Configuration.GetValue<bool>("UseInMemoryDB");

            // Add services to the container.

            //builder.Services.AddControllers(options =>
            //{
            //    options.Filters.Add<GlobalExceptionFilter>();
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // Ensure the Swashbuckle.AspNetCore NuGet package is referenced in the project.
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoListService, ToDoListService>();

            builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });
            if (useInMemoryDB)
            {

                // we could have written that logic here but as per clean architecture, we are separating these into their own piece of code
                builder.Services.AddInMemoryDatabase();
            }
            else
            {
                //use this for real database on your sql server
                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DbContext"),
                    providerOptions => providerOptions.EnableRetryOnFailure()
                    );
                }
                  );
            }

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            if (useInMemoryDB)
            {
                // Seed data. Use this config for in memory database
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    SeedData(context); // Call to seed data
                }
            }
            else
            {
                //User this if you want database on your sql server
                // Automatically create the database if it does not exist. This is required only for real database
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated(); // This creates the database if it doesn't exist
                                                      //Seeding initial data is taken care from OnModelCreating of AppDbContext class.
                }
            }

            app.Run();
        }
        private static void SeedData(AppDbContext context)
        {
            context.Users.AddRange(
                new User { UserId = 1, Username = "JohnDoe", Email = "john@example.com", CreatedAt = DateTime.Now },
                new User { UserId = 2, Username = "JaneDoe", Email = "jane@example.com", CreatedAt = DateTime.Now }
            );

            context.ToDoLists.AddRange(
                new ToDoList { ToDoListId = 1, UserId = 1, Title = "Groceries", Description = "Things to buy", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new ToDoList { ToDoListId = 2, UserId = 1, Title = "Work", Description = "Work-related tasks", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );

            context.ToDoItems.AddRange(
                new ToDoItem { ToDoItemId = 1, ToDoListId = 1, Title = "Buy milk", Description = "Get whole milk", DueDate = DateTime.Now.AddDays(2), IsCompleted = false, Priority = PriorityLevel.Medium, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new ToDoItem { ToDoItemId = 2, ToDoListId = 1, Title = "Buy eggs", Description = "Get a dozen eggs", DueDate = DateTime.Now.AddDays(3), IsCompleted = false, Priority = PriorityLevel.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );

            context.SaveChanges(); // Save changes to the in-memory database
        }
    }
}
