using System;
using System.Collections;
using UnityEngine;

namespace WGame
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        private const float VelocityDamping = 0.1f;
        private const float DisappearingTime = 2f;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _transparentMaterial;

        private Animator _animator;
        private Action _hit;
        private Action _attackEnded;
        private bool _isDead;

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
            _animator.SetFloat(Constants.CharacterAnimatorController.Parameters.Velocity, 0);
            _animator.SetTrigger(Constants.CharacterAnimatorController.Parameters.Attack);
        }

        public void SetDie()
        {
            if (_isDead == false)
            {
                _isDead = true;
                _animator.SetTrigger(Constants.CharacterAnimatorController.Parameters.Die);
            }
        }

        public void Disappear(Action callback)
        {
            StartCoroutine(DisappearAnimation(DisappearingTime, callback));
        }

        private IEnumerator DisappearAnimation(float time, Action callback)
        {
            _renderer.sharedMaterial = _transparentMaterial;

            float t = 0;
            Material material = _renderer.material;
            Color color = material.color;
            while (t < 1)
            {
                t += Time.deltaTime / time;
                color.a = Mathf.Lerp(1f, 0f, Ease.OutSine(t));
                material.color = color;
                yield return null;
            }

            callback?.Invoke();
        }

        private void AnimEvent_Hit()
        {
            if (_isDead == false)
                _hit?.Invoke();
        }

        private void AnimEvent_AttackEnded()
        {
            if (_isDead == false)
                _attackEnded?.Invoke();
        }
    }
}