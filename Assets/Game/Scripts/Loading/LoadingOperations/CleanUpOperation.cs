using System.Threading.Tasks;

namespace WGame
{
    public class CleanUpOperation : ILoadingOperation
    {
        private readonly GameScenario _gameScenario;

        public CleanUpOperation(GameScenario gameScenario)
        {
            _gameScenario = gameScenario;
        }

        public async Task Load()
        {
            await _gameScenario.CleanUpAsync();
        }
    }
}
