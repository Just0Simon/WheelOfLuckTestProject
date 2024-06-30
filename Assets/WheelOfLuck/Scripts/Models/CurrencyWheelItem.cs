using UnityEngine;

namespace WheelOfLuck
{
    [CreateAssetMenu(order = 1, fileName = "CurrencyItem", menuName = "WheelOfLuck/Items/Currency")]
    public class CurrencyWheelItem : WheelItem
    {
        public override string Label => _amount.ToString();
        [SerializeField] private int _amount = 100;
        
        public override void Collect()
        {
            // Custom currency save method
        }
    }
}
