using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon_SO _weapon;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Fighter fighter = other.GetComponent<Fighter>();
            fighter.EquipWeapon(_weapon);

            gameObject.SetActive(false);
        }
    }
}
