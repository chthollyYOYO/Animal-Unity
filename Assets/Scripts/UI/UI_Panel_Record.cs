using FairyGUI;
using Framework;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public partial class UI_Panel_Record : ICreate, IShow<int>, IHide
    {
        private InputController_UI controller = new();
        private Dictionary<long, Record> records = new();
        private bool isLongPressDelete;
        private float? startTime;

        private Record GetRecord(int index) => records.ContainsKey(index) ? records[index] : null;

        public void OnCreate()
        {
            controller.onClickItem += OnClickItem;
            controller.onClickClose += OnClickClose;
            controller.onLongPressDeleteStart += OnLongPressDeleteStart;
            controller.onLongPressDelete += OnLongPressDelete;
            controller.onLongPressDeleteEnd += OnLongPressDeleteEnd;

            list_record.itemRenderer = RecordRenderer;
        }

        public void OnShow(int style)
        {
            ctrl_style.selectedIndex = style;
            Update();

            controller.RegisterObjects(list_record.GetChildren());
            controller.SetSelection(list_record.GetChildAt(0));

            InputSystem.Instance.RegisterController(controller);
        }

        public void OnHide()
        {
            InputSystem.Instance.UnRegisterController(controller);
        }

        private void Update()
        {
            this.records.Clear();
            var records = RecordManager.Instance.ReadAll();
            foreach (var record in records)
                this.records.Add(record.recordId, record);
            list_record.numItems = 8;
            com_opL.Update(0);
        }

        private void RecordRenderer(int index, GObject obj)
        {
            var ui = obj as UI_Button_Record;
            var record = GetRecord(index);
            ui.Update(record);
        }

        private void OnClickItem(GObject obj)
        {
            this.CloseSelf();

            int index = list_record.GetChildIndex(obj);
            var record = GetRecord(index);
            record ??= RecordManager.Instance.Create(index);
            GameStart.EnterGame(record);
        }

        private void OnClickClose()
        {
            this.CloseSelf();
            UIManager.Instance.ShowPanelSync<UI_Panel_Start>();
        }

        private void OnLongPressDeleteStart(GObject obj)
        {
            int index = list_record.GetChildIndex(obj);
            var record = GetRecord(index);
            if (record != null)
                isLongPressDelete = true;
        }

        private void OnLongPressDelete(GObject obj)
        {
            if (isLongPressDelete)
            {
                if (!startTime.HasValue)
                    startTime = Time.time;

                float duration = Time.time - startTime.Value;
                float per = Mathf.Clamp01(duration / 3);
                com_opL.Update(per);

                if (duration > 3)
                {
                    int index = list_record.GetChildIndex(obj);
                    var record = GetRecord(index);
                    RecordManager.Instance.Delete(record.recordId);

                    controller.OnLongPressDeleteEnd();
                    Update();
                }
            }
        }

        private void OnLongPressDeleteEnd()
        {
            isLongPressDelete = false;
            startTime = null;
            com_opL.Update(0);
        }
    }
}