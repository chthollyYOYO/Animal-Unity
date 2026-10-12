using UnityEngine;

namespace Framework
{
    public class AudioPlayer
    {
        public float fadeInDuration = 1.5f;
        public float fadeOutDuration = 0.5f;

        public AudioClip currentClip
        {
            get => audioSource.clip;
            private set
            {
                audioSource.clip = value;
                if (value == null)
                    audioSource.Stop();
                else
                    audioSource.Play();
            }
        }

        public AudioClip targetClip
        {
            get;
            private set;
        }

        private float currentVolume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }

        public float targetVolume
        {
            get => _targetVolume;
            set
            {
                _targetVolume = value;
                if (!isUpdatting)
                    audioSource.volume = value;
            }
        }
        private float _targetVolume;

        public bool loop
        {
            get => audioSource.loop;
            set => audioSource.loop = value;
        }

        public float time
        {
            get => audioSource.time;
            set => audioSource.time = value;
        }

        private GameObject gameObject;
        private AudioSource audioSource;

        private bool isUpdatting;

        public AudioPlayer(Transform parent, string name, bool loop, float volume = 1)
        {
            gameObject = new(name);
            gameObject.transform.parent = parent;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = loop;

            targetVolume = volume;
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(gameObject);

            TryRemoveListener();
        }

        public void Play(string location)
        {
            var clip = ResourceManager.LoadAssetSync<AudioClip>(location);
            Play(clip);
        }

        public void Play(AudioClip clip)
        {
            if (clip == null)
                return;

            targetClip = clip;
            TryAddListener();
            OnUpdate(0);
        }

        public void Stop()
        {
            targetClip = null;
            TryAddListener();
            OnUpdate(0);
        }

        private void OnUpdate(float deltaTime)
        {
            if (currentClip == targetClip) // && currentClip != null && targetClip != null
            {
                if (currentVolume < targetVolume)
                {
                    currentVolume += deltaTime / fadeInDuration;
                }
                else
                {
                    currentVolume = targetVolume;
                    TryRemoveListener();
                }
            }
            else
            {
                if (currentClip == null) // && targetClip != null
                {
                    currentClip = targetClip;
                    currentVolume = 0;
                }
                else if (targetClip == null) // && currentClip != null
                {
                    if (currentVolume > 0)
                    {
                        currentVolume -= deltaTime / fadeOutDuration;
                    }
                    else
                    {
                        currentVolume = 0;
                        TryRemoveListener();
                    }
                }
                else // currentClip != null && targetClip != null
                {
                    if (currentVolume > 0)
                    {
                        currentVolume -= deltaTime / 0.5f;
                    }
                    else
                    {
                        currentClip = targetClip;
                        currentVolume = 0;
                    }
                }
            }
        }

        private void TryAddListener()
        {
            if (!isUpdatting)
            {
                isUpdatting = true;
                GameMonoSingleton.Instance.onUpdate += OnUpdate;
            }
        }

        private void TryRemoveListener()
        {
            if (isUpdatting)
            {
                isUpdatting = false;
                GameMonoSingleton.Instance.onUpdate -= OnUpdate;
            }
        }
    }
}
