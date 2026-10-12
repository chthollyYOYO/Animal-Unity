using PlasticPipe.PlasticProtocol.Messages;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class BeatmapEditorWindow : OdinEditorWindow
{
    [ValueDropdown("GetPrefabs")]
    [OnValueChanged("OnSelectionChanged")]
    [LabelText("当前资源")]
    public GameObject prefab;

    private GameObject gameObject;
    private PlayableDirector director;
    private TimelineAsset timeline;

    private static string FILE_TIMELINE(int id) => $"Assets/Resources/Beatmap/Timelines/{id}.playable";
    private static string FILE_GAMEOBJECT(int id) => $"Assets/Resources/Beatmap/Prefabs/{id}.prefab";

    private Dictionary<IBeatmapEmitter, IBeatmapChecker> dict = new();

    [MenuItem("Editor/音游编辑器")]
    public static void Open()
    {
        GetWindow<BeatmapEditorWindow>().Show();
    }

    [Button("新建")]
    private void Add()
    {
        int id = 1;
        while (true)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FILE_GAMEOBJECT(id));
            if (prefab == null)
                break;
            id++;
        }

        var timeline = CreateInstance<TimelineAsset>();
        AssetDatabase.CreateAsset(timeline, FILE_TIMELINE(id));

        GameObject gameObject = new(id.ToString());
        var director = gameObject.AddComponent<PlayableDirector>();
        director.playableAsset = timeline;
        PrefabUtility.SaveAsPrefabAsset(gameObject, FILE_GAMEOBJECT(id));
        DestroyImmediate(gameObject);

        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FILE_GAMEOBJECT(id));
        OnSelectionChanged();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EditorApplication.update += OnUpdate;
    }

    protected virtual void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        ClearScene();
    }

    private ValueDropdownList<GameObject> GetPrefabs()
    {
        ValueDropdownList<GameObject> result = new();
        int id = 1;
        while (true)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(FILE_GAMEOBJECT(id));
            if (prefab == null)
                break;
            result.Add(prefab);
            id++;
        }
        return result;
    }

    private void OnSelectionChanged()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Beatmap.unity");
            ClearScene();

            gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            director = gameObject.GetComponent<PlayableDirector>();
            timeline = director.playableAsset as TimelineAsset;
        }
    }

    private void ClearScene()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "Beatmap")
        {
            var gameObjects = scene.GetRootGameObjects();
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                var gameObject = gameObjects[i];
                if (!gameObject.CompareTag("MainCamera"))
                    DestroyImmediate(gameObject);
            }
        }
    }

    private void OnUpdate()
    {
        if (gameObject == null)
            return;

        var tracks = timeline.GetOutputTracks();
        foreach (var track in tracks)
        {
            var markers = track.GetMarkers();
            foreach (var marker in markers)
            {
                if (marker is IBeatmapEmitter emitter)
                {
                    //var chargeBarClip = FindChargeBarClip(marker);
                    //if (chargeBarClip != null)
                    //{
                    //    float timeOffset = (float)(director.time - chargeBarClip.start);
                    //    float duration = (float)chargeBarClip.duration;

                    //    if (BeatmapChargeBar.IsVaild(timeOffset, duration) > 0)
                    //    {
                    //        var checker = UpdateEmitter(emitter, () => BeatmapOnceCube.Create()) as BeatmapOnceCube;
                    //        checker.UpdateChecker(timeOffset);
                    //    }
                    //    else
                    //    {
                    //        DestroyEmitter(emitter);
                    //    }
                    //}
                    //else
                    //{
                        float timeOffset = (float)(director.time - marker.time);
                        if (BeatmapOnce.IsVaild(timeOffset))
                        {
                            var checker = UpdateEmitter(emitter, () => BeatmapOnce.Create()) as BeatmapOnce;
                            checker.UpdateChecker(timeOffset);
                        }
                        else
                        {
                            DestroyEmitter(emitter);
                        }
                    //}
                }
            }

            if (track is LongPressTrack lpTrack)
            {
                foreach (var clip in lpTrack.GetClips())
                {
                    if (clip.asset is LongPressClip lpClip)
                    {
                        float timeOffset = (float)(director.time - clip.start);
                        float duration = (float)clip.duration;
                        if (BeatmapLongPress.IsVaild(timeOffset, duration) > 0)
                        {
                            var checker = UpdateEmitter(lpClip.template, () => BeatmapLongPress.Create()) as BeatmapLongPress;
                           // checker.UpdateChecker(timeOffset, duration);
                            checker.Checker(timeOffset, duration);
                            checker.Moving(timeOffset, duration);
                        }
                        else
                        {
                            DestroyEmitter(lpClip.template);
                        }
                    }
                }
            }
            ////chargebar判定
            //if (track is ChargeBarTrack cbtrack)
            //{
            //    foreach (var clip in cbtrack.GetClips())
            //    {
            //        if (clip.asset is ChargeBarClip cbclip)
            //        {
            //            float timeOffset = (float)(director.time - clip.start);
            //            float duration = (float)clip.duration;
            //            if (BeatmapChargeBar.IsVaild(timeOffset, duration) > 0)
            //            {
            //                var checker = UpdateEmitter(cbclip.template, () => BeatmapChargeBar.Create()) as BeatmapChargeBar;
            //                checker.UpdateChecker(timeOffset, duration);
            //            }
            //            else
            //            {
            //                DestroyEmitter(cbclip.template);
            //            }
            //        }
            //    }
            //}
        }

        foreach (var key in dict.Keys.ToArray())
        {
            if (key == null)
            {
                dict[key].Destroy();
                dict.Remove(key);
            }
        }
    }

    private IBeatmapChecker UpdateEmitter(IBeatmapEmitter emitter, Func<IBeatmapChecker> creator)
    {
        if (!dict.TryGetValue(emitter, out var checker))
        {
            checker = creator();
            dict.Add(emitter, checker);

            checker.position = emitter.Position;
            checker.rotation = emitter.Rotation;
        }
        else
        {
            if (emitter.Position != checker.position)
            {
                emitter.Position = checker.position;
                EditorUtility.SetDirty(timeline);
            }
            if (emitter.Rotation != checker.rotation)
            {
                emitter.Rotation = checker.rotation;
                EditorUtility.SetDirty(timeline);
            }
        }

        checker.SetStyle(emitter.KeyCode);
        return checker;
    }

    private void DestroyEmitter(IBeatmapEmitter emitter)
    {
        if (dict.TryGetValue(emitter, out var checker))
        {
            checker.Destroy();
            dict.Remove(emitter);
        }
    }

    //private TimelineClip FindChargeBarClip(IMarker marker)
    //{
    //    foreach (var track in timeline.GetOutputTracks())
    //    {
    //        if (track is ChargeBarTrack cbTrack)
    //        {
    //            foreach (var cbClip in cbTrack.GetClips())
    //            {
    //                if (cbClip.start <= marker.time && marker.time <= cbClip.end)
    //                {
    //                    return cbClip;
    //                }
    //            }
    //        }
    //    }
    //    return null;
    //}
}