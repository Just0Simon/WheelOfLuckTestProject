using UnityEngine;
using WheelOfLuck.ReadOnly;

namespace WheelOfLuck 
{
    public abstract class WheelItem : ScriptableObject, ICollectable, IReadOnlyWheelItem
    {
        public Sprite Icon => _icon; 
        public abstract string Label { get; }
        
        [SerializeField] private Sprite _icon;

        [Tooltip("Probability in %")]
        [Range(0f, 100f)]
        public float Chance = 100f;

        [HideInInspector] public int Index;
        [HideInInspector] public double _weight = 0f;

        public abstract void Collect();
    }
}
