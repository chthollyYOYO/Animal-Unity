using Framework;
using Spine.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XNode;

namespace GameEditor
{
    public static class EditorHelper
    {
        public const string DATA_DIRECTION = "Assets/EditorData";

        public static string[] CheckDropFiles(EditorWindow window)
        {
            //鼠标位于当前窗口
            if (EditorWindow.mouseOverWindow == window)
            {
                //拖入窗口未松开鼠标
                if (Event.current.type == EventType.DragUpdated)
                {
                    //改变鼠标外观
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                //拖入窗口并松开鼠标
                else if (Event.current.type == EventType.DragExited)
                {
                    //获取焦点，使unity置顶(在其他窗口的前面)
                    window.Focus();

                    //可以使用鼠标位置判断进入指定区域
                    //Rect rect = EditorGUILayout.GetControlRect();
                    //rect.Contains(Event.current.mousePosition);
                    if (DragAndDrop.paths != null)
                    {
                        return DragAndDrop.paths;
                    }
                }
            }
            return null;
        }

        public static void CreateFolderIfNotExist(List<string> parentFolderNames)
        {
            string dirPath = "Assets";
            for (int i = 0; i < parentFolderNames.Count; i++)
            {
                CreateFolderIfNotExist(dirPath, parentFolderNames[i]);
                dirPath = Path.Combine(dirPath, parentFolderNames[i]);
            }
        }

        public static void CreateFolderIfNotExist(string dirPath, string folderName)
        {
            string path = Path.Combine(dirPath, folderName);
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(dirPath, folderName);
        }

        public static T GUIDToAsset<T>(string guid) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(guid))
                return null;

            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
        }

        public static string AssetToGUID(UnityEngine.Object assetObject)
        {
            if (assetObject == null)
                return null;

            var assetPath = AssetDatabase.GetAssetPath(assetObject);
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            return guid;
        }

        public static string GUIDToAssetPath(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return null;
            return AssetDatabase.GUIDToAssetPath(guid);
        }

        public static string FindSpineAnimationReferenceGUIDBySameName(string sourceAnimationReferenceGUID, string targetAnimationReferenceDirectory)
        {
            var sourceAsset = GUIDToAsset<AnimationReferenceAsset>(sourceAnimationReferenceGUID);
            if (sourceAsset != null)
            {
                var targetPath = $"{targetAnimationReferenceDirectory}/{sourceAsset.name}.asset";
                var targetAsset = AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>(targetPath);
                if (targetAsset != null)
                {
                    return AssetToGUID(targetAsset);
                }
                else
                {
                    UnityEngine.Debug.LogError($"目标资源未找到:{targetPath}");
                    return string.Empty;
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"源资源未找到:{sourceAnimationReferenceGUID}");
                return string.Empty;
            }
        }

        public static T ReadJson<T>(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return Helper.JsonDeserialize<T>(json);
            }
            else
            {
                return default;
            }
        }

        public static T ReadProto<T>(string path)
        {
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                return Helper.ProtoDeserialize<T>(bytes);
            }
            else
            {
                return default;
            }
        }

        public static void SaveJson(object obj, string path)
        {
            var json = Helper.JsonSerialize(obj);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }

        public static void SaveProto(object obj, string path)
        {
            var bytes = Helper.ProtoSerialize(obj);
            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
        }

        public static T ReadOrCreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            if (File.Exists(path))
            {
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                return obj;
            }
            else
            {
                var obj = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(obj, path);
                return obj;
            }
        }

        public static int[] GetConnectionNodeIds(NodePort port)
        {
            var connections = port.GetConnections();
            var result = new int[connections.Count];
            for (int i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                result[i] = port.node.graph.nodes.IndexOf(connection.node);
            }
            return result;
        }
    }
}