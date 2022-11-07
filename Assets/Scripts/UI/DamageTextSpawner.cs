using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText _damageTextPrefab = null;

        //Unity Event
        public void Spawn(float damageText)
        {
            DamageText instance = Instantiate<DamageText>(_damageTextPrefab, transform);
            instance.SetText(damageText);
        }
    }
}
