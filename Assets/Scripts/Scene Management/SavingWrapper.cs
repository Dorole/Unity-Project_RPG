using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float _fadeInTime = 0.3f;
        [SerializeField] KeyCode _saveKey = KeyCode.Keypad1;
        [SerializeField] KeyCode _loadKey = KeyCode.Keypad2;
        [SerializeField] KeyCode _deleteKey = KeyCode.Delete;

        const string DEFAULT_SAVE_FILE = "save";
        SavingSystem _savingSystem;
        Fader _fader;

        void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            _fader = FindObjectOfType<Fader>();

            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(DEFAULT_SAVE_FILE);
            _fader.FadeOutImmediate();
            yield return _fader.CO_FadeIn(_fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(_saveKey))
                Save();

            if (Input.GetKeyDown(_loadKey))
                Load();

            if (Input.GetKeyDown(_deleteKey))
                Delete();
        }
        public void Save()
        {
            _savingSystem.Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            _savingSystem.Load(DEFAULT_SAVE_FILE);
        }

        public void Delete()
        {
            _savingSystem.Delete(DEFAULT_SAVE_FILE);
        }

    }
}
