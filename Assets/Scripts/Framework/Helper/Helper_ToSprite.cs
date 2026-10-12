using FairyGUI;
using UnityEngine;

namespace Framework
{
    public static partial class Helper
    {
        public static Sprite CreateSpriteByUrl(string url, Vector2 pivot, float pixelsPerUnit = 100)
        {
            if (UIPackage.GetItemAssetByURL(url) is not NTexture nTexture)
                return null;

            if (nTexture.nativeTexture is not Texture2D rootTexture)
                return null;

            var uvRect = nTexture.uvRect;
            Rect pixelRect = new(
                uvRect.x * rootTexture.width,
                uvRect.y * rootTexture.height,
                uvRect.width * rootTexture.width,
                uvRect.height * rootTexture.height
                );

            Sprite sprite = Sprite.Create(rootTexture, pixelRect, pivot, pixelsPerUnit);
            return sprite;
        }
    }
}