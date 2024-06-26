using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WheelOfLuck
{
    public class WheelOfLuck : MonoBehaviour
    {
        [Header("Wheel items:")]
        [SerializeField] private List<WheelItem> _items;

        [Space]
        [SerializeField] private WheelDrawer _drawer;
        [SerializeField] private SpinController _spinController;

        // Events
        public event Action OnSpinStartEvent;
        public event Action<WheelItem> OnSpinEndEvent;

        public bool IsSpinning { get; private set; }

        private int _minItemsCount = 2;
        private int _maxItemsCount = 12;

        private void Awake()
        {
            _drawer.Initialize(_items, _minItemsCount, _maxItemsCount);
            _spinController.OnSpinStart += OnSpinStart;
        }


        private void Start()
        {
            _drawer.Generate();

            _spinController.CalculateWeightsAndIndices();
            
        }

        public void Spin()
        {
            if(IsSpinning) 
                return;

            _spinController.Spin(_accumulatedWeight, nonZeroChancesIndices);
        }

        private void OnSpinStart()
        {
            IsSpinning = true;
        }

        private void OnSpinEnd(WheelItem item)
        {
            IsSpinning = false;
            OnSpinEndEvent?.Invoke(item);
        }

        public void AddOnSpinStartAction(Action action)
        {
            OnSpinStartEvent += action;
        }

        public void AddOnSpinEndAction(Action<WheelItem> action)
        {
            OnSpinEndEvent += action;
        }

        private void OnDestroy()
        {
            OnSpinStartEvent = null;
            OnSpinEndEvent = null;
        }
    }
}
