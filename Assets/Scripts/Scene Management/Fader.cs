using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup _canvasGroup;
        Coroutine _currentActiveFade;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator CO_FadeOut(float time)
        {
            return CO_Fade(1, time);
        }

        public IEnumerator CO_FadeIn(float time)
        {
            return CO_Fade(0, time);
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        IEnumerator CO_FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        IEnumerator CO_Fade(float target, float time)
        {
            if (_currentActiveFade != null)
                StopCoroutine(_currentActiveFade);

            _currentActiveFade = StartCoroutine(CO_FadeRoutine(target, time));
            yield return _currentActiveFade;
        }
    }
}
