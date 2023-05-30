using UnityEngine;

namespace RPG.Control
{
    public class ShaderSwitch : MonoBehaviour
    {
        [SerializeField] Shader _outlineShader;
        [SerializeField] GameObject _selectableObject;

        Shader _defaultShader;
        Renderer _renderer;

        private void Start()
        {
            if (_selectableObject == null)
                _renderer = GetComponentInChildren<Renderer>();
            else
                _renderer = _selectableObject.GetComponent<Renderer>();
            _defaultShader = _renderer.material.shader;
        }

        private void OnMouseEnter()
        {
            if (!enabled) return;
            _renderer.material.shader = _outlineShader;
        }

        private void OnMouseExit()
        {
            if (!enabled) return;
            _renderer.material.shader = _defaultShader;
        }
    }
}
