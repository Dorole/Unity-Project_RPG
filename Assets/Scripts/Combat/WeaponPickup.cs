using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon_SO _weapon;
        [SerializeField] float _respawnTime = 5f;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Fighter fighter = other.GetComponent<Fighter>();
            fighter.EquipWeapon(_weapon);

            //gameObject.SetActive(false);
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}
