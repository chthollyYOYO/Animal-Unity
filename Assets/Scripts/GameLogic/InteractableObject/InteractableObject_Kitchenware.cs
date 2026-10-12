using FairyGUI;
using Framework;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    /// <summary>
    /// 厨具
    /// </summary>
    [RequireComponent(typeof(SceneFlag))]
    public class InteractableObject_Kitchenware : InteractableObject
    {
        public enum CookProgress
        {
            Free = 0,
            Cooking,
            Completed,
        }

        public event UnityAction onItemChanged;

        public int kitchenwareId => (int)sceneFlag.flagType;

        private SceneFlag sceneFlag;
        private Transform tf_top;
        private Transform tf_bottom;

        public CookProgress progress;
        public Item[] inItems = new Item[4];
        public Item outItem;
        public float duration;
        public float time;
        private GProgressBar ui_prog;
        private UI_Component_CookCompleted ui_completed;

        protected override void Start()
        {
            base.Start();
            sceneFlag = GetComponent<SceneFlag>();
            tf_top = transform.FindDeepChild("Top");
            tf_bottom = transform.FindDeepChild("Bottom");

            ui_prog = SceneUIManager.Instance.CreateUI<GProgressBar>("Main", "ProgressBar_Cook");
            ui_prog.visible = false;

            ui_completed = SceneUIManager.Instance.CreateUI<UI_Component_CookCompleted>("Main", "Component_CookCompleted");
            ui_completed.visible = false;
        }

        protected virtual void OnDestroy()
        {
            //ui_prog?.Dispose();
            //ui_completed?.Dispose();
            SceneUIManager.Instance.DestroyUI(ui_prog);
            SceneUIManager.Instance.DestroyUI(ui_completed);
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

        public override bool IsInteractable()
        {
            return progress is CookProgress.Free or CookProgress.Completed;
        }

        public override void OnInteract()
        {
            base.OnInteract();
            if (progress == CookProgress.Free)
            {
                UIManager.Instance.ShowPanelSync<UI_Panel_Kitchenware, InteractableObject_Kitchenware>(this);
            }
            else if (progress == CookProgress.Completed)
            {
                if (BagSystem.Instance.TryAddItem(outItem))
                {
                    outItem = null;
                    progress = CookProgress.Free;
                    ui_prog.visible = false;
                    ui_completed.visible = false;
                }
            }
        }

        public void StartCooking(/*GComponent parent*/)
        {
            progress = CookProgress.Cooking;

            duration = outItem.GetCookingTime();
            time = 0;

            //ui_prog ??= UIPackage.CreateObject("Main", "ProgressBar_Cook") as GProgressBar;
            ui_prog.visible = true;
            switch (sceneFlag.flagType)
            {
                case SceneFlagType.烤架:
                    AudioManager.Instance.PlaySound("Music/jiaohu/shaokao");
                    break;
                case SceneFlagType.炒锅:
                    AudioManager.Instance.PlaySound("Music/jiaohu/chaoguo");
                    break;
                case SceneFlagType.炖锅:
                    AudioManager.Instance.PlaySound("Music/jiaohu/dunguo");
                    break;
                case SceneFlagType.蒸锅:
                    AudioManager.Instance.PlaySound("Music/jiaohu/zhenglong");
                    break;
            }
            ui_prog.max = 1;
            ui_prog.value = 0;
            //parent.AddChild(ui_prog);

            //ui_completed ??= UIPackage.CreateObject("Main", "Component_CookCompleted") as UI_Component_CookCompleted;
            ui_completed.visible = true;
            ui_completed.lod_icon.url = outItem.GetIcon();
            //parent.AddChild(ui_completed);
        }

        public void LateUpdate()
        {
            if(ui_completed != null)
                ui_completed.xy = UICamera.Instance.WorldPointToUI(tf_top.position);
            if(ui_prog!= null)
                ui_prog.xy = UICamera.Instance.WorldPointToUI(tf_bottom.position);
        }

        public void CompleteCooking()
        {
            inItems.RemoveAll();
            ui_prog.visible = false;
            progress = CookProgress.Completed;
        }

        private void Update()
        {
            OnLogicUpdate(Time.deltaTime * GameMonoSingleton.Instance.gameSpeed);
        }

        private void OnLogicUpdate(float deltaTime)
        {
            if (progress != CookProgress.Cooking)
                return;
            time += deltaTime;
            if (time > duration)
            {
                time = duration;
                CompleteCooking();
            }
            else
            {
                ui_prog.value = time / duration;
            }
        }

        public bool TryAddItem(Item item)
        {
            var result = inItems.TryAdd_NonStackable(item);
            if (result)
            {
                CheckRecipe();
                onItemChanged?.Invoke();
            }
            return result;
        }

        public Item RemoveItem(int index)
        {
            var item = inItems.RemoveOne(index);
            CheckRecipe();
            onItemChanged?.Invoke();
            return item;
        }

        public Item GetInItem(int index)
        {
            return inItems[index];
        }

        public List<Item> RemoveAllItems()
        {
            List<Item> items = inItems.RemoveAll();
            CheckRecipe();
            onItemChanged?.Invoke();
            return items;
        }

        private void CheckRecipe()
        {
            var itemIds = inItems.Where(it => it != null).Select(it => it.id).ToArray();
            int itemId = RecipeSystem.Instance.GetOutItem(itemIds, kitchenwareId);
            if (itemId > 0)
                outItem = Item.Create(itemId, 1);
            else
                outItem = null;
        }
    }
}
