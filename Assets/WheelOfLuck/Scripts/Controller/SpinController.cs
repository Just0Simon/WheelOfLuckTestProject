using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WheelOfLuck
{
    public class SpinController : MonoBehaviour
    {
        [Space]
        [SerializeField] private Transform _wheelCircle;

        [Space]
        [Header("Wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;

        public event Action OnSpinStart;
        public event Action<WheelItemSO> OnSpinEnd;

        private IReadOnlyList<WheelItemSO> _items;

        private float _itemAngle;
        private float _halfItemAngle;
        private float _halfItemAngleWithPaddings;

        private double _accumulatedWeight;

        private List<int> _grantedItemsList = new List<int>();

        public void Initialize(IReadOnlyList<WheelItemSO> items)
        {
            _items = items;

            _itemAngle = 360f / _items.Count;
            _halfItemAngle = _itemAngle / 2f;
            _halfItemAngleWithPaddings = _halfItemAngle - (_halfItemAngle / 4f);
        }

        public void Spin(double accumulatedWeight, int randomItemIndex, IReadOnlyList<int> nonZeroChanceItemIndexes)
        {
            _accumulatedWeight = accumulatedWeight;

            OnSpinStart?.Invoke();

            int index = randomItemIndex;
            WheelItemSO item = _items[index];

            if (item.Chance == 0 && nonZeroChanceItemIndexes.Count != 0)
            {
                index = nonZeroChanceItemIndexes[Random.Range(0, nonZeroChanceItemIndexes.Count)];
                item = _items[index];
            }

            float angle = -(_itemAngle * index);

            float rightOffset = (angle - _halfItemAngleWithPaddings) % 360;
            float leftOffset = (angle + _halfItemAngleWithPaddings) % 360;

            float randomAngle = Random.Range(leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.back * (randomAngle + 2 * 360 * spinDuration);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = _wheelCircle.eulerAngles.z;

            bool isIndicatorOnTheLine = false;

            _wheelCircle
            .DORotate(targetRotation, spinDuration, RotateMode.Fast)
            .SetEase(Ease.InOutQuart)
            .OnUpdate(() => {
                float diff = Mathf.Abs(prevAngle - currentAngle);
                if (diff >= _halfItemAngle)
                {
                    if (isIndicatorOnTheLine)
                    {

                    }
                    prevAngle = currentAngle;
                    isIndicatorOnTheLine = !isIndicatorOnTheLine;
                }
                currentAngle = _wheelCircle.eulerAngles.z;
            })
            .OnComplete(() => {
                OnSpinEnd?.Invoke(item);
            });
        }

        
    }
}
