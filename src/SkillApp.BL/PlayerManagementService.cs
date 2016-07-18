using System.Linq;

using SkillApp.BL.Base;
using SkillApp.BL.Interfaces;
using SkillApp.Data.Interfaces;
using SkillApp.Entities.Entities;

namespace SkillApp.BL
{
    public class PlayerManagementService : BaseManagementService<Player, IPlayerRepository>, IPlayerManagementService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerManagementService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public override IPlayerRepository Repository
        {
            get { return _playerRepository; }
        }

        public Player GetFirstPlayer()
        {
            return _playerRepository.GetAll().FirstOrDefault();
        }
    }
}
