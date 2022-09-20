using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);

            if (Input.GetKeyDown(KeyCode.L))
                GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }
    }
}
