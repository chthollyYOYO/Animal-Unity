using UnityEditor;

namespace GameEditor.AssetImporter
{
    public class CustomTextureImporter : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            //检查是否是UI文件夹下的资源
            if (assetPath.Contains("Assets/Resources/UI/"))
            {
                //获取纹理导入器
                TextureImporter textureImporter = (TextureImporter)assetImporter;

                ////设置纹理类型为Sprite
                //textureImporter.textureType = TextureImporterType.Default;
                ////设置精灵类型为2D
                //textureImporter.spriteImportMode = SpriteImportMode.Single;
                ////设置过滤模式为Bilinear
                //textureImporter.filterMode = FilterMode.Bilinear;
                ////设置纹理格式为RGBA32
                //textureImporter.alphaIsTransparency = false;

                //设置最大尺寸为4096
                textureImporter.maxTextureSize = 4096;

                //应用修改
                textureImporter.SaveAndReimport();
            }
        }
    }
}