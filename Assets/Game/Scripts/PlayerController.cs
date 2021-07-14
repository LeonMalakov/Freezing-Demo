using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Character))]
    public class PlayerController : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            Vector2 input = new Vector2(
                x: Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0,
                y: Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0);

            _character.SetMove(input);
        }
    }
}