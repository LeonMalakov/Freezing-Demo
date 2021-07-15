using UnityEngine;

namespace WGame
{
    public class Item : Interactivable
    {
        [SerializeField] [Range(0, 120)] private float _lifeTimeToAdd = 20;

        public float LifeTimeToAdd => _lifeTimeToAdd;

        public override bool Interact(Character character)
        {
            return character.Grab(this);
        }

        public override bool InteractWithItem(Character character, Item item) => false;

        public void Recycle()
        {
            Destroy(gameObject);
        }
    }
}