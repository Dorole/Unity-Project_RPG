using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core.UI.Dragging
{
    /// <summary>
    /// Can be both a source and a destination for dragging.
    /// If dragging happens between two containers, should check for swap.
    /// </summary>
    public interface IDragContainer<T> : IDragSource<T>, IDragDestination<T> where T : class
    {
    }
}
