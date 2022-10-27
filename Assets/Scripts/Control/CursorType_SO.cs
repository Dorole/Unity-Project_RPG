using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(fileName="New Cursor", menuName ="Cursor")]
    public class CursorType_SO : ScriptableObject
    {
        [SerializeField] Texture2D _texture;
        [SerializeField] Vector2 _hotspot;

        public void SetCursor()
        {
            if (_texture != null)
                Cursor.SetCursor(_texture, _hotspot, CursorMode.Auto);
        }
    }
}
