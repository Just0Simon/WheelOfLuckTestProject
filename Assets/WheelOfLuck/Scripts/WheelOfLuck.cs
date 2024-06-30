using System;
using System.Collections.Generic;
using UnityEngine;

namespace WheelOfLuck
{
    public class WheelOfLuck : MonoBehaviour
    {
        [SerializeField] private WheelItemsPack _itemsPack;
        [Space]
        [SerializeField] private WheelDrawer _drawer;
        [SerializeField] private SpinController _spinController;

        // Events
        public event Action OnSpinStartEvent;
        public event Action<WheelItem> OnSpinEndEvent;

        public bool IsSpinning { get; private set; }

        private IReadOnlyList<WheelItem> Items => _itemsPack.Items;
        private int _minItemsCount = 2;
        private int _maxItemsCount = 12;

        private void Awake()
        {
            _drawer.Initialize(Items, _minItemsCount, _maxItemsCount);
            _spinController.Initialize(Items);
            
            _spinController.OnSpinStart += OnSpinStart;
            _spinController.OnSpinEnd += OnSpinEnd;
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

            _spinController.Spin(_itemsPack.GetRandomItemIndex(), _itemsPack.NonZeroChanceItemIndexes);
        }

        private void OnSpinStart()
        {
            IsSpinning = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spin();
            }
        }

        private void OnSpinEnd(WheelItem item)
        {
            IsSpinning = false;
            OnSpinEndEvent?.Invoke(item);

            _itemsPack.ReplaceItem(item);

            _drawer.DrawWheelItems();
        }

        private void OnDestroy()
        {
            OnSpinStartEvent = null;
            OnSpinEndEvent = null;
        }
    }
}
