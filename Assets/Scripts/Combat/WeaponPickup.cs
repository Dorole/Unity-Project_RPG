using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] CursorType_SO _cursor;
        [SerializeField] WeaponConfig_SO _weapon;
        [SerializeField] float _respawnTime = 5f;
        [SerializeField] float _healthToRestore = 0f;

        //void OnTriggerEnter(Collider other)
        //{
        //    if (!other.CompareTag("Player")) return;

        //    Pickup(other.gameObject);
        //}

        private void Pickup(GameObject collector)
        {
            if (_weapon != null)
                collector.GetComponent<Fighter>().EquipWeapon(_weapon);

            if (_healthToRestore > 0)
                collector.GetComponent<Health>().Heal(_healthToRestore);

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

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType_SO GetCursorType()
        {
            return _cursor;
        }
    }
}
