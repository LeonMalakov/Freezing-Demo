using System;
using UnityEngine;

namespace WGame
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _screen;

        private static Loading instance;

        private bool _isLoading;

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public static void LoadGame()
        {
            Load(
                new LoadGameSceneOperation(),
                new StartGameOperation());
        }

        public static void RestartGame(GameScenario gameScenario)
        {
            Load(
                new CleanUpOperation(gameScenario),
                new StartGameOperation());
        }

        private static async void Load(params ILoadingOperation[] loadingOperations)
        {
            if (instance._isLoading)
                throw new InvalidOperationException();

            instance._isLoading = true;

            await instance._screen.Load(loadingOperations);

            instance._isLoading = false;
        }
    }
}
