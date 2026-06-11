
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Pioneer_Education.API.Extensions;
using Pioneer_Education.Core.Contracts;
using Pioneer_Education.Infrastructure.Data.Contexts;
using Pioneer_Education.Infrastructure.Jobs;
using Pioneer_Education.Infrastructure.Repository;
using Pioneer_Education.Services.Abstraction;
using Pioneer_Education.Services.Helpers;
using Pioneer_Education.Services.MappingProfiles;
using Pioneer_Education.Services.Services;
using System.Threading.Tasks;

namespace Pioneer_Education.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.....

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(A =>
            {
                A.AllowNullCollections = true;
            }, typeof(ServiceAssemblyRefernce).Assembly);

            //HangFire Register
            builder.Services.AddHangfire(opt =>
            {
                opt.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHangfireServer();

            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddTransient<IAttacehmentService, AttatchmentService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            var app = builder.Build();

            await app.MigrateDataBaseAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseHangfireDashboard();


            RecurringJob.AddOrUpdate<CourseStatusJob>
                ("course-status-job", job => job.UpdateCourseStatusAsync(), Cron.Daily);

            app.MapControllers();

            app.Run();
        }
    }
}
