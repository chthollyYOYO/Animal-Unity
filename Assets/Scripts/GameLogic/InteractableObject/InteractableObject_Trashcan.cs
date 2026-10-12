using Framework;
using UI;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 垃圾桶
    /// </summary>
    [RequireComponent(typeof(SceneFlag))]
    public class InteractableObject_Trashcan : InteractableObject
    {
        public SceneFlag sceneFlag;

        protected override void Start()
        {
            base.Start();
            sceneFlag = GetComponent<SceneFlag>();
        }

        //public override void OnSelected()
        //{
        //    base.OnSelected();
        //    UI_Panel_Main.current.ChangeInteractButtonStyle(1);
        //}

        //public override void OnCancelSelected()
        //{
        //    base.OnCancelSelected();
        //    UI_Panel_Main.current.ChangeInteractButtonStyle(0);
        //}

        public override void OnInteract()
        {
            base.OnInteract();
            UIManager.Instance.ShowPanelSync<UI_Panel_Trashcan>();
        }
    }
}
