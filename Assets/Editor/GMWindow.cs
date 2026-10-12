using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using GameLogic;
using Framework;
using System.Collections.Generic;
using System;
using UI;
using Unity.VisualScripting;


namespace GameEditor
{
    public class GMWindow : OdinEditorWindow
    {
        [NonSerialized]
        private List<int> rawItems = new List<int>()
        {
            2011,
            2002,//
            2018,
            2001,//
            2015,//
            2020,//
            2012,//
        };

        private int money
        {
            get => Record.current.money;
            set => Record.current.SetMoney(value);
        }
        [MenuItem("Tools/GM窗口")]
        public static void Open()
        {
            GetWindow<GMWindow>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            titleContent = new("GM窗口");
        }

        [FoldoutGroup("通用")]
        [Button("学会所有食谱")]
        private void AddAllRecipes()
        {
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                var recipes = Record.current.obtainedRecipe;
                if (cfg.Type == 3 && !recipes.Contains(cfg.Id))
                    recipes.Add(cfg.Id);
            }
        }

        [FoldoutGroup("物品")]
        [ValueDropdown("@Framework.Helper.GetItemList()")]
        [LabelText("物品")]
        public int itemId;

        [FoldoutGroup("物品")]
        [LabelText("数量")]
        public int itemCount = 1;

        [FoldoutGroup("物品")]
        [Button("添加到背包")]
        private void AddItemToBag()
        {
            BagSystem.Instance.TryAddItem(itemId);
        }

        [FoldoutGroup("物品")]
        [Button("添加到冰箱、饮料柜、保温箱")]
        private void AddItemToBox()
        {
            var cfg = ConfigManager.GetItemCfg(itemId);
            var item = Item.Create(itemId, itemCount);
            if (cfg.Type == 2)
            {
                var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
                if (flag != null)
                {
                    var obj = flag.GetComponent<InteractableObject_IceBox>();
                    obj.TryAddItem(item);
                }
                else
                {
                    Record.current.iceBox.TryAdd_Stackable(item);
                }
            }
            else if (cfg.Type == 3)
            {
                var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.保温箱, 0);
                if (flag != null)
                {
                    var obj = flag.GetComponent<InteractableObject_WarmBox>();
                    obj.TryAddItem(item);
                }
                else
                {
                    Record.current.warmBox.TryAdd_NonStackable(item);
                }
            }
            else if (cfg.Type == 5)
            {
                var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
                if (flag != null)
                {
                    var obj = flag.GetComponent<InteractableObject_IceBox>();
                    obj.TryAddItem(item);
                }
            }
        }

        [FoldoutGroup("物品")]
        [Button("购买")]
        private void Buy()
        {
            VegetableShopSystem.Instance.BuyByItemId(itemId, itemCount);
        }

        [Button("加钱")]
        private void AddMoney(int amount)
        {
            money += amount;
        }
      
        [Button("一件添加至冰箱")]
        private void RawItemToIceBox()
        {
            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
            for (int i = 0; i < rawItems.Count; i++)
            {
                var item = Item.Create(rawItems[i],10);
                if (flag != null)
                {
                    var obj = flag.GetComponent<InteractableObject_IceBox>();
                    obj.TryAddItem(item);
                }
                else
                {
                    Record.current.iceBox.TryAdd_Stackable(item);
                }
            }
        }
        [FoldoutGroup("物品")]
        [Button("打开礼物背包")]
        private void GiftBag()
        {
            GiftBagParam param = new();
            UIManager.Instance.ShowPanelForget<UI_Panel_GiftBag, GiftBagParam>(param);
        }
        [FoldoutGroup("物品")]
        [Button("清空背包")]
        private void ClearBag()
        {
            BagSystem.Instance.RemoveAllItems();
        }

        [FoldoutGroup("对话")]
        [LabelText("ID")]
        public int dialogueId;

        [FoldoutGroup("对话")]
        [Button("开始")]
        private void DialogueStart()
        {
            DialogueSystem.Instance.StartDialogue(dialogueId);
        }

        [FoldoutGroup("对话")]
        [Button("结束")]
        private void DialogueStop()
        {
            DialogueSystem.Instance.StopDialogue();
        }

        [FoldoutGroup("物品")]
        [Button("清空冰箱、饮料柜、保温箱")]
        private void ClearBox()
        {
            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
            if (flag != null)
            {
                var obj = flag.GetComponent<InteractableObject_IceBox>();
                obj.RemoveAllItems();
            }
            else
            {
                Record.current.iceBox.RemoveAll();
            }

            flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.保温箱, 0);
            if (flag != null)
            {
                var obj = flag.GetComponent<InteractableObject_WarmBox>();
                obj.RemoveAllItems();
            }
            else
            {
                Record.current.warmBox.RemoveAll();
            }

            flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.饮料柜, 0);
            if (flag != null)
            {
                var obj = flag.GetComponent<InteractableObject_DrinkBox>();
                obj.RemoveAllItems();
            }
            else
            {
                Record.current.drinkBox.RemoveAll();
            }
        }

        [FoldoutGroup("经营")]
        [Button("倒计时")]
        private void Timer(int time)
        {
            StallSystem.Instance.remainTime = time;
        }

        [FoldoutGroup("经营")]
        [Button("大招能量")]
        private void Ulty()
        {
            UltySystem.Instance.FullEnergy();
        }

        [FoldoutGroup("音游")]
        [Button("音游")]
        private void RythmGame(int ID)
        {
            BeatmapSystem.Instance.StartBeat(ID);
        }

        [FoldoutGroup("音游")]
        [Button("结束")]
        private void Stop()
        {
            BeatmapSystem.Instance.Stop();
        }

        [FoldoutGroup("音游")]
        [Button("重新播放")]
        private void Restart()
        {
            BeatmapSystem.Instance.ReStart();
        }

        //[FoldoutGroup("商店")]
        //[Button("进入菜店")]
        //private void VegetableShop()
        //{
        //    UIManager.Instance.ShowPanelForget<UI_Panel_VegetableShop>();
        //}
        //[FoldoutGroup("商店")]
        //[Button("进入书店")]
        //private void BookStore()
        //{
        //    UIManager.Instance.ShowPanelForget<UI_Panel_Bookstore>();
        //}

        [Button("日期")]
        private void Date(int date)
        {
            Record.current.date = date;
            VegetableShopSystem.Instance.Update();
        }
        [FoldoutGroup("商店")]
        [Button("菜店等级")]
        private void ShopLevel(int level)
        {
            Record.current.vegetableShopLevel = level;
            VegetableShopSystem.Instance.Update();
        }

        [Button("Save")]
        private void save()
        {
            RecordManager.Instance.Save(Record.current);
        }
    }
}