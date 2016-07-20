using Microsoft.Extensions.DependencyInjection;

using SkillApp.BL;
using SkillApp.BL.Interfaces;

namespace SkillApp.DependencyResolution
{
    public static class ServicesModule
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayerManagementService, PlayerManagementService>();
        }
    }
}
