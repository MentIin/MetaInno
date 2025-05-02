using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.Logic
{
    public static class LogicExtensions
    {
        public static void SetSpriteAndPivot(this Image image, RectTransform rectTransform, Sprite sprite)
        {
            image.sprite = sprite;

            rectTransform.pivot = sprite.pivot / sprite.rect.size;
        }

        public static bool CheckIfLayerInMask(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}