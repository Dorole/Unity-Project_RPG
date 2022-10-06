using UnityEngine;

namespace RPG.Core
{
    public class TimedObjectDestructor : MonoBehaviour
    {
		public float timeOut = 7f;
		public bool detachChildren = false;

		void Awake()
		{
			Invoke("DestroyNow", timeOut);
		}

		void DestroyNow()
		{
			if (detachChildren)
				transform.DetachChildren();

			Destroy(gameObject);
		}
	}
}
