using Framework;
using UnityEngine;

namespace GameLogic
{
    public partial class DialogueSystem
    {
        private AudioClip cacheBgm;

        private void CacheSceneBgm()
        {
            cacheBgm = AudioManager.Instance.currentBgm;
        }

        private void PlaySceneBgm()
        {
            AudioManager.Instance.PlayBgm(cacheBgm);
        }
    }
}