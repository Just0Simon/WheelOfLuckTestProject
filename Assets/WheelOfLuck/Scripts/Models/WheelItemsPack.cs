using System.Collections.Generic;
using UnityEngine;

namespace WheelOfLuck
{
    [CreateAssetMenu(fileName = "WheelItemsPack", order = 1, menuName = "WheelOfLuck/ItemsPack")]
    public class WheelItemsPack : ScriptableObject
    {
        public List<WheelItemSO> Items => _selectedItems;
        public IReadOnlyList<int> NonZeroChanceItemIndexes => _nonZeroChanceItemIndexes;
        public double TotalWeight => _accumulatedWeight;

        [Range(0, 5)]
        [SerializeField] private int _firstItemGrantedCount = 2;
        [SerializeField] private int _maxItemsOnWheel = 5;
        private int _grantedCollectedItemsCount = 0;

        [Header("What it will consist of")]
        [SerializeField] private int _consumebleItemsCount = 3;
        [SerializeField] private int _uniqueItemsCount = 2;
        
        [Header("Pool of wheel's items")]
        [SerializeField] private List<WheelItemSO> _itemsPool;
        
        [SerializeField] private double _accumulatedWeight;
        [SerializeField] private List<int> _nonZeroChanceItemIndexes;
        [SerializeField] private List<WheelItemSO> _selectedItems;

        public WheelItemSO ReplaceItem(WheelItemSO collectedItem)
        {
            if(_itemsPool.Count == _selectedItems.Count)
                return collectedItem;
            
            _selectedItems.Remove(collectedItem);
            
            WheelItemSO item = null;
            do
            {
                item = _itemsPool[Random.Range(0, _itemsPool.Count)];
            } while (_selectedItems.Contains(item) || collectedItem == item);

            return item;
        }

        public virtual int GetRandomItemIndex()
        {
            if (_firstItemGrantedCount > _grantedCollectedItemsCount)
            {
                return _grantedCollectedItemsCount;
            }
            
            double r = Random.Range(0, 2) * _accumulatedWeight;
            Debug.Log(Random.Range(0, 2));
            for (int i = 0; i < Items.Count; i++)
                if (Items[i]._weight >= r)
                    return i;

            return 0;
        }

        public void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < _selectedItems.Count; i++)
            {
                WheelItemSO item = _selectedItems[i];

                _accumulatedWeight += item.Chance;
                item._weight = _accumulatedWeight;

                item.Index = i;

                if (item.Chance > 0)
                    _nonZeroChanceItemIndexes.Add(i);
            }
        }

        private void OnValidate()
        {
            ValidateItemsPool();
        }

        private void ValidateItemsPool()
        {
            _accumulatedWeight = 0;
            
            if (_itemsPool.Count == 0)
                return;
            
            _nonZeroChanceItemIndexes = new List<int>();

            int selectedItemsCount = _itemsPool.Count < _maxItemsOnWheel ? _itemsPool.Count : _maxItemsOnWheel;
            _selectedItems = new List<WheelItemSO>(_itemsPool.GetRange(0, selectedItemsCount));

            CalculateWeightsAndIndices();
        }
    }
}
