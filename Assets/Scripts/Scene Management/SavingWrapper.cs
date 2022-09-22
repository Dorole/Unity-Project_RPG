using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float _fadeInTime = 0.3f;

        const string DEFAULT_SAVE_FILE = "save";
        SavingSystem _savingSystem;
        Fader _fader;

        private void Awake()
        {
            _savingSystem = GetComponent<SavingSystem>();
            _fader = FindObjectOfType<Fader>();
        }

        IEnumerator Start()
        {
            _fader.FadeOutImmediate();
            yield return _savingSystem.LoadLastScene(DEFAULT_SAVE_FILE);
            yield return _fader.FadeIn(_fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();

            if (Input.GetKeyDown(KeyCode.L))
                Load();
        }
        public void Save()
        {
            _savingSystem.Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            _savingSystem.Load(DEFAULT_SAVE_FILE);
        }

    }
}
