using UnityEngine;

namespace RPG.Inventories
{
    public interface ICollectable 
    {
        void HandleCollection();
        Transform GetTransform();
    }
}
