using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public partial class AudioManager
    {
        public float ambienceVolume
        {
            get => _ambienceVolume;
            set
            {
                _ambienceVolume = value;
                UpdateAmbienceVolume();
            }
        }
        private float _ambienceVolume = 1;

        private GameObject ambienceGameObject;
        private Dictionary<string, AudioPlayer> ambiencePlayers = new();

        public void InitializeAmbience()
        {
            ambienceGameObject = new("Ambience");
            ambienceGameObject.transform.parent = transform;
        }

        public void PlayAmbience(string location)
        {
            if (ambiencePlayers.ContainsKey(location))
                return;

            AudioPlayer player = new(ambienceGameObject.transform, location, true, ambienceVolume * totalVolume);
            player.Play(location);
            ambiencePlayers.Add(location, player);
        }

        public void StopAmbience(string location)
        {
            if (!ambiencePlayers.TryGetValue(location, out var player))
                return;

            player.Destroy();
            ambiencePlayers.Remove(location);
        }

        private void UpdateAmbienceVolume()
        {
            foreach (var player in ambiencePlayers.Values)
                player.targetVolume = ambienceVolume * totalVolume;
        }
    }
}
