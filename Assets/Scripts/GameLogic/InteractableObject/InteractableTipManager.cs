using Framework;
using System;

namespace GameLogic
{
    public class InteractableTipManager : Singleton<InteractableTipManager>
    {
        public event Action onTipChanged;

        public void Init()
        {
            PlayerController.Instance.onSelectedChanged += UpdateTip;
        }

        public void Destroy()
        {
            PlayerController.Instance.onSelectedChanged -= UpdateTip;
        }

        public bool isShowTip
        {
            get => _isShowTip;
            set
            {
                if (_isShowTip != value)
                {
                    _isShowTip = value;
                    onTipChanged?.Invoke();
                }
            }
        }
        private bool _isShowTip = true;

        public int tipStyle
        {
            get => _tipStyle;
            set
            {
                if (_tipStyle != value)
                {
                    _tipStyle = value;
                    onTipChanged?.Invoke();
                }
            }
        }
        private int _tipStyle;

        public void UpdateTip()
        {
            var obj = PlayerController.Instance.currentSelectedObject;
            if (obj is InteractableObject_Car
                or InteractableObject_StallDoor)
            {
                tipStyle = 6;
            }
            else if (obj is InteractableObject_DrinkBox
                or InteractableObject_IceBox
                or InteractableObject_WarmBox
                or InteractableObject_Trashcan
                or InteractableObject_Kitchenware
                or InteractableObject_Shop
                or InteractableObject_Npc
                )
            {
                tipStyle = 1;
            }
            else if (obj is InteractableObject_Table table)
            {
                if (!table.Clean)
                {
                    tipStyle = 4;
                }
                else if (table.GetServeable(out _))
                {
                    tipStyle = 3;
                }
            }
            else if (obj is InteractableObject_Till)
            {
                if (StallSystem.Instance.isStalling)
                {
                    tipStyle = 5;
                }
                else
                {
                    tipStyle = 2;
                }
            }
            else
            {
                tipStyle = 0;
            }
        }
    }
}
