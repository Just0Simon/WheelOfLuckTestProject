using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace WheelOfLuck
{
    [CreateAssetMenu(fileName = "WheelItemsPack", order = 1, menuName = "WheelOfLuck/ItemsPack")]
    public class WheelItemsPack : ScriptableObject
    {
        public List<WheelItemSO> Items => _runtimeItems;;
        [SerializeField] private List<WheelItemSO> _itemsPool;

        private double _accumulatedWeight;
        private List<WheelItemSO> _runtimeItems;
        private List<WheelItemSO> _selectedItems;

        public WheelItemSO ReplaceItem(WheelItemSO collectedItem)
        {
            return Items[0];
        }

        public virtual int GetRandomItemIndex()
        {
            double r = Random.Range(0, 2) * _accumulatedWeight;
            Debug.Log(Random.Range(0, 2));
            for (int i = 0; i < _itemsPool.Count; i++)
                if (_itemsPool[i]._weight >= r)
                    return i;

            return 0;
        }

        public void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < _itemsPool.Count; i++)
            {
                WheelItemSO item = _itemsPool[i];

                //add weights:
                _accumulatedWeight += item.Chance;
                item._weight = _accumulatedWeight;

                //add index :
                item.Index = i;

                //save non zero chance indices:
                if (item.Chance > 0)
                    _grantedItemsList.Add(i);
            }
        }

        private void OnValidate()
        {
            _runtimeItems = new List<WheelItemSO>(_itemsPool);
        }
    }
}
