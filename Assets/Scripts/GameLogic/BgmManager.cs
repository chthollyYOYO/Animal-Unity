using Framework;

namespace GameLogic
{
    public class BgmManager : Singleton<BgmManager>
    {
        private float timeTemp;

        public void Initialize()
        {
            PlayerController.Instance.onSceneMoveStart += StopBgm;
            PlayerController.Instance.onSceneMoveEnd += PlayBgm;
            StallSystem.Instance.onStallingChanged += PlayBgm;
            UltySystem.Instance.onUltyStateChanged += PlayBgm;
            GuideSystem.Instance.onGuideIdChanged += PlayBgm;
            PlayBgm();
        }

        public void Destroy()
        {
            PlayerController.Instance.onSceneMoveStart -= StopBgm;
            PlayerController.Instance.onSceneMoveEnd -= PlayBgm;
            StallSystem.Instance.onStallingChanged -= PlayBgm;
            UltySystem.Instance.onUltyStateChanged -= PlayBgm;
            GuideSystem.Instance.onGuideIdChanged -= PlayBgm;
        }

        public void StopBgm()
        {
            AudioManager.Instance.StopBgm();
        }

        public void PlayBgm()
        {
            if (PlayerController.Instance.currentScene == SceneName.旧街场景)
            {
                AudioManager.Instance.PlayBgm("Music/xuzhang/BGM1");
            }
            else if (PlayerController.Instance.currentScene == SceneName.经营场景)
            {
                if (StallSystem.Instance.isStalling)
                {
                    if (UltySystem.Instance.isTimeStop)
                    {
                        timeTemp = AudioManager.Instance.bgmTime;
                        AudioManager.Instance.PlayBgm("Music/xuzhang/dazhao");
                    }
                    else if (GuideSystem.Instance.guideId == 1)
                    {
                        AudioManager.Instance.PlayBgm("Music/xuzhang/BGM-kaidian");
                    }
                    else
                    {
                        AudioManager.Instance.PlayBgm("Music/xuzhang/BGM-youwan", time: timeTemp);
                        timeTemp = 0;
                    }
                }
                else
                {
                    AudioManager.Instance.PlayBgm("Music/xuzhang/BGM-kaidian");
                }
            }
        }
    }
}
