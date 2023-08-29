using Application.Abstractions.Services;
using Application.Repositories.ExerciseRepositories;
using Application.Repositories.WorkoutRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Concretes.Repositories.ExerciseRepositories;
using Persistance.Concretes.Repositories.WorkoutRepositories;
using Persistance.Concretes.Services;
using Persistance.Context.MongoDbContext;

namespace Persistance
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<MongoSettings>(opt =>
            {
                opt.ConnectionString = cfg.GetSection("MongoConnection:ConnectionString").Value;
                opt.Database = cfg.GetSection("MongoConnection:Database").Value;
            });
            services.AddScoped<IWorkoutReadRepository, WorkoutReadRepository>();
            services.AddScoped<IWorkoutWriteRepository, WorkoutWriteRepository>();
            
            services.AddScoped<IExerciseReadRepository, ExerciseReadRepository>();
            services.AddScoped<IExerciseWriteRepository, ExerciseWriteRepository>();
            
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IExerciseService, ExerciseService>();

            return services;
        }
    }
}
