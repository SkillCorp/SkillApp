using SkillApp.Data.Interfaces;
using SkillApp.Data.Repositories.Base;
using SkillApp.Entities.Entities;

namespace SkillApp.Data.Repositories
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}
