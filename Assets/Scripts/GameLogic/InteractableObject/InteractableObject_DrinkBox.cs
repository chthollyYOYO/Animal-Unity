using Framework;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    /// <summary>
    /// 饮料柜
    /// </summary>
    [RequireComponent(typeof(SceneFlag))]
    public class InteractableObject_DrinkBox : InteractableObject, IInteractableObject_Box
    {
        public SceneFlag sceneFlag;

        public UnityAction onItemChanged { get; set; }

        public Item[] items => Record.current.drinkBox;

        public SceneFlagType flagType => sceneFlag.flagType;

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
            UIManager.Instance.ShowPanelSync<UI_Panel_Box, IInteractableObject_Box>(this);
        }

        public bool TryAddItem(Item item)
        {
            if (item.GetItemType() != 5)
                return false;

            bool result = items.TryAdd_Stackable(item);
            if (result)
            {
                onItemChanged?.Invoke();
            }
            return result;
        }

        public Item RemoveItem(int index)
        {
            var item = items.RemoveOne(index);
            onItemChanged?.Invoke();
            return item;
        }

        public List<Item> RemoveAllItems()
        {
            var items = this.items.RemoveAll();
            onItemChanged?.Invoke();
            return items;
        }
    }
}
