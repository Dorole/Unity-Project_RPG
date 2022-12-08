using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] GameObject _uiContainer;
        [SerializeField] KeyCode _toggleKey;
        //TO DO: add on screen buttons for opening/closing

        void Start()
        {
            _uiContainer.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(_toggleKey))
                _uiContainer.SetActive(!_uiContainer.activeSelf);
        }
    }
}
