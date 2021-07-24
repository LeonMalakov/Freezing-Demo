using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace WGame
{
    public static class MonoBehaviourExtensions
    {
        public static async Task WaitForCoroutine(this MonoBehaviour behaviour, IEnumerator routine)
        {
            var taskCompletition = new TaskCompletionSource<bool>();
            behaviour.StartCoroutine(Coroutine(behaviour, routine, taskCompletition));
            await taskCompletition.Task;
        }

        private static IEnumerator Coroutine(MonoBehaviour behaviour, IEnumerator internalRoutine, TaskCompletionSource<bool> taskCompletion)
        {
            yield return behaviour.StartCoroutine(internalRoutine);
            taskCompletion.SetResult(true);
        }
    }
}
