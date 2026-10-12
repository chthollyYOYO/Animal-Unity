using Spine;
using Spine.Unity;
using Spine.Unity.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor.AssetImporter
{
    public class CustomSpineImporter : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFromAssetPaths)
        {
            //直接从文件管理器拖进来，打开Unity时执行
            List<string> spineNames = new();
            for (int i = 0; i < imported.Length; i++)
            {
                if (imported[i].EndsWith(".skel.bytes"))
                {
                    var skel = imported[i];
                    var path = skel.Replace(".skel.bytes", "");
                    var atlas = Array.Find(imported, s => s == $"{path}.atlas.txt");
                    var png = Array.Find(imported, s => s == $"{path}.png");
                    if (!string.IsNullOrEmpty(atlas) && !string.IsNullOrEmpty(png))
                    {
                        string[] strs = new string[] { skel, atlas, png };
                        CreateSpine(strs);
                    }
                }
            }
            
            //三个文件一起拖进来时可能要替换
            if (imported.Length == 3
                && (Array.Exists(imported, s => s.EndsWith(".skel.bytes")) || Array.Exists(imported, s => s.EndsWith(".skel 1.bytes")))
                && (Array.Exists(imported, s => s.EndsWith(".atlas.txt")) || Array.Exists(imported, s => s.EndsWith(".atlas 1.txt")))
                && Array.Exists(imported, s => s.EndsWith(".png"))
                )
            {
                var dir = Path.GetDirectoryName(imported[0]);

                //已有此文件，将原文件删除、新文件meta删除、新文件的" 1"去掉
                for (int i = 0; i < imported.Length; i++)
                {
                    var filePath = imported[i];
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    if (fileName.EndsWith(" 1"))
                    {
                        var newFileName = fileName.Replace(" 1", "");
                        var newFilePath = Path.Combine(dir, newFileName + Path.GetExtension(filePath));
                        File.Delete(newFilePath);
                        File.Delete($"{filePath}.meta");
                        File.Move(filePath, newFilePath);

                        imported[i] = newFilePath;
                    }
                }

                CreateSpine(imported);
            }
        }

        /// <param name="imported">仅传入Spine的三个文件</param>
        private static void CreateSpine(string[] imported)
        {
            var dir = Path.GetDirectoryName(imported[0]);

            //删除动画引用
            var refDir = Path.Combine(dir, "ReferenceAssets");
            if (Directory.Exists(refDir))
            {
                var filePaths = Directory.GetFiles(refDir);
                Array.ForEach(filePaths, f => File.Delete(f));
            }
            AssetDatabase.Refresh();

            //执行spine生成逻辑
            var png = Array.Find(imported, s => s.EndsWith(".png"));
            AssetUtility.HandleOnPostprocessAllAssets(imported, new() { png });

            //替换材质的shader
            var materialPath = png.Replace(".png", "_Material.mat");
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (materialPath.Contains("BubbleEmoji"))
            {
                material.shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
            }
            else if ((Path.GetDirectoryName(materialPath)).EndsWith("UI"))
            {

            }
            else
            {
                material.shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
            }

            //生成动画引用
            var skeletonDataAssetPath = png.Replace(".png", "_SkeletonData.asset");
            var skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(skeletonDataAssetPath);
            CreateAnimationReferenceAssets(skeletonDataAsset);
        }

        private static void CreateAnimationReferenceAssets(SkeletonDataAsset targetSkeletonDataAsset)
        {
            const string AssetFolderName = "ReferenceAssets";
            SkeletonData targetSkeletonData = targetSkeletonDataAsset.GetSkeletonData(false);
            string parentFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(targetSkeletonDataAsset));
            string dataPath = parentFolder + "/" + AssetFolderName;
            if (!AssetDatabase.IsValidFolder(dataPath))
            {
                AssetDatabase.CreateFolder(parentFolder, AssetFolderName);
            }

            FieldInfo nameField = typeof(AnimationReferenceAsset).GetField("animationName", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo skeletonDataAssetField = typeof(AnimationReferenceAsset).GetField("skeletonDataAsset", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (Spine.Animation animation in targetSkeletonData.Animations)
            {
                string assetPath = string.Format("{0}/{1}.asset", dataPath, AssetUtility.GetPathSafeName(animation.Name));
                AnimationReferenceAsset existingAsset = AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>(assetPath);
                if (existingAsset == null)
                {
                    AnimationReferenceAsset newAsset = ScriptableObject.CreateInstance<AnimationReferenceAsset>();
                    skeletonDataAssetField.SetValue(newAsset, targetSkeletonDataAsset);
                    nameField.SetValue(newAsset, animation.Name);
                    AssetDatabase.CreateAsset(newAsset, assetPath);
                }
            }

            UnityEngine.Object folderObject = AssetDatabase.LoadAssetAtPath(dataPath, typeof(UnityEngine.Object));
            if (folderObject != null)
            {
                Selection.activeObject = folderObject;
                EditorGUIUtility.PingObject(folderObject);
            }
        }
    }
}