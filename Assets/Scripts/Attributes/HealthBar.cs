using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        Health _health;
        [SerializeField] RectTransform _foregroundImage = null;
        [SerializeField] Canvas _rootCanvas = null;

        private void Start()
        {
            _health = GetComponentInParent<Health>();
            UpdateHealthBar();
        }

        public void UpdateHealthBar()
        {
            if (!_rootCanvas) 
                Debug.LogWarning("HealthBar canvas not assigned!");

            if (Mathf.Approximately(_health.GetFraction(), 1) || Mathf.Approximately(_health.GetFraction(), 0))
                _rootCanvas.enabled = false;
            else
                _rootCanvas.enabled = true;

            _foregroundImage.localScale = new Vector3(_health.GetFraction(), 1, 1);
        }
    }
}
