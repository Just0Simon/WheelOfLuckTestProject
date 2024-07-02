using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WheelOfLuck.Demo
{
    public class WheelOfLuckEntrypoint : MonoBehaviour
    {
        [SerializeField] private RewardWidget _rewardWidget;
        [SerializeField] private WheelOfLuck _wheelOfLuck;
        [SerializeField] private CurrencySpinCostProvider _spinCostProvider;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private Button _addCurrencyButton;
        
        private void Awake()
        {
            _spinButton.onClick.AddListener(OnSpinButtonPressed);
            _addCurrencyButton.onClick.AddListener(() => _spinCostProvider.AddCurrency(50));
        }

        private void OnEnable()
        {
            _wheelOfLuck.SpinEnded += SpinEnded;
        }

        private void OnSpinButtonPressed()
        {
            _spinButton.interactable = false;
            _wheelOfLuck.Spin();
        }
        
        private void SpinEnded(WheelItem item)
        {
            _spinButton.interactable = true;
            _rewardWidget.ShowItem(item);
        }

        private void OnDisable()
        {
            _wheelOfLuck.SpinEnded -= SpinEnded;
        }
    }
}