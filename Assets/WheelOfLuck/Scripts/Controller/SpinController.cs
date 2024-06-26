using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WheelOfLuck
{
    public class SpinController : MonoBehaviour
    {
        [Header("Wheel settings :")]
        [Range(1, 20)] 
        [SerializeField] private float _spinDuration = 8;
        [Range(1, 3)]
        [SerializeField] private int _spinBeyond360Count = 1;
        [SerializeField] private Ease _spinEasing = Ease.InOutQuart;

        [Space]
        [SerializeField] private Transform _wheelCircle;

        public event Action OnSpinStart;
        public event Action<WheelItemSO> OnSpinEnd;

        private IReadOnlyList<WheelItemSO> _items;

        private float _itemAngle;
        private float _halfItemAngle;
        private float _halfItemAngleWithPaddings;

        private List<int> _grantedItemsList = new List<int>();

        public void Initialize(IReadOnlyList<WheelItemSO> items)
        {
            _items = items;

            _itemAngle = 360f / _items.Count;
            _halfItemAngle = _itemAngle / 2f;
            _halfItemAngleWithPaddings = _halfItemAngle - (_halfItemAngle / 4f);
        }

        public void Spin(int randomItemIndex, IReadOnlyList<int> selectedItemsIndexes)
        {
            OnSpinStart?.Invoke();

            int index = randomItemIndex;
            WheelItemSO item = _items[index];

            if (item.Chance == 0 && selectedItemsIndexes.Count != 0)
            {
                index = selectedItemsIndexes[Random.Range(0, selectedItemsIndexes.Count)];
                item = _items[index];
            }

            float angle = -(_itemAngle * index);

            float rightOffset = (angle - _halfItemAngleWithPaddings) % 360;
            float leftOffset = (angle + _halfItemAngleWithPaddings) % 360;

            float randomAngle = Random.Range(leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.back * (randomAngle + _spinBeyond360Count * 360);
            Debug.Log(targetRotation);
            // fix offset rotation
            targetRotation += _wheelCircle.transform.rotation.eulerAngles;
            Debug.Log(targetRotation);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = _wheelCircle.eulerAngles.z;

            bool isIndicatorOnTheLine = false;

            Sequence spinSequence = DOTween.Sequence();

            spinSequence
            .Append(
                _wheelCircle
                .DORotate(targetRotation, _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= _halfItemAngle)
                    {
                        if (isIndicatorOnTheLine)
                        {
                            // Here you can play sound
                        }
                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }
                    currentAngle = _wheelCircle.eulerAngles.z;
                }))
            .AppendCallback(() =>
            {
                OnSpinEnd?.Invoke(item);
            });
        }        
    }
}
