using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace WGame
{
    public class LoadGameSceneOperation : ILoadingOperation
    {
        public async Task Load()
        {
            await LoadScene();
            InitGame();
        }

        private static async Task LoadScene()
        {
            var op = SceneManager.LoadSceneAsync(Constants.Scenes.Game);

            while (op.isDone == false)
                await Task.Delay(1);
        }

        private static void InitGame()
        {
            Game game = FindGame();
            game.Init();
        }

        private static Game FindGame()
        {
            var scene = SceneManager.GetActiveScene();
            var game = scene.GetRootGameObjects().FirstOrDefault(x => x.GetComponent<Game>()).GetComponent<Game>();
            return game;
        }
    }
}
