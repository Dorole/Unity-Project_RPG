using UnityEngine.EventSystems;
using UnityEngine;
using RPG.UI.Inventories;

namespace RPG.Core.UI.Tooltips
{
    public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        GameObject _tooltip = null;

        /// <summary>
        /// Call to update information on the tooltip prefab.
        /// </summary>
        public abstract void UpdateTooltip(GameObject tooltip);

        /// <summary>
        /// Returns true when the tooltip spawner should be allowed to create a tooltip.
        /// </summary>
        public abstract bool CanCreateTooltip();

        void OnDestroy()
        {
            ClearTooltip();
        }

        void OnDisable()
        {
            ClearTooltip();
        }

        void AssignTooltip()
        {
            _tooltip = FindObjectOfType<ItemTooltip>(true).gameObject;
        }

        void ClearTooltip()
        {
            if (!_tooltip) return;

            if (_tooltip.activeInHierarchy)
                _tooltip.SetActive(false);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_tooltip) 
                AssignTooltip();

            var parentCanvas = GetComponentInParent<Canvas>();

            if (_tooltip.activeInHierarchy && !CanCreateTooltip())
                ClearTooltip();

            if (!_tooltip.activeInHierarchy && CanCreateTooltip())
                _tooltip.SetActive(true);

            if (_tooltip.activeInHierarchy)
            {
                UpdateTooltip(_tooltip);
                PositionTooltip();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            var slotCorners = new Vector3[4];
            
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);
            Rect rect = new Rect(slotCorners[0], slotCorners[2] - slotCorners[0]);
            if (rect.Contains(eventData.position)) return;

            ClearTooltip();
        }

        void PositionTooltip()
        {
            //Ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            Vector3[] tooltipCorners = new Vector3[4];
            _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);

            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            //quadrant calculations
            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
        }

        /// <summary>
        /// Converts boolean representation of a quadrant into an index. 
        /// </summary>
        /// <param name="below">Below of center.</param>
        /// <param name="right">Right of center.</param>
        int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;
        }

    }
}
