using UnityEngine;

namespace WheelOfLuck
{
    public class SkinWheelItem : WheelItemSO
    {
        [SerializeField] private string _uniqueItemName;

        public override void Collect()
        {
            // Custom skip apply method
        }
    }
}
