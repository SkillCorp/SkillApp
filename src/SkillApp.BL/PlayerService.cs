using SkillApp.BL.Interfaces;

namespace SkillApp.BL
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerManagementService _playerManagementService;
        public PlayerService(IPlayerManagementService playerManagementService)
        {
            _playerManagementService = playerManagementService;
        }

        public string Play()
        {
            return _playerManagementService.GetFirstPlayer().Name;
        }
    }
}
