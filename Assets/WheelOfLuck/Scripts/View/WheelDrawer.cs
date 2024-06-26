using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfLuck
{
    public class WheelDrawer : MonoBehaviour
    {
        [Range(0.2f, 2f)]
        [SerializeField] private float _wheelSize = 1f;
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private GameObject _wheelItemPrefab;
        [SerializeField] private Transform _wheelItemsParent;
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private Transform _linesParent;

        private int _minItems;
        private int _maxItems;
        private Vector2 _itemMinSize = new Vector2(81f, 146f);
        private Vector2 _itemMaxSize = new Vector2(144f, 213f);

        private float _itemAngle;
        private float _halfItemAngle;
        private float _halfItemAngleWithPaddings;
        private IReadOnlyList<WheelItemSO> _items;

        public void Initialize(IReadOnlyList<WheelItemSO> items, int minItems, int maxItems)
        {
            _items = items;
            _minItems = minItems;
            _maxItems = maxItems;

            _itemAngle = 360 / _items.Count;

            _halfItemAngle = _itemAngle / 2f;
            _halfItemAngleWithPaddings = _halfItemAngle - (_halfItemAngle / 4f);
        }

        private void ClearItemsAndLinesContainers()
        {
            for(int i = 0; i < _linesParent.childCount; i++)
            {
                Destroy(_linesParent.GetChild(i).gameObject);
            }

            for(int i = 0; i < _wheelItemsParent.childCount; i++)
            {
                Destroy(_wheelItemsParent.GetChild(i).gameObject);
            }
        }

        public void Generate()
        {
            ClearItemsAndLinesContainers();

            RectTransform rt = _wheelItemPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float itemWidth = Mathf.Lerp(_itemMinSize.x, _itemMaxSize.x, 1f - Mathf.InverseLerp(_minItems, _maxItems, _items.Count));
            float itemHeight = Mathf.Lerp(_itemMinSize.y, _itemMaxSize.y, 1f - Mathf.InverseLerp(_minItems, _maxItems, _items.Count));

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemHeight);

            for (int i = 0; i < _items.Count; i++)
                DrawPiece(i);
        }

        private void DrawPiece(int index)
        {
            WheelItemSO item = _items[index];
            Transform itemTrns = InstantiateItem().transform.GetChild(0);

            itemTrns.GetChild(0).GetComponent<Image>().sprite = item.Icon;
            itemTrns.GetChild(1).GetComponent<TMP_Text>().text = item.Label;
            itemTrns.GetChild(2).GetComponent<TMP_Text>().text = item.Amount.ToString();

            DrawLine(index);

            itemTrns.RotateAround(_wheelItemsParent.position, Vector3.back, _itemAngle * index);
        }

        private void DrawLine(int itemIndex)
        {
            Transform lineTrns = Instantiate(_linePrefab, _linesParent.position, Quaternion.identity, _linesParent).transform;
            lineTrns.RotateAround(_wheelItemsParent.position, Vector3.back, (_itemAngle * itemIndex) + _halfItemAngle);
        }

        private GameObject InstantiateItem()
        {
            return Instantiate(_wheelItemPrefab, _wheelItemsParent.position, Quaternion.identity, _wheelItemsParent);
        }

        private void OnValidate()
        {
            if (_wheelTransform != null)
                _wheelTransform.localScale = new Vector3(_wheelSize, _wheelSize, 1f);
        }
    }
}
