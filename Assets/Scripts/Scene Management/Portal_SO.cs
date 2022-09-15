using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    [CreateAssetMenu(fileName = "New Portal", menuName = "Portal")]
    public class Portal_SO : ScriptableObject
    {
        [SerializeField] int _sceneToLoad;
        public int SceneToLoad => _sceneToLoad;

        [SerializeField] Portal_SO _destination;
        public Portal_SO Destination => _destination;
    }
}
