using System.Collections.Generic;
using UnityEngine;

namespace WheelOfLuck
{
    [CreateAssetMenu(fileName = "WheelItemsPack", order = 1, menuName = "WheelOfLuck/ItemsPack")]
    public class WheelItemsPack : ScriptableObject
    {
        public List<WheelItemSO> Items => _selectedItems;
        public IReadOnlyList<int> NonZeroChanceItemIndexes => _selectedItemIndexes;
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
        
        private double _accumulatedWeight;
        private List<int> _selectedItemIndexes;

        [Space]
        [Header("Automated")]
        [SerializeField] private List<WheelItemSO> _selectedItems;

        public void ReplaceItem(WheelItemSO collectedItem)
        {
            if(_itemsPool.IndexOf(collectedItem) < _firstItemGrantedCount - 1)
            {
                _grantedCollectedItemsCount++;
            }

           if (_itemsPool.Count == _selectedItems.Count)
                return;
            
            _selectedItems.Remove(collectedItem);
            
            WheelItemSO item = null;
            do
            {
                item = _itemsPool[Random.Range(0, _itemsPool.Count)];
            } while (_selectedItems.Contains(item) || collectedItem == item);
            
            _selectedItems.Add(item);

            CalculateWeightsAndIndices();
        }

        public virtual int GetRandomItemIndex()
        {
            if (_firstItemGrantedCount > _grantedCollectedItemsCount)
            {
                return _grantedCollectedItemsCount;
            }
            
            double r = Random.Range(0, 2) * _accumulatedWeight;

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

                _selectedItemIndexes.Add(i);
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
            
            _selectedItemIndexes = new List<int>();

            int selectedItemsCount = _itemsPool.Count < _maxItemsOnWheel ? _itemsPool.Count : _maxItemsOnWheel;
            _selectedItems = new List<WheelItemSO>(_itemsPool.GetRange(_grantedCollectedItemsCount, selectedItemsCount));

            CalculateWeightsAndIndices();
        }

        [ContextMenu("Custom Reset")]
        public void Reset()
        {
            _accumulatedWeight = 0;
            _grantedCollectedItemsCount = 0;
        }
    }
}
