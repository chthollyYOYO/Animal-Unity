using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public partial class AudioManager
    {
        public float soundVolume
        {
            get => _soundVolume;
            set
            {
                _soundVolume = value;
                UpdateSoundVolume();
            }
        }
        private float _soundVolume = 1;

        private GameObject soundGameObject;
        private Dictionary<int, AudioPlayer> soundPlayers = new();

        private int nextSoundId = 1;

        private void InitializeSound()
        {
            soundGameObject = new("Sound");
            soundGameObject.transform.parent = transform;
        }

        /// <returns>audioPlayerId，可用于停止音效</returns>
        public int PlaySound(string location)
        {
            var audioClip = ResourceManager.LoadAssetSync<AudioClip>(location);
            if (audioClip == null)
                return 0;

            int id = nextSoundId++;

            AudioPlayer player = new(soundGameObject.transform, location, false, soundVolume * totalVolume);
            player.fadeInDuration = 0;
            player.Play(location);
            soundPlayers.Add(id, player);
            AutoStopSound(id, audioClip.length);
            return id;
        }

        private async void AutoStopSound(int id, float audioClipLength)
        {
            await UniTask.Delay((int)(audioClipLength * 1000));
            StopSound(id);
        }

        public void StopSound(int audioPlayerId)
        {
            if (soundPlayers.TryGetValue(audioPlayerId, out var player))
            {
                player.Destroy();
                soundPlayers.Remove(audioPlayerId);
            }
        }

        private void UpdateSoundVolume()
        {
            foreach (var player in soundPlayers.Values)
                player.targetVolume = soundVolume * totalVolume;
        }
    }
}
