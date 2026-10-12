//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine.Timeline;
//using UnityEngine;

//[CustomEditor(typeof(ChargeBarClip))]
//public class ChargeBarClipEditor : Editor
//{
//    private const string CUBE_PREFAB_PATH =
//        "Assets/Resources/Beatmap/Components/BeatmapOnceCube.prefab";

//    private float newCubeTime = 0f;

//    public override void OnInspectorGUI()
//    {
//        ChargeBarClip clip = target as ChargeBarClip;

//        if (clip == null)
//            return;

//        EditorGUI.BeginChangeCheck();

//        DrawDefaultInspector();

//        if (EditorGUI.EndChangeCheck())
//        {
//            RefreshChargeBarPreview(clip);

//            EditorUtility.SetDirty(clip);
//            AssetDatabase.SaveAssets();
//        }

//        EditorGUILayout.Space();
//        EditorGUILayout.LabelField("ChargeBar Cube Judge Points", EditorStyles.boldLabel);

//        newCubeTime = EditorGUILayout.FloatField("New Cube Time", newCubeTime);

//        if (GUILayout.Button("Add Cube Judge Point"))
//        {
//            AddJudgePoint(clip, newCubeTime);
//            CreatePreviewCube(clip, newCubeTime);
//        }

//        EditorGUILayout.Space();

//        if (clip.template.judgePoints != null)
//        {
//            for (int i = 0; i < clip.template.judgePoints.Count; i++)
//            {
//                EditorGUILayout.BeginHorizontal();

//                EditorGUILayout.LabelField(
//                    $"Point {i}",
//                    clip.template.judgePoints[i].startTime.ToString("0.00")
//                );


//                if (GUILayout.Button("Remove", GUILayout.Width(70)))
//                {
//                    clip.template.judgePoints.RemoveAt(i);

//                    EditorUtility.SetDirty(clip);
//                    AssetDatabase.SaveAssets();

//                    EditorGUILayout.EndHorizontal();
//                    break;
//                }

//                EditorGUILayout.EndHorizontal();
//            }
//        }
//    }

//    private void AddJudgePoint(ChargeBarClip clip, float judgeTime)
//    {
//        if (clip.template.judgePoints == null)
//            clip.template.judgePoints = new List<ChargeBarJudgePoint>();

//        clip.template.judgePoints.Add(new ChargeBarJudgePoint
//        {
//            startTime = judgeTime
//        });

//        EditorUtility.SetDirty(clip);
//        AssetDatabase.SaveAssets();
//    }

//    private void CreatePreviewCube(ChargeBarClip clip, float judgeTime)
//    {
//        GameObject cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CUBE_PREFAB_PATH);

//        if (cubePrefab == null)
//        {
//            Debug.LogWarning($"找不到 Cube Prefab：{CUBE_PREFAB_PATH}");
//            return;
//        }

//        BeatmapChargeBar chargeBar = Object.FindObjectOfType<BeatmapChargeBar>();

//        if (chargeBar == null)
//        {
//            Debug.LogWarning("场景里没有 BeatmapChargeBar 预览对象");
//            return;
//        }

//        if (chargeBar.bar == null)
//        {
//            Debug.LogWarning("BeatmapChargeBar 的 bar 没有赋值");
//            return;
//        }

//        float duration = GetCurrentTimelineClipDuration(clip);

//        chargeBar.UpdateBar(duration);

//        float barLength = duration * chargeBar.speed;

//        float distance = judgeTime * chargeBar.speed;
//        distance = Mathf.Clamp(distance, 0, barLength);

//        float left = -barLength * 0.5f;

//        Vector3 localPos = Vector3.zero;
//        localPos.x = left + distance;
//        localPos.y = 0f;
//        localPos.z = -0.1f;

//        GameObject cube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;

//        if (cube == null)
//        {
//            Debug.LogWarning("Cube Prefab 实例化失败");
//            return;
//        }

//        cube.name = $"Preview_Cube_{judgeTime:0.00}";

//        cube.transform.SetParent(chargeBar.bar.transform, false);
//        cube.transform.localPosition = localPos;
//        cube.transform.localRotation = Quaternion.identity;
//        cube.transform.localScale = Vector3.one;

//        SetSortingOrder(cube, 10);

//        EditorUtility.SetDirty(cube);
//    }

//    private void RefreshChargeBarPreview(ChargeBarClip clip)
//    {
//        BeatmapChargeBar chargeBar = Object.FindObjectOfType<BeatmapChargeBar>();

//        if (chargeBar == null)
//            return;

//        chargeBar.UpdateBar(clip.template.previewDuration);
//    }

//    private void SetSortingOrder(GameObject obj, int order)
//    {
//        var renderers = obj.GetComponentsInChildren<SpriteRenderer>(true);

//        foreach (var renderer in renderers)
//        {
//            renderer.sortingOrder = order;
//        }
//    }
//    private float GetCurrentTimelineClipDuration(ChargeBarClip targetClip)
//{
//    string[] guids = AssetDatabase.FindAssets("t:TimelineAsset");

//    foreach (string guid in guids)
//    {
//        string path = AssetDatabase.GUIDToAssetPath(guid);
//        TimelineAsset timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(path);

//        if (timeline == null)
//            continue;

//        foreach (var track in timeline.GetOutputTracks())
//        {
//            if (track is ChargeBarTrack cbTrack)
//            {
//                foreach (var timelineClip in cbTrack.GetClips())
//                {
//                    if (timelineClip.asset == targetClip)
//                    {
//                        return (float)timelineClip.duration;
//                    }
//                }
//            }
//        }
//    }

//    return 1f;
//}
//}