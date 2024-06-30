using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfLuck.Demo
{
    public class RewardWidget : MonoBehaviour
    {
        [SerializeField] private RectTransform _rewardMenuTransform;
        [SerializeField] private Image _itemImage;
        [SerializeField] private TMP_Text _itemLabel;
        [SerializeField] private Button _collectButton;

        private void Awake()
        {
            _collectButton.onClick.AddListener(OnCollectButtonPressed);
        }

        public void ShowItem(WheelItem item)
        {
            _rewardMenuTransform.gameObject.SetActive(true);
            _itemImage.sprite = item.Icon;
            _itemLabel.text = item.Label;
        }

        private void OnCollectButtonPressed()
        {
            _rewardMenuTransform.gameObject.SetActive(false);
        }
    }
}