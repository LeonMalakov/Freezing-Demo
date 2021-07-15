using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        private const float VelocityDamping = 0.1f;

        private Animator _animator;

        public void Init()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetVelocity(float speed)
        {
            _animator.SetFloat(Constants.CharacterAnimatorController.Parameters.Velocity, speed, VelocityDamping, Time.deltaTime);
        }

        public void SetIsLoaded(bool isLoaded)
        {
            _animator.SetBool(Constants.CharacterAnimatorController.Parameters.IsLoaded, isLoaded);
        }
    }
}