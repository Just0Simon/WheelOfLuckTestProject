using UnityEngine;

namespace WheelOfLuck
{
    [CreateAssetMenu(order = 1, fileName = "UniqueItem", menuName = "WheelOfLuck/Items/Unique")]
    public class UniqueWheelItem : WheelItem
    { 
        public override string Label => _label;
        [SerializeField] private string _label;
        [SerializeField] private string _uniqueItemName;
        
        public override void Collect()
        {
            // Custom unique item apply method
        }
    }
}
