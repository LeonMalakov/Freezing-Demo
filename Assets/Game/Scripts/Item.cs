using System;
using UnityEngine;

namespace WGame
{
    public class Item : GameBehaviour, IInteractivable
    {
        [SerializeField] [Range(0, 120)] private int _lifeTimeToAdd = 20;

        public int LifeTimeToAdd => _lifeTimeToAdd;
        public Transform Transform => transform;

        public void Init()
        {
        }

        public bool Interact(Player character)
        {
            return character.Grab(this);
        }

        public bool InteractWithItem(Player character, Item item) => false;

        public void DisableCollision()
        {
            GetComponent<Collider>().enabled = false;
        }

        public void EnableCollision()
        {
            GetComponent<Collider>().enabled = true;
        }

        public void Recycle()
        {
            Game.RemoveItem(this);
        }

        public void BecomeActive()
        {
        }

        public void BecomeInactive()
        {
        }
    }
}