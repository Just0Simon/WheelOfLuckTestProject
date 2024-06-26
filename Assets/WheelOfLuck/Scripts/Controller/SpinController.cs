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
        public event Action OnSpinEnd;

        private IReadOnlyList<WheelItem> _items;

        private float _pieceAngle;
        private float _halfPieceAngle;
        private float _halfPieceAngleWithPaddings;

        private double _accumulatedWeight;

        private List<int> _grantedItemsList = new List<int>();

        public void Initialize(IReadOnlyList<WheelItem> items)
        {
            _items = items;

            _pieceAngle = 360 / _items.Count;
            _halfPieceAngle = _pieceAngle / 2f;
            _halfPieceAngleWithPaddings = _halfPieceAngle - (_halfPieceAngle / 4f);
        }

        public void Spin(double accumulatedWeight, List<int> chellengingItemIndexes)
        {
            _accumulatedWeight = accumulatedWeight;

            OnSpinStart?.Invoke();

            int index = GetRandomPieceIndex();
            WheelItem piece = _items[index];

            if (piece.Chance == 0 && chellengingItemIndexes.Count != 0)
            {
                index = chellengingItemIndexes[Random.Range(0, chellengingItemIndexes.Count)];
                piece = _items[index];
            }

            float angle = -(_pieceAngle * index);

            float rightOffset = (angle - _halfPieceAngleWithPaddings) % 360;
            float leftOffset = (angle + _halfPieceAngleWithPaddings) % 360;

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
                if (diff >= _halfPieceAngle)
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
                OnSpinEnd?.Invoke();
            });
        }

        public void CalculateWeightsAndIndices()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                WheelItem piece = _items[i];

                //add weights:
                _accumulatedWeight += piece.Chance;
                piece._weight = _accumulatedWeight;

                //add index :
                piece.Index = i;

                //save non zero chance indices:
                if (piece.Chance > 0)
                    _grantedItemsList.Add(i);
            }
        }

        private int GetRandomPieceIndex()
        {
            double r = Random.Range(0, 2) * _accumulatedWeight;
            Debug.Log(Random.Range(0, 2));
            for (int i = 0; i < _items.Count; i++)
                if (_items[i]._weight >= r)
                    return i;

            return 0;
        }
    }
}
