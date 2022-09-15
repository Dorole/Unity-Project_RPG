using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] Transform _spawnPoint;
        [SerializeField] Portal_SO _portalData;

        [SerializeField] float _fadeOutTime = 0.5f;
        [SerializeField] float _fadeInTime = 1f;
        [SerializeField] float _fadeWaitTime = 0.5f;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;

            StartCoroutine(Transition());
        }

        IEnumerator Transition()
        {
            if (_portalData.SceneToLoad < 0)
            {
                Debug.LogError("SceneToLoad not set!");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeOutTime);
            yield return SceneManager.LoadSceneAsync(_portalData.SceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(_fadeWaitTime);
            yield return fader.FadeIn(_fadeInTime);

            Destroy(gameObject);
        }

        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = otherPortal._spawnPoint.rotation;
        }

        Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();

            for (int i = 0; i < portals.Length; i++)
            {
                if (portals[i] == this)
                    continue;

                if (portals[i]._portalData != _portalData.Destination)
                    continue;
 
                return portals[i];
            }

            return null;
        }
    }
}
