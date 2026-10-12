using Cysharp.Threading.Tasks;
using Framework;
using System;
using UI;
using UnityEngine;

namespace GameLogic
{
    public partial class PlayerController
    {
        int qteCount = 0;
        //QTE正误状态记录
        bool qteStatus = false;
        public event Action<bool> onQteEnd;
        private InputController_QTE qteController = new();
        private UI_Component_QTEList ui_qte;
        private (int, int)[] qteData;

        private void InitQTE()
        {
            //ui_qte = UIPackage.CreateObject("Main", "Component_QTEList") as UI_Component_QTEList;
            ui_qte = SceneUIManager.Instance.CreateUI<UI_Component_QTEList>("Main", "Component_QTEList");
            //UI_Panel_Main.current.GetScene().AddChild(ui_qte);
            ui_qte.visible = false;

            qteController.onKeyDown += qteOnKeyDown;
        }

        private void DestroyQTE()
        {
            SceneUIManager.Instance.DestroyUI(ui_qte);
        }

        //<summary>
        //处理做菜QTE
        //<summary>
        public void StartQte(int[] directions, Action<bool> onQteEnd)
        {
            qteData = new (int, int)[directions.Length];
            for (int i = 0; i < directions.Length; i++)
                qteData[i] = (directions[i], 0);
            this.onQteEnd = onQteEnd;
            ui_qte.xy = UICamera.Instance.WorldPointToUI(tf_top.position);
            //弹出QTE UI
            ui_qte.visible = true;
            ui_qte.Update(qteData);
            InputSystem.Instance.RegisterController(qteController);
            qteCount = 0;

        }

        private void qteOnKeyDown(KeyCode keyCode)
        {
            //up方向键和W
            if ((keyCode == KeyCode.UpArrow || keyCode == KeyCode.W) && qteData[qteCount].Item1 == 1)
            {
                //QTE成功，颜色转绿
                qteData[qteCount].Item2 = 1;
                //更新QTE UI颜色
                ui_qte.Update(qteData);
                //计数增加
                qteCount++;
            }
            //left方向键和A
            else if ((keyCode == KeyCode.LeftArrow || keyCode == KeyCode.A) && qteData[qteCount].Item1 == 2)
            {
                qteData[qteCount].Item2 = 1;
                ui_qte.Update(qteData);
                qteCount++;
            }
            else if ((keyCode == KeyCode.RightArrow || keyCode == KeyCode.D) && qteData[qteCount].Item1 == 4)
            {
                qteData[qteCount].Item2 = 1;
                ui_qte.Update(qteData);
                qteCount++;
            }
            else if ((keyCode == KeyCode.DownArrow || keyCode == KeyCode.S) && qteData[qteCount].Item1 == 3)
            {
                qteData[qteCount].Item2 = 1;
                ui_qte.Update(qteData);
                qteCount++;
            }
            else
            {
                //QTE失败，状态更新为False
                qteStatus = false;
                qteEnd();
            }

            //当QTE全部完成，无需继续按键
            if (qteCount == qteData.Length)
            {
                qteEnd();
                qteStatus = true;
            }
        }

        InputController_UI tmpCtrl = new();
        private async void qteEnd()
        {
            InputSystem.Instance.UnRegisterController(qteController);

            InputSystem.Instance.RegisterController(tmpCtrl);
            await UniTask.Delay(500);
            InputSystem.Instance.UnRegisterController(tmpCtrl);

            ui_qte.visible = false;
            onQteEnd?.Invoke(qteStatus);
        }
    }
}