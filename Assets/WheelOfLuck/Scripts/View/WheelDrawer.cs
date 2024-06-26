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
        [SerializeField] private GameObject _wheelPiecePrefab;
        [SerializeField] private Transform _wheelPiecesParent;
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private Transform _linesParent;


        private int _minItems;
        private int _maxItems;
        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);

        private float _pieceAngle;
        private float _halfPieceAngle;
        private float _halfPieceAngleWithPaddings;
        private IReadOnlyList<WheelItemSO> _items;

        public void Initialize(IReadOnlyList<WheelItemSO> items, int minItems, int maxItems)
        {
            _items = items;
            _minItems = minItems;
            _maxItems = maxItems;

            _pieceAngle = 360 / _items.Count;

            _halfPieceAngle = _pieceAngle / 2f;
            _halfPieceAngleWithPaddings = _halfPieceAngle - (_halfPieceAngle / 4f);
        }

        public void Generate()
        {
            _wheelPiecePrefab = InstantiatePiece();

            RectTransform rt = _wheelPiecePrefab.transform.GetChild(0).GetComponent<RectTransform>();
            float pieceWidth = Mathf.Lerp(pieceMinSize.x, pieceMaxSize.x, 1f - Mathf.InverseLerp(_minItems, _maxItems, _items.Count));
            float pieceHeight = Mathf.Lerp(pieceMinSize.y, pieceMaxSize.y, 1f - Mathf.InverseLerp(_minItems, _maxItems, _items.Count));

            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pieceWidth);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pieceHeight);

            for (int i = 0; i < _items.Count; i++)
                DrawPiece(i);
        }

        private void DrawPiece(int index)
        {
            WheelItemSO piece = _items[index];
            Transform pieceTrns = InstantiatePiece().transform.GetChild(0);

            pieceTrns.GetChild(0).GetComponent<Image>().sprite = piece.Icon;
            pieceTrns.GetChild(1).GetComponent<TMP_Text>().text = piece.Label;
            pieceTrns.GetChild(2).GetComponent<TMP_Text>().text = piece.Amount.ToString();

            DrawLine(index);

            pieceTrns.RotateAround(_wheelPiecesParent.position, Vector3.back, _pieceAngle * index);
        }

        private void DrawLine(int itemIndex)
        {
            Transform lineTrns = Instantiate(_linePrefab, _linesParent.position, Quaternion.identity, _linesParent).transform;
            lineTrns.RotateAround(_wheelPiecesParent.position, Vector3.back, (_pieceAngle * itemIndex) + _halfPieceAngle);
        }

        private GameObject InstantiatePiece()
        {
            return Instantiate(_wheelPiecePrefab, _wheelPiecesParent.position, Quaternion.identity, _wheelPiecesParent);
        }

        private void OnValidate()
        {
            if (_wheelTransform != null)
                _wheelTransform.localScale = new Vector3(_wheelSize, _wheelSize, 1f);
        }
    }
}
