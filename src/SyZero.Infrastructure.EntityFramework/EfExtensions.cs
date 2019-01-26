
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SyZero.Infrastructure.EntityFramework
{
    public static class EfExtensions
    {
        public static IServiceCollection UserMongoLog(this IServiceCollection services,string configurationSection)
        {
            services.AddDbContext<SyDbContext>(options =>
                options.UseSqlServer(configurationSection));
            return services;
        }
    }
}
