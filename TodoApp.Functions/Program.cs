using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        bool useInMemoryDB = configuration.GetValue<bool>("UseInMemoryDB");  

        if (useInMemoryDB)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TodoDb"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbContext")));
        }

        // Application services
        services.AddScoped<IToDoItemService, ToDoItemService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IToDoListService, ToDoListService>();

        // Repositories
        services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IToDoListRepository, ToDoListRepository>();
    })
    .Build();

host.Run();