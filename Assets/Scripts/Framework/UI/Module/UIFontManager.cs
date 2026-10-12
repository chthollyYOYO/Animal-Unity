//using Cysharp.Threading.Tasks;
//using FairyGUI;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Framework
//{
//    public class UIFontManager : Singleton<UIFontManager>
//    {
//        private const string assetPath1 = "Assets/RuntimeData/Font/HappyZcool-2016.ttf";
//        private const string assetPath2 = "Assets/RuntimeData/Font/MSYH.TTC";
//        private List<ResourceHandle> handles = new();

//        public async UniTask Initialize()
//        {
//            var handle = await ResourceManager.LoadAssetAsync(assetPath1);
//            handles.Add(handle);
//            if (handle.TryGetAssetObject<Font>(out var font))
//            {
//                DynamicFont dFont = new();
//                dFont.name = "HappyZcool-2016";
//                dFont.nativeFont = font;
//                FontManager.RegisterFont(dFont, "HappyZcool-2016");
//            }
//            else
//            {
//                Debug.LogError($"字体资源异常");
//            }

//            handle = await ResourceManager.LoadAssetAsync(assetPath2);
//            handles.Add(handle);
//            if (handle.TryGetAssetObject<Font>(out var font2))
//            {
//                DynamicFont dFont2 = new();
//                dFont2.name = "Microsoft YaHei";
//                dFont2.nativeFont = font2;
//                FontManager.RegisterFont(dFont2, "Microsoft YaHei");

//                UIConfig.defaultFont = "Microsoft YaHei";
//            }
//            else
//            {
//                Debug.LogError($"字体资源异常");
//            }
//        }
//    }
//}
