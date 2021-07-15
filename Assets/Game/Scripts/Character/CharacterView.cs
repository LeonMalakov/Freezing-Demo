using System;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        private const float VelocityDamping = 0.1f;

        private Animator _animator;
        private Action _hit;
        private Action _attackEnded;

        public void Init(Action hit, Action attackEnded)
        {
            _hit = hit;
            _attackEnded = attackEnded;
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

        public void SetAttack()
        {
            _animator.SetTrigger(Constants.CharacterAnimatorController.Parameters.Attack);
        }

        private void AnimEvent_Hit()
        {
            _hit?.Invoke();
        }

        private void AnimEvent_AttackEnded()
        {
            _attackEnded?.Invoke();
        }
    }
}