using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfLuck
{
    public class WheelOfLuck : MonoBehaviour
    {
        [SerializeField] private WheelItemsPack _itemsPack;
        [Space]
        [SerializeField] private WheelDrawer _drawer;
        [SerializeField] private SpinProvider _spinProvider;
        [SerializeField] private BaseSpinCostProvider _spinCostProvider;
        [SerializeField] private Button _spinButton;
        
        public event Action SpinStarted;
        public event Action<WheelItem> SpinEnded;

        public bool IsSpinning { get; private set; }

        private IReadOnlyList<WheelItem> Items => _itemsPack.Items;
        private int _minItemsCount = 2;
        private int _maxItemsCount = 12;

        private void Awake()
        {
            _drawer.Initialize(Items, _minItemsCount, _maxItemsCount);
            _spinProvider.Initialize(Items);
            
            _drawer.DrawWheelItems();
            
            _spinProvider.OnSpinStart += SpinStartEventAction;
            _spinProvider.OnSpinEnd += SpinEndEventAction;
            
            _spinCostProvider.SpinAvailableUpdated += OnSpinAvailableUpdated;

            SpinStarted += _spinCostProvider.OnSpinStart;
        }
        
        public void Spin()
        {
            if(IsSpinning) 
                return;

            _spinProvider.Spin(_itemsPack.GetRandomItemIndex(), _itemsPack.NonZeroChanceItemIndexes);
        }

        private void SpinStartEventAction()
        {
            IsSpinning = true;
            SpinStarted?.Invoke();
        }

        private void SpinEndEventAction(WheelItem item)
        {
            IsSpinning = false;
            SpinEnded?.Invoke(item);

            _itemsPack.ReplaceItem(item);

            _drawer.DrawWheelItems();
        }

        private void OnSpinAvailableUpdated(bool available)
        {
            _spinButton.interactable = available;
        }

        private void OnDestroy()
        {
            _spinProvider.OnSpinStart -= SpinStartEventAction;
            _spinProvider.OnSpinEnd -= SpinEndEventAction;

            _spinCostProvider.SpinAvailableUpdated -= OnSpinAvailableUpdated;
            
            SpinStarted -= _spinCostProvider.OnSpinStart;
        }
    }
}
