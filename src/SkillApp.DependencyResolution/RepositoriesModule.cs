using Microsoft.Extensions.DependencyInjection;

using SkillApp.Data.Interfaces;
using SkillApp.Data.Repositories;

namespace SkillApp.DependencyResolution
{
    public static class RepositoriesModule
    {
        public static void AddCustomRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPlayerRepository, PlayerRepository>();
        }
    }
}
