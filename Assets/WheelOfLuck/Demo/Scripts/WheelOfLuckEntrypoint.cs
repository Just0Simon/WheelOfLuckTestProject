using UnityEngine;

namespace WheelOfLuck.Demo
{
    public class WheelOfLuckEntrypoint : MonoBehaviour
    {
        [SerializeField] private RewardWidget _rewardWidget;
        [SerializeField] private WheelOfLuck _wheelOfLuck;

        private void OnEnable()
        {
            _wheelOfLuck.OnSpinEndEvent += OnSpinEnd;
        }

        private void OnSpinEnd(WheelItem item)
        {
            _rewardWidget.ShowItem(item);
        }

        private void OnDisable()
        {
            _wheelOfLuck.OnSpinEndEvent -= OnSpinEnd;
        }
    }
}