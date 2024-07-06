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
        [SerializeField] private SpinController _spinController;
        [SerializeField] private BaseSpinCostProvider _spinCostProvider;
        [SerializeField] private Button _spinButton;
        
        public event Action SpinStarted;
        public event Action<WheelItem> SpinEnded;

        public bool IsSpinning { get; private set; }

        private IReadOnlyList<WheelItem> Items => _itemsPack.Items;

        private void Awake()
        {
            _drawer.Initialize(Items);
            _spinController.Initialize(Items);
            
            _drawer.DrawWheelItems();
            
            _spinController.OnSpinStart += SpinStartEventAction;
            _spinController.OnSpinEnd += SpinEndEventAction;
            
            _spinCostProvider.SpinAvailableUpdated += OnSpinAvailableUpdated;

            SpinStarted += _spinCostProvider.OnSpinStart;
        }
        
        public void Spin()
        {
            if(IsSpinning) 
                return;

            _spinController.Spin(_itemsPack.GetRandomItemIndex(), _itemsPack.NonZeroChanceItemIndexes);
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
            _spinController.OnSpinStart -= SpinStartEventAction;
            _spinController.OnSpinEnd -= SpinEndEventAction;

            _spinCostProvider.SpinAvailableUpdated -= OnSpinAvailableUpdated;
            
            SpinStarted -= _spinCostProvider.OnSpinStart;
        }
    }
}
