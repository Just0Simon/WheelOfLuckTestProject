using UnityEngine;

namespace WheelOfLuck.ReadOnly
{
    public interface IReadOnlyWheelItem
    {
        public Sprite Icon { get; }
        public string Label { get; }
    }
}