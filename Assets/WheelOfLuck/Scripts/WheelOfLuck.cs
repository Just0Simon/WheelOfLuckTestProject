using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace WheelOfLuck
{
    public class WheelOfLuck : MonoBehaviour
    {
        [SerializeField] private WheelItemsPack _itemsPack;
        [Space]
        [SerializeField] private WheelDrawer _drawer;
        [SerializeField] private SpinProvider _spinProvider;
        [SerializeField] private BaseSpinCostProvider _spinCostProvider;
        
        public event Action OnSpinStartEvent;
        public event Action<WheelItem> OnSpinEndEvent;

        public bool IsSpinning { get; private set; }

        private IReadOnlyList<WheelItem> Items => _itemsPack.Items;
        private int _minItemsCount = 2;
        private int _maxItemsCount = 12;

        private void Awake()
        {
            _drawer.Initialize(Items, _minItemsCount, _maxItemsCount);
            _spinProvider.Initialize(Items);
        }

        private void OnSpinAvailableUpdated(bool available)
        {
            
        }

        private void Start()
        {
            _drawer.DrawWheelItems();
        }

        [ContextMenu("Spin")]
        public void Spin()
        {
            if(IsSpinning) 
                return;

            _spinProvider.Spin(_itemsPack.GetRandomItemIndex(), _itemsPack.NonZeroChanceItemIndexes);
        }

        private void OnSpinStart()
        {
            IsSpinning = true;
        }

        private void OnSpinEnd(WheelItem item)
        {
            IsSpinning = false;
            OnSpinEndEvent?.Invoke(item);

            _itemsPack.ReplaceItem(item);

            _drawer.DrawWheelItems();
        }

        private void OnEnable()
        {
            _spinProvider.OnSpinStart += OnSpinStart;
            _spinProvider.OnSpinEnd += OnSpinEnd;
            
            _spinCostProvider.SpinAvailableUpdated += OnSpinAvailableUpdated;
        }

        private void OnDisable()
        {
            _spinProvider.OnSpinStart -= OnSpinStart;
            _spinProvider.OnSpinEnd -= OnSpinEnd;

            _spinCostProvider.SpinAvailableUpdated -= OnSpinAvailableUpdated;
        }
    }
}
