using System;
using Framework;
using UI;
using UnityEngine;

namespace GameLogic
{
    public class UltySystem : Singleton<UltySystem>
    {
        public event Action onUltyValueChanged;
        public float ultyValue
        {
            get => _ultyValue;
            set
            {
                _ultyValue = value;
                onUltyValueChanged?.Invoke();
            }
        }
        private float _ultyValue;

        public float ultyMax = 5;
        public float duration = 10;

        public event Action onUltyStateChanged;
        public bool isTimeStop
        {
            get => _isTimeStop;
            set
            {
                _isTimeStop = value;
                onUltyStateChanged?.Invoke();
            }
        }
        private bool _isTimeStop;

        public void FullEnergy()
        {
            ultyValue = ultyMax;
        }

        public void EnergyUp()
        {
            float ultyValue = this.ultyValue + 1;
            this.ultyValue = Mathf.Clamp(ultyValue, 0, ultyMax);
        }

        public void UltyPress()
        {
            if (IsFull())
            {
                StartUlty();
            }
        }

        public void StartUlty()
        {
            if (isTimeStop)
                return;

            isTimeStop = true;
            GameMonoSingleton.Instance.onUpdate += LogicUpdate;
            GameMonoSingleton.Instance.gameSpeed = 0;
            UIManager.Instance.ShowPanelSync<UI_Panel_Ulty>();
        }

        private void OnStopUlty()
        {
            isTimeStop = false;
            GameMonoSingleton.Instance.onUpdate -= LogicUpdate;
            GameMonoSingleton.Instance.gameSpeed = 1;
            UIManager.Instance.HidePanel<UI_Panel_Ulty>();
        }

        public void LogicUpdate(float deltaTime)
        {
            float decayRate = ultyMax / duration;
            ultyValue -= decayRate * deltaTime;
            if (ultyValue <= 0)
            {
                ultyValue = 0;
                OnStopUlty();
            }
        }

        public bool IsFull()
        {
            return Mathf.Abs(ultyMax - ultyValue) < 1e-4;
        }
    }
}