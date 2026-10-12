using FairyGUI;
using Framework;
using GameLogic;
using System.Collections.Generic;

namespace UI
{
    public partial class UI_Panel_Start : ICreate, IShow, IHide, INotCache
    {
        private InputController_UI controller = new();
        private List<Record> records;
        private bool hasRecord;

        public void OnCreate()
        {
            controller.onClickItem += OnClickItem;
        }

        public void OnShow()
        {
            records = RecordManager.Instance.ReadAll();
            hasRecord = false; //hasRecord = records.Count > 0; 测试期间不使用读档功能
            if (!hasRecord)
                list_menu.RemoveChildAt(0);
            
            controller.RegisterObjects(list_menu.GetChildren());
            controller.SetSelection(list_menu.GetChildAt(0));

            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void OnClickItem(GObject obj)
        {
            int index = list_menu.GetChildIndex(obj);
            if (!hasRecord)
                index += 1;

            switch (index)
            {
                case 0:
                    this.CloseSelf();
                    Record record = records[0];
                    long saveTime = record.lastSaveTime;
                    for (int i = 1; i < records.Count; i++)
                    {
                        if (records[i].lastSaveTime > saveTime)
                        {
                            record = records[i];
                            saveTime = record.lastSaveTime;
                        }
                    }
                    GameStart.EnterGame(record);
                    break;

                case 1:
                    this.CloseSelf();
                    UIManager.Instance.ShowPanelSync<UI_Panel_Record, int>(0);
                    break;

                case 2:
                    //设置
                    break;

                case 3:
                    Helper.ApplicationQuit();
                    break;
            }
        }
    }
}