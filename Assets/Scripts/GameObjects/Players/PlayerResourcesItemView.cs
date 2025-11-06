using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameObjects.Players
{
    public class PlayerResourcesItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _resourceCount;

        public void SetIcon(Sprite sprite) =>
            _icon.sprite = sprite;

        public void SetCount(int count) =>
            _resourceCount.text = $"{count}";
    }
}