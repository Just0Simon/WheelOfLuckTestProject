using System.Collections.Generic;
using UnityEngine;

namespace WheelOfLuck 
{
    [CreateAssetMenu(fileName = "WheelItemsPack", menuName = "WheelItemsPack", order = 1)]
    public class WheelItemsPack : ScriptableObject
    {
        public List<WheelItem> _items;
        public bool _canRunOut;
        public bool RunOut => _items.Count == 0;
        private bool _collecrte;
        private const int _maxItems = 10;

        private void OnValidate()
        {
            if(_items.Count > _maxItems)
            {
                Debug.Log($"Pack contains more then {_maxItems} items. Recomended less then 10 packs");
            }
        }
    }
}
