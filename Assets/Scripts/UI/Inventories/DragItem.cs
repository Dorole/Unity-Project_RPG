using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Core.UI.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class
    {
        Vector3 _startPosition;
        Transform _originalParent;
        IDragSource<T> _source;

        Canvas _parentCanvas;

        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            _source = GetComponentInParent<IDragSource<T>>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;

            //else won't get drop event
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(_parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(_originalParent, true);

            IDragDestination<T> container;
            if (!EventSystem.current.IsPointerOverGameObject())
                container = _parentCanvas.GetComponent<IDragDestination<T>>();
            else
                container = GetContainer(eventData);

            if (container != null)
                DropItemIntoContainer(container);
        }

        IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
                return container;
            }
            return null;
        }
        void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (ReferenceEquals(destination, _source)) return;

            var destinationContainer = destination as IDragContainer<T>;
            var sourceContainer = _source as IDragContainer<T>;

            if (destinationContainer == null || sourceContainer == null || 
                sourceContainer.GetItem() == null || 
                ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }


            AttemptSwap(destinationContainer, sourceContainer);
        }

        void AttemptSwap(IDragContainer<T> destinationContainer, IDragContainer<T> sourceContainer)
        {
            var removedSourceNumber = sourceContainer.GetNumber();
            var removedSourceItem = sourceContainer.GetItem();
            var removedDestinationNumber = destinationContainer.GetNumber();
            var removedDestinationItem = destinationContainer.GetItem();

            //temporarily remove items from both containers
            sourceContainer.RemoveItems(removedSourceNumber);
            destinationContainer.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, sourceContainer, destinationContainer);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destinationContainer, sourceContainer);

            //do take backs, if needed
            if (sourceTakeBackNumber > 0)
            {
                sourceContainer.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destinationContainer.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber-= destinationTakeBackNumber;
            }

            //abort if a swap can't be done
            if (sourceContainer.MaxAcceptable(removedDestinationItem) < removedDestinationNumber || 
                destinationContainer.MaxAcceptable(removedSourceItem) < removedSourceNumber ||
                removedSourceNumber == 0)
            {
                if (removedDestinationNumber > 0)
                    destinationContainer.AddItems(removedDestinationItem, removedDestinationNumber);

                if (removedSourceNumber > 0)
                    sourceContainer.AddItems(removedSourceItem, removedSourceNumber);

                return;
            }

            //do swaps
            if (removedDestinationNumber > 0)
                sourceContainer.AddItems(removedDestinationItem, removedDestinationNumber);

            if (removedSourceNumber > 0)
                destinationContainer.AddItems(removedSourceItem, removedSourceNumber);
        }
        
        void AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = _source.GetItem();
            var draggingAmount = _source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(draggingAmount, acceptable);

            if (toTransfer <= 0) return;
            else
            {
                _source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
            }
        }

        int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removalSource, IDragContainer<T> destination)
        {
            int takeBackNumber = 0;
            int destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                int sourceTakeBackAcceptable = removalSource.MaxAcceptable(removedItem);

                //abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber)
                    return 0;
            }

            return takeBackNumber;
        }

    }
}
