using UnityEngine;

namespace Framework
{
    public class FPSCalculator : Singleton<FPSCalculator>
    {
        public float deltaTime = 0.5f;
        public float fps;

        private float lastTime;
        private int frameCount;

        public FPSCalculator()
        {
            GameMonoSingleton.Instance.onUpdate += OnUpdate;
        }

        ~FPSCalculator()
        {
            GameMonoSingleton.Instance.onUpdate -= OnUpdate;
        }

        private void OnUpdate(float _)
        {
            frameCount++;
            if (Time.realtimeSinceStartup >= lastTime + deltaTime)
            {
                fps = frameCount / (Time.realtimeSinceStartup - lastTime);
                lastTime += deltaTime;
                frameCount = 0;
            }
        }
    }
}
