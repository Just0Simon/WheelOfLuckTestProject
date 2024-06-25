using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WheelOfLuck
{
    public class WheelOfLuckWidget : MonoBehaviour
    {
        [SerializeField] private GameObject _linePrefab;
        [SerializeField] private Transform _linesParent;

        [Space]
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Transform _wheelCircle;
        [SerializeField] private GameObject _wheelPiecePrefab;
        [SerializeField] private Transform _wheelPiecesParent;

        [Space]
        [Header("Picker wheel settings :")]
        [Range(1, 20)] public int spinDuration = 8;
        [SerializeField][Range(.2f, 2f)] private float wheelSize = 1f;

        [Space]
        [Header("Picker wheel pieces :")]
        public WheelPiece[] WheelPieces;

        // Events
        private UnityAction onSpinStartEvent;
        private UnityAction<WheelPiece> onSpinEndEvent;


        private bool _isSpinning = false;

        public bool IsSpinning => _isSpinning;

        private Vector2 pieceMinSize = new Vector2(81f, 146f);
        private Vector2 pieceMaxSize = new Vector2(144f, 213f);
        private int piecesMin = 2;
        private int piecesMax = 12;

        private float pieceAngle;
        private float halfPieceAngle;
        private float halfPieceAngleWithPaddings;


        private double accumulatedWeight;

        private List<int> nonZeroChancesIndices = new List<int>();

        private void OnValidate()
        {
            if (_wheelTransform != null)
                _wheelTransform.localScale = new Vector3(wheelSize, wheelSize, 1f);
        }
    }
}
