using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WheelOfLuck.ReadOnly;

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

        private readonly Vector2 _itemMinSize = new Vector2(81f, 146f);
        private readonly Vector2 _itemMaxSize = new Vector2(144f, 213f);

        private float _itemAngle;
        private float _halfItemAngle;
        private IReadOnlyList<WheelItem> _items;

        private List<WheelItemSlot> _wheelItemSlotList;

        private readonly int _minItemsCount = 2;
        private readonly int _maxItemsCount = 12;
        
        public void Initialize(IReadOnlyList<WheelItem> items)
        {
            _wheelItemSlotList = new List<WheelItemSlot>(items.Count);
            
            _items = items;
            _itemAngle = 360f / _items.Count;

            _halfItemAngle = _itemAngle / 2f;

            RectTransform rt = _wheelItemPrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float itemWidth = Mathf.Lerp(_itemMinSize.x, _itemMaxSize.x, 1f - Mathf.InverseLerp(_minItemsCount, _maxItemsCount, _items.Count));
            float itemHeight = Mathf.Lerp(_itemMinSize.y, _itemMaxSize.y, 1f - Mathf.InverseLerp(_minItemsCount, _maxItemsCount, _items.Count));

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemHeight);
            
            if (_wheelItemSlotList.Count < _items.Count)
            {
                InstantiateAndInitializeItemSlots();
            }
        }

        public void DrawWheelItems()
        {
            for (int i = 0; i < _items.Count; i++)
                DrawItem(i);
        }

        private void InstantiateAndInitializeItemSlots()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                WheelItem item = _items[i];
                Transform itemTransform = InstantiateItem().transform.GetChild(0);
                
                Image image = itemTransform.GetChild(0).GetComponent<Image>();
                TMP_Text labelText = itemTransform.GetChild(1).GetComponent<TMP_Text>();

                WheelItemSlot slot = new WheelItemSlot()
                {
                    ItemTransform = itemTransform,
                    ItemImage = image,
                    LabelText = labelText
                };
                
                slot.SetItem(item);
                
                slot.ItemTransform.RotateAround(_wheelItemsParent.position, Vector3.back, _itemAngle * i);
                
                _wheelItemSlotList.Add(slot);
                
                DrawLine(i);
            }
        }

        private void DrawItem(int index)
        {
            WheelItem item = _items[index];

            _wheelItemSlotList[index].SetItem(item);
        }

        private void DrawLine(int itemIndex)
        {
            Transform lineTransfrom = Instantiate(_linePrefab, _linesParent.position, Quaternion.identity, _linesParent).transform;
            lineTransfrom.RotateAround(_wheelItemsParent.position, Vector3.back, (_itemAngle * itemIndex) + _halfItemAngle);
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

        [Serializable]
        public class WheelItemSlot
        {
            public Transform ItemTransform;
            public Image ItemImage;
            public TMP_Text LabelText;

            public void SetItem(IReadOnlyWheelItem wheelItem)
            {
                ItemImage.sprite = wheelItem.Icon;
                LabelText.text = wheelItem.Label;
            }
        }
    }
}
