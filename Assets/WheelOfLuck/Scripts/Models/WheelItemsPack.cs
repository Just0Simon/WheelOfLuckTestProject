using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace WheelOfLuck
{
    [CreateAssetMenu(fileName = "WheelItemsPack", order = 1, menuName = "WheelOfLuck/ItemsPack")]
    public class WheelItemsPack : ScriptableObject
    {
        public List<WheelItem> Items => _selectedItems;
        public IReadOnlyList<int> NonZeroChanceItemIndexes => _selectedItemIndexes;
        public double TotalWeight => _accumulatedWeight;
        
        [SerializeField] private int _firstItemGrantedCount = 2;
        [SerializeField] private int _itemsOnWheelCount = 5;
        private int _grantedCollectedItemsCount = 0;
        
        [Header("What it will consist of")]
        [SerializeField] private int _consumableItemsCount = 3;
        [SerializeField] private int _uniqueItemsCount = 2;

        [Header("Pool of wheel's items")]
        [Tooltip("Will drop as ordered")]
        [SerializeField] private List<WheelItem> _grantedItems;
        [SerializeField] private List<WheelItem> _itemsPool;
        
        private double _accumulatedWeight;
        private List<int> _selectedItemIndexes;

        [Space]
        [Header("Automated")]
        [SerializeField] private List<WheelItem> _selectedItems;

        public void ReplaceItem(WheelItem collectedItem)
        {
            if(_itemsPool.IndexOf(collectedItem) < _firstItemGrantedCount)
            {
                _grantedCollectedItemsCount++;
            }

            if (_itemsPool.Count == _selectedItems.Count)
                return;

            int itemIndex = _selectedItems.IndexOf(collectedItem);

            WheelItem item = null;
            do
            {
                item = _itemsPool[Random.Range(_firstItemGrantedCount, _itemsPool.Count)];
            } while (_selectedItems.Contains(item) || collectedItem == item);
            
            _selectedItems[itemIndex] = item;

            CalculateWeightsAndIndices();
        }

        public virtual int GetRandomItemIndex()
        {
            if (_grantedCollectedItemsCount < _firstItemGrantedCount)
            {
                return _grantedCollectedItemsCount;
            }
            
            double r = Random.Range(0, 2) * _accumulatedWeight;

            for (int i = 0; i < Items.Count; i++)
                if (Items[i]._weight >= r)
                    return i;
            
            return -1;
        }

        public void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < _selectedItems.Count; i++)
            {
                WheelItem item = _selectedItems[i];

                _accumulatedWeight += item.Chance;
                item._weight = _accumulatedWeight;

                item.Index = i;

                _selectedItemIndexes.Add(i);
            }
        }
        
        
        private void OnValidate()
        {
            ValidateWheelItems();
            
            ValidateItemsPool();
            
            CalculateWeightsAndIndices();
        }

        private int previousParam1;
        private int previousParam2;
        private void ValidateWheelItems()
        {
            if (_consumableItemsCount < 0)
                _consumableItemsCount = previousParam1;
            
            if (_uniqueItemsCount < 0)
                _uniqueItemsCount = previousParam2;

            if (_consumableItemsCount != previousParam1 && _consumableItemsCount <= _itemsOnWheelCount)
            {
                _uniqueItemsCount = _itemsOnWheelCount - _consumableItemsCount;
            }
            else if (_uniqueItemsCount != previousParam2 && _uniqueItemsCount <= _itemsOnWheelCount)
            {
                _consumableItemsCount = _itemsOnWheelCount - _uniqueItemsCount;
            }
            else
            {
                _consumableItemsCount = previousParam1;
                _uniqueItemsCount = previousParam2;
            }
            
            if (_itemsOnWheelCount != _consumableItemsCount + _uniqueItemsCount)
            {
                if(_itemsOnWheelCount > _consumableItemsCount + _uniqueItemsCount)
                    _consumableItemsCount += _itemsOnWheelCount - _consumableItemsCount - _uniqueItemsCount;
                if (_itemsOnWheelCount < _consumableItemsCount + _uniqueItemsCount)
                {
                    if (_consumableItemsCount > _uniqueItemsCount)
                    {
                        _consumableItemsCount -= Mathf.Abs(_itemsOnWheelCount - _consumableItemsCount - _uniqueItemsCount);
                    }
                    else
                    {
                        _uniqueItemsCount -= Mathf.Abs(_itemsOnWheelCount - _consumableItemsCount - _uniqueItemsCount); 
                    }
                }
            }
            
            previousParam1 = _consumableItemsCount;
            previousParam2 = _uniqueItemsCount;
        }

        private void ValidateItemsPool()
        {
            _accumulatedWeight = 0;
            
            if (_itemsPool.Count == 0)
                return;
            
            _selectedItemIndexes = new List<int>();

            int consumableAdded = 0;
            int uniqueAdded = 0;

            _selectedItems.Clear();

            int addedGrantedItems = _grantedCollectedItemsCount;
            
            while (_firstItemGrantedCount != 0 && addedGrantedItems < _firstItemGrantedCount)
            {
                _selectedItems.Add(_grantedItems[addedGrantedItems]);
                addedGrantedItems++;

                if (_selectedItems.Count == _itemsOnWheelCount)
                {
                    return;
                }
            }

            while (_selectedItems.Count != _itemsOnWheelCount)
            {
                foreach (var item in _itemsPool)
                {
                    if (item is CurrencyWheelItem && consumableAdded < _consumableItemsCount)
                    {
                        _selectedItems.Add(item);
                        consumableAdded++;
                    }
                    else if (item is UniqueWheelItem && uniqueAdded < _uniqueItemsCount)
                    {
                        _selectedItems.Add(item);
                        uniqueAdded++;
                    }

                    if (_selectedItems.Count == _itemsOnWheelCount)
                    {
                        break;
                    }
                }
            }
            
        }

        [ContextMenu("Custom Reset")]
        public void Reset()
        {
            _accumulatedWeight = 0;
            _grantedCollectedItemsCount = 0;
        }
    }
}
