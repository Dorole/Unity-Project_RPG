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
            _renderer = GetComponentInChildren<Renderer>();
            _defaultShader = _renderer.material.shader;
        }

        private void OnMouseEnter()
        {
            _renderer.material.shader = _outlineShader;
        }

        private void OnMouseExit()
        {
            _renderer.material.shader = _defaultShader;
        }
    }
}
