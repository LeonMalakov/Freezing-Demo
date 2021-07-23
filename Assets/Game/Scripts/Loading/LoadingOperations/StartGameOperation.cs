using System.Threading.Tasks;

namespace WGame
{
    public class StartGameOperation : ILoadingOperation
    {
        public async Task Load()
        {
            await Game.Play();
        }
    }
}
