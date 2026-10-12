using System;
using System.Collections.Generic;
using Framework;
using GameLogic;
using UI;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BeatmapSystem : Singleton<BeatmapSystem>
{
    public bool isPlaying;
    public Action stopPlaying;
    private PlayableDirector director;
    private GameObject gameObject;

    //private Camera cameraBefore;
    private Vector3 lastCameraPostion;
    private float lastCameraSize;

    private List <JudgeNote> judgeNotes = new List<JudgeNote>();
    private List <JudgeNote> onJudgeNotes = new List<JudgeNote>();
    
    private static string FILE_TIMELINE(int id) => $"Beatmap/Prefabs/{id}";
    private float judgePerfect = 0.2f;

    public void StartBeat(int ID)
    {



        //cameraBefore = Camera.main;
        //Helper.SetMainCameraEnable(cameraBefore, false);
        //Helper.SetMainCameraEnable(CameraManager.Instance.RythmCamera, true);
        //Helper.ChangeMainCamera(CameraManager.Instance.RythmCamera);
        lastCameraPostion = Camera.main.transform.position;
        lastCameraSize = Camera.main.orthographicSize;
        Camera.main.transform.position = new(0, 0.54f, -10f);
        Camera.main.orthographicSize = 8.86f;

        UIManager.Instance.HidePanel<UI_Panel_Main>();

        var prefab = ResourceManager.LoadAssetSync<GameObject>(FILE_TIMELINE(ID));
        gameObject = UnityEngine.Object.Instantiate(prefab);
        director = gameObject.GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;
        
        foreach (var track in timeline.GetOutputTracks())
        {
            var markers = track.GetMarkers();
            foreach (var marker in markers)
            {
                if (marker is IBeatmapEmitter emitter)
                {
                    judgeNotes.Add(new JudgeNote
                    {
                        startTime = (float)marker.time,
                        keyCode = emitter.KeyCode,
                        position = emitter.Position,
                        rotation = emitter.Rotation,
                        judged = JudgeType.none,
                        spawnTime = (float)(marker.time - IBeatmapChecker.DURATION),
                        Type = NoteType.Once,
                    });
                }
                if (marker is PauseEmitter pause)
                {
                    judgeNotes.Add(new JudgeNote
                    {
                        startTime = (float)marker.time,
                        Type = NoteType.Pause,
                    });
                }
            }
            if (track is LongPressTrack lpTrack)
            {
                foreach (var clip in lpTrack.GetClips())
                {
                    if (clip.asset is LongPressClip lpClip)
                    {
                        if (lpClip.template is IBeatmapEmitter emitter)
                        {
                            judgeNotes.Add(new JudgeNote()
                            {
                                spawnTime = (float)(clip.start - IBeatmapChecker.DURATION),
                                startTime = (float)clip.start,
                                endTime = (float)clip.end,
                                keyCode = emitter.KeyCode,
                                position = emitter.Position,
                                rotation = emitter.Rotation,
                                judged = JudgeType.none,
                                Type = NoteType.Hold,
                            });
                        }
                    }
                }
            }
        }
        judgeNotes.Sort((a, b) => a.startTime.CompareTo(b.startTime));
        GameMonoSingleton.Instance.onLogicUpdate += OnLogicUpdate;

        director.Play();
        isPlaying = true;
        director.stopped += End;
    }

    public void OnLogicUpdate(float deltaTime)
    {
        if (!isPlaying)
            return;
        float currentTime = (float)director.time;
        CreateChecker(currentTime);
        LongPressJudge(currentTime);
        ShortPressJudge(currentTime);
        UpdateOnJudgeList();
        UpdateEffect(deltaTime);
        PauseCheck(currentTime);
    }

    public void ReStart()
    {
        director.Play();
        isPlaying = true;
    }

    private void PauseCheck(float currentIime)
    {
        for (int i = 0; i < judgeNotes.Count; i++)
        {
            if (judgeNotes[i].Type == NoteType.Pause)
            {
                if (currentIime > judgeNotes[i].startTime)
                {
                    director.Pause();
                    judgeNotes.RemoveAt(i);
                    isPlaying = false;
                    BeatmapRestartParm param2 = new()
                    {
                        OnConfirm = ReStart
                    };
                    BeatmapTutorialParam param = new()
                    {
                        OnConfirm = ()=>
                        UIManager.Instance.ShowPanelForget<UI_Anim_CountDown, BeatmapRestartParm>(param2),
                    };

                    UIManager.Instance.ShowPanelForget
                        <UI_Panel_BeatmapTutorial, BeatmapTutorialParam>(param);

                    break;
                }
            }
        }

    }

    private void LongPressJudge(float currentTime)
    {
        //遍历所有已被激活的notes
        foreach (var note in onJudgeNotes)
        {
            if (note.Type == NoteType.Hold && note.judged == JudgeType.none)
            {
                if (!note.holding)
                {
                    float offsetTime1 = Mathf.Abs(currentTime - note.startTime);
                    //第一次摁住，摁住时间点在good window内，至少没miss
                    if (offsetTime1 <= judgePerfect && LongPressStartCheck(note.keyCode))
                    {
                        note.holding = true;
                        Debug.Log("Start Holding");
                        note.holdingStart = currentTime;
                        AudioManager.Instance.PlaySound("Music/jiaohu/anjianyin");

                    }
                    //超过判定窗口，miss
                    else if (currentTime > (note.startTime + judgePerfect))
                    {
                        note.judged = JudgeType.miss;
                        Debug.Log("Hold Missing");
                        if (note.checker is BeatmapLongPress longpress)
                        {
                            longpress.MissClick();
                        }
                    }
                }

                if (note.holding && note.judged == JudgeType.none)
                {
                    float offsetTime2 = Mathf.Abs(currentTime - note.endTime);
                    if (!LongPressCheck(note.keyCode))
                    {
                        if ((currentTime >= note.startTime - judgePerfect) && currentTime < (note.endTime - judgePerfect))
                        {
                            Debug.Log("HoldingBreak, Missing");
                            note.judged = JudgeType.miss;
                            if (note.checker is BeatmapLongPress longpress)
                            {
                                longpress.MissClick();
                            }
                        }
                        else if (offsetTime2 < judgePerfect)
                        {
                            Debug.Log("Hold Perfect");
                            note.judged = JudgeType.perfect;
                            if (note.checker is BeatmapLongPress longpress)
                            {
                                AudioManager.Instance.PlaySound("Music/jiaohu/anjianyin");
                                longpress.PerfectClick();
                                AudioManager.Instance.PlaySound("Music/jiaohu/anjianyin");
                            }
                        }
                        note.holding = false;
                    }

                    if (currentTime > (note.endTime + judgePerfect))
                    {
                        Debug.Log("Longer Than Holding Time, Missing");
                        note.judged = JudgeType.miss;
                        note.holding = false;
                        if (note.checker is BeatmapLongPress longpress)
                        {
                            longpress.MissClick();
                        }
                    }
                }
            }
        }
    }

    private void ShortPressJudge(float CurrentTime)
    {
        foreach (var note in onJudgeNotes)
        {
            if (note.Type == NoteType.Once && note.judged == JudgeType.none)
            {
                float offset = Mathf.Abs(CurrentTime - note.startTime);
                if (ShortPressCheck(note.keyCode))
                {              
                    if (offset <= judgePerfect)
                    {
                        Debug.Log("Perfect");
                        AudioManager.Instance.PlaySound("Music/jiaohu/anjianyin");
                        note.judged = JudgeType.perfect;
                        if (note.checker is BeatmapOnce once)
                        {
                            once.PerfectClick();
                        }
                    }
                }

                if (CurrentTime > (note.startTime + judgePerfect))
                {
                    Debug.Log("Miss");
                    note.judged = JudgeType.miss;
                    if (note.checker is BeatmapOnce once)
                    {
                        once.MissClick();
                    }
                }                
            }
        }
    }
    private void UpdateEffect(float deltaTime)
    {
        foreach (var note in onJudgeNotes)
        {
            if (note.judged == JudgeType.perfect)
            {
                if (note.checker is BeatmapOnce once)
                {
                    bool finished = once.UpdateClickCircle(deltaTime);

                    if (finished)
                    {
                        note.judged = JudgeType.Done;
                    }
                }
                else if (note.checker is BeatmapLongPress longpress)
                {
                    bool finished = longpress.UpdateClickCircle(deltaTime);

                    if (finished)
                        note.judged = JudgeType.Done;
                }
            }
            else if (note.judged == JudgeType.miss)
            {
                if (note.checker is BeatmapOnce once)
                {
                    bool finished = once.UpdateMiss(deltaTime);
                    if (finished)
                        note.judged = JudgeType.Done;
                }
                else if (note.checker is BeatmapLongPress longpress)
                {
                    bool finished = longpress.UpdateMiss(deltaTime);

                    if (finished)
                        note.judged = JudgeType.Done;
                }
            }
        }
    }
    private bool ShortPressCheck(BeatmapKeyCode keycode)
    {
        return keycode switch
        {
            BeatmapKeyCode.Space =>
            Input.GetKeyDown(KeyCode.Space),

            BeatmapKeyCode.W =>
            Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow),

            BeatmapKeyCode.S =>
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow),

            BeatmapKeyCode.A =>
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow),

            BeatmapKeyCode.D =>
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow),

            _ => false,
        };
    }

    private bool LongPressStartCheck(BeatmapKeyCode keyCode)
    {
        return keyCode switch
        {
            BeatmapKeyCode.Space =>
            Input.GetKeyDown(KeyCode.Space),

            BeatmapKeyCode.W =>
            Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow),

            BeatmapKeyCode.S =>
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow),

            BeatmapKeyCode.A =>
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow),

            BeatmapKeyCode.D =>
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow),

            _ => false,
        };
    }
    private bool LongPressCheck(BeatmapKeyCode keycode)
    {
        return keycode switch
        {
            BeatmapKeyCode.Space =>
            Input.GetKey(KeyCode.Space),

            BeatmapKeyCode.W =>
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow),

            BeatmapKeyCode.S =>
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow),

            BeatmapKeyCode.A =>
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow),

            BeatmapKeyCode.D =>
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow),

            _ => false,
        };
    }
    private void CreateChecker(float currentTime)
    {
        foreach (var note in judgeNotes)
        {
            if (note.Type == NoteType.Pause)
                continue;
            if (currentTime >= note.spawnTime && note.checker == null)
            {
                if (note.Type == NoteType.Once)
                    note.checker = BeatmapOnce.Create();
                else if (note.Type == NoteType.Hold)
                    note.checker = BeatmapLongPress.Create();

                note.checker.position = note.position;
                note.checker.rotation = note.rotation;
                note.checker.SetStyle(note.keyCode);
                onJudgeNotes.Add(note);   
            }

            if (note.checker != null)
            {
                if (note.checker is BeatmapOnce once)
                {
                    if (note.judged == JudgeType.miss || note.judged == JudgeType.perfect || note.judged == JudgeType.Done)
                        continue;
                    float timeOffset = currentTime - note.startTime;
                    once.UpdateChecker(timeOffset);    
                }
                else if (note.checker is BeatmapLongPress longpress)
                {
                    if (note.judged == JudgeType.miss || note.judged == JudgeType.perfect || note.judged == JudgeType.Done)
                        continue;
                    float timeOffset = currentTime - note.startTime;
                    float duration = note.endTime - note.startTime;
                    longpress.Checker(timeOffset, duration);
                    if (note.holding)
                    {
                        float offset = currentTime - note.holdingStart;
                        longpress.Moving(offset, duration);
                    }
                }
            }
        }
    }

    private void UpdateOnJudgeList()
    {
        for (int i = onJudgeNotes.Count - 1; i >= 0; i--)
        {
            var note = onJudgeNotes[i];

            if (note.judged == JudgeType.Done)
            {
                note.checker?.Destroy();
                note.checker = null;

                onJudgeNotes.RemoveAt(i);
            }
        }
    }

    
    public void Stop()
    {
        foreach (var note in judgeNotes)
        {
            note.checker?.Destroy();
        }

        foreach (var note in onJudgeNotes)
        {
            note.checker?.Destroy();
        }
        judgeNotes.Clear();
        onJudgeNotes.Clear();
        director.Stop();
    }
    
    private void End(PlayableDirector director)
    {
        GameMonoSingleton.Instance.onLogicUpdate -= OnLogicUpdate;
        isPlaying = false;
        stopPlaying?.Invoke();
        UnityEngine.Object.Destroy(gameObject);
        //Helper.SetMainCameraEnable(cameraBefore, true);
        //Helper.SetMainCameraEnable(CameraManager.Instance.RythmCamera, false);
        //Helper.ChangeMainCamera(cameraBefore);
        Camera.main.transform.position = lastCameraPostion;
        Camera.main.orthographicSize = lastCameraSize;

        UIManager.Instance.ShowPanelForget<UI_Panel_Main>();
    }
}