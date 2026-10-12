using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Framework
{
    public class VideoManager : MonoSingleton<VideoManager>
    {
        private Dictionary<string, VideoClip> clips = new();

        public UniTask LoadAsset(string videoName)
        {
            clips[videoName] = Resources.Load<VideoClip>($"Video/{videoName}");
            return UniTask.CompletedTask;
        }

        public void UnloadAsset(string videoName)
        {
            if (clips.TryGetValue(videoName, out var clip))
            {
                Resources.UnloadAsset(clip);
                clips.Remove(videoName);
            }
        }

        public RenderTexture PlayInRenderTexture(string videoName, Action onFinished = null)
        {
            if (TryGetVideoClip(videoName, true, out var videoClip))
            {
                GameObject go = new("VideoPlayer");
                go.transform.parent = transform;

                var videoPlayer = go.AddComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                videoPlayer.Play();

                var renderTexture = RenderTexture.GetTemporary((int)videoClip.width, (int)videoClip.height);
                videoPlayer.targetTexture = renderTexture;

                SimpleTimer.Create((float)videoClip.length, () =>
                {
                    RenderTexture.ReleaseTemporary(renderTexture);
                    UnityEngine.Object.Destroy(go);
                    onFinished?.Invoke();
                });

                return renderTexture;
            }
            return null;
        }

        public bool TryGetVideoClip(string videoName, bool logError, out VideoClip videoClip)
        {
            videoClip = null;
            if (clips.TryGetValue(videoName, out var clip))
            {
                videoClip = clip;
                return true;
            }
            else
            {
                if (logError)
                    LogManager.Error($"Video {videoName} not loaded.");
                return false;
            }
        }
    }
}
