using System;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfLuck.Demo
{
    public class WheelOfLuckEntrypoint : MonoBehaviour
    {
        [SerializeField] private RewardWidget _rewardWidget;
        [SerializeField] private WheelOfLuck _wheelOfLuck;
        [SerializeField] private Button _spinButton;

        private void Awake()
        {
            _spinButton.onClick.AddListener(OnSpinButtonPressed);
        }

        private void OnEnable()
        {
            _wheelOfLuck.OnSpinEndEvent += OnSpinEnd;
        }

        private void OnSpinButtonPressed()
        {
            _spinButton.interactable = false;
            _wheelOfLuck.Spin();
        }
        
        private void OnSpinEnd(WheelItem item)
        {
            _spinButton.interactable = true;
            _rewardWidget.ShowItem(item);
        }

        private void OnDisable()
        {
            _wheelOfLuck.OnSpinEndEvent -= OnSpinEnd;
        }
    }
}