using UnityEngine;

namespace Framework
{
    public partial class AudioManager
    {
        public float bgmVolume
        {
            get => _bgmVolume;
            set
            {
                _bgmVolume = value;
                UpdateBgmVolume();
            }
        }
        private float _bgmVolume = 0.3f;

        private AudioPlayer bgmPlayer;

        public AudioClip currentBgm => bgmPlayer.currentClip;

        public float bgmTime => bgmPlayer.time;

        private void InitializeBgm()
        {
            bgmPlayer = new(transform, "Bgm", true, bgmVolume * totalVolume);
        }

        public void PlayBgm(string location, bool loop = true, float time = 0)
        {
            bgmPlayer.loop = loop;
            bgmPlayer.time = time;
            bgmPlayer.Play(location);
        }

        public void PlayBgm(AudioClip clip)
        {
            bgmPlayer.Play(clip);
        }

        public void StopBgm()
        {
            bgmPlayer.Stop();
        }

        private void UpdateBgmVolume()
        {
            bgmPlayer.targetVolume = bgmVolume * totalVolume;
        }
    }
}
