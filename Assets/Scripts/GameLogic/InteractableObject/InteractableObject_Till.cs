using FairyGUI;
using Framework;
using UI;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 收银台
    /// </summary>
    public class InteractableObject_Till : InteractableObject
    {
        private float longPressDuration;
        private Transform till_top;
        private UI_Component_TableManual ui_tableManual;
        //private bool OnStalling;
  
        protected override void Start()
        {
            //选座UI界面组件
            base.Start();
            till_top = transform.FindDeepChild("Till_Top");
            //ui_tableManual ??= UIPackage.CreateObject("Main", "Panel_TableManual") as UI_Panel_TableManual;
            ui_tableManual = SceneUIManager.Instance.CreateUI<UI_Component_TableManual>("Main", "Component_TableManual");
            //ui_tableManual.xy = UICamera.Instance.WorldPointToUI(till_top.position);
            //ui_tableManual.visible = false;
            //UI_Panel_Main.current.GetScene().AddChild(ui_tableManual);
            //OnStalling = false;
            ui_tableManual.Hide();
        }

        private void OnDestroy()
        {
            SceneUIManager.Instance.DestroyUI(ui_tableManual);
        }

        //private void LateUpdate()
        //{
        //    //ui_tableManual.xy = UICamera.Instance.WorldPointToUI(till_top.position);
        //    //if (base.Selecting && StallSystem.Instance.liningDinerIds.Count != 0)
        //    //     UI_Panel_Main.current.ChangeInteractButtonStyle(5);
        //    //else if(StallSystem.Instance.isStalling)
        //    //    if (base.Selecting && StallSystem.Instance.liningDinerIds.Count == 0)
        //    //        UI_Panel_Main.current.ChangeInteractButtonStyle(0);
        //    //if (base.Selecting && StallSystem.Instance.liningDinerIds.Count == 0)
        //    //    ui_tableManual.OnHide();
        //}

        //public override void OnSelected()
        //{
        //    base.OnSelected();
        //    if (!StallSystem.Instance.isStalling && !StallSystem.Instance.isServing)
        //        UI_Panel_Main.current.ChangeInteractButtonStyle(2);
        //}

        //public override void OnCancelSelected()
        //{
        //    base.OnCancelSelected();
        //    UI_Panel_Main.current.ChangeInteractButtonStyle(0);
        //}

        public override bool IsInteractable()
        {
            return !StallSystem.Instance.isStalling || StallSystem.Instance.IsPeekReady();
        }

        public override void OnLongPress(float deltaTime)
        {
            base.OnLongPress(deltaTime);
            if (!StallSystem.Instance.isStalling)
            {
                longPressDuration += deltaTime;
                UpdateUI();

                if (longPressDuration > 3)
                {
                    OnLongPressEnd();
                    //UI_Panel_Main.current.ChangeInteractButtonStyle(0);
                    StallSystem.Instance.StartStall();
                    InteractableTipManager.Instance.UpdateTip();
                    //OnStalling = true; 
                }
            }
        }

        public override void OnLongPressEnd()
        {
            base.OnLongPressEnd();
            longPressDuration = 0;
            UpdateUI();
        }

        private void UpdateUI()
        {
            UI_Panel_Main.current.OnLongPressInteract(longPressDuration / 3);
        }

        public override void OnInteract()
        {
            base.OnInteract();
            if (/*OnStalling == true*/StallSystem.Instance.isStalling)
            {
                if(StallSystem.Instance.IsPeekReady())
                {
                    ui_tableManual.xy = UICamera.Instance.WorldPointToUI(till_top.position);
                    ui_tableManual.Show();
                    //UI_Panel_Main.current.ChangeInteractButtonStyle(0);
                }
            }
        }
    }
}
