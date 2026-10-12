using Cysharp.Threading.Tasks;
using Framework;
using System;
using System.Linq;
using UI;

namespace GameLogic
{
    public partial class StallSystem : Singleton<StallSystem>
    {
        public event Action onStallingChanged;
        public bool isStalling
        {
            get => _isStalling;
            set
            {
                if (_isStalling != value)
                {
                    _isStalling = value;
                    onStallingChanged?.Invoke();
                }
            }
        }
        private bool _isStalling;

        public event Action onTimeChanged;
        public float remainTime
        {
            get => _remainTime;
            set
            {
                _remainTime = value;
                onTimeChanged?.Invoke();
            }
        }
        private float _remainTime;

        private int money
        {
            get => Record.current.money;
            set => Record.current.SetMoney(value);
        }

        public void StartStall()
        {
            if (isStalling)
                return;

            isStalling = true;
            remainTime = 300;
            InitTables();
            InitDiners();

            GameMonoSingleton.Instance.onLogicUpdate += OnLogicUpdate;
        }

        public void StopStall()
        {
            isStalling = false;
            ForceDinerLeave();
            ClearLining();
            GameMonoSingleton.Instance.onLogicUpdate -= OnLogicUpdate;
            _StopStall();
        }

        private async void _StopStall()
        {
            bool IsCompleted()
            {
                if (tables.Values.Any(table => !table.Clean))
                    return false;

                if (diners.Values.Any(diner => diner.currAction is DinerAction.等餐 or DinerAction.干饭))
                    return false;

                return true;
            }

            await UniTask.WaitUntil(IsCompleted);
            UIManager.Instance.ShowPanelSync<UI_Panel_StallEnd>();
        }

        public void OnLogicUpdate(float deltaTime)
        {
            OnLogicUpdate_DinerCreator(deltaTime);

            remainTime -= deltaTime;
            if (remainTime <= 0)
            {
                StopStall();
            }
        }
    }
}
