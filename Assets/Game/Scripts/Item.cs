using UnityEngine;

namespace WGame
{
    public class Item : GameBehaviour, IInteractivable
    {
        [SerializeField] [Range(0, 120)] private float _lifeTimeToAdd = 20;

        public IItemsOwner OriginFactory { get; set; }

        public float LifeTimeToAdd => _lifeTimeToAdd;

        public Vector3 Position => transform.position;

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
            OriginFactory.Reclaim(this);
        }

        public void BecomeActive()
        {
        }

        public void BecomeInactive()
        {
        }
    }
}