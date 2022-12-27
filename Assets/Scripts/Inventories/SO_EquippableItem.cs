using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Inventory/Equippable Item"))]
    public class SO_EquippableItem : SO_InventoryItem
    {
        [SerializeField] EquipLocation _allowedEquipLocation = EquipLocation.Weapon;
        public EquipLocation AllowedEquipLocation => _allowedEquipLocation;
    }
}
