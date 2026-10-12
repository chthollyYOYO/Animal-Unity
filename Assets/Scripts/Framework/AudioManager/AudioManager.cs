namespace Framework
{
    public partial class AudioManager : MonoSingleton<AudioManager>
    {
        public float totalVolume
        {
            get => _totalVolume;
            set
            {
                _totalVolume = value;
                UpdateVolume();
            }
        }
        private float _totalVolume = 1;

        public void Initialize()
        {
            InitializeAmbience();
            InitializeBgm();
            InitializeSound();
        }

        private void UpdateVolume()
        {
            UpdateAmbienceVolume();
            UpdateBgmVolume();
            UpdateSoundVolume();
        }
    }
}
