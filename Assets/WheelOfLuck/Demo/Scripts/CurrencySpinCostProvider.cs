using UnityEngine;

namespace WheelOfLuck
{
    public class CurrencySpinCostProvider : BaseSpinCostProvider
    {
        [SerializeField] private string _firstFreeSpinKey = "firstFreeSpin"; 
        [SerializeField] private int _coins = 0;
        [SerializeField] private int _spinCost = 100;

        private bool _firstSpinFree;

        private void Awake()
        {
            if (PlayerPrefs.HasKey(_firstFreeSpinKey))
                _firstSpinFree = PlayerPrefs.GetInt(_firstFreeSpinKey) == 1;
            else
            {
                _firstSpinFree = true;
            }
        }

        private void Start()
        {
            UpdateAvailable();
        }

        public override void OnSpinStart()
        {
            if (_firstSpinFree)
            {
                _firstSpinFree = false;
                PlayerPrefs.SetInt(_firstFreeSpinKey, 0);
            }
            else
            {
                _coins -= _spinCost;
            }
            
            UpdateAvailable();
        }
        
        protected override void UpdateAvailable()
        {
            if (_firstSpinFree)
            {
                Available = _firstSpinFree;
            }
            else
            {
                Available = _coins >= _spinCost;
            }
            
            OnSpinAvailableUpdated(Available);
        }

        private void OnValidate()
        {
            UpdateAvailable();
        }
        
#if UNITY_EDITOR

        public void AddCurrency(int amount)
        {
            _coins += amount;
            UpdateAvailable();
        }
#endif
    }
}
