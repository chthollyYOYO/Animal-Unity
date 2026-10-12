using GameLogic;
using System;
using System.Collections.Generic;

namespace Framework
{
    public partial class Record
    {
        public static Record current;

        public string name;
        public int date = 1;
        public int money = 2000;
        public Item[] bag = new Item[4];
        public Item[] iceBox = new Item[16];
        public Item[] drinkBox = new Item[16];
        public Item[] warmBox = new Item[16];
        public List<int> obtainedRecipe = new();

        public event Action onMoneyChanged;

        public int vegetableShopLevel = 1;
        public List<ShelfItem> vegetableShelfItems = new List<ShelfItem>();

        public int bookStoreLevel = 1;
        public List<ShelfItem> bookStoreShelfItems = new List<ShelfItem>();

        public List<Item> giftBagItems = new List<Item>();

        public List<int> completedDialogues = new();

        [NonSerialized]
        public int timePeriod = 1; // 1, 2, 3
        [NonSerialized]
        public float secondsThisPeriod;

        partial void OnCreate()
        {
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                if (cfg.Type == 3 && cfg.IsDefault)
                    obtainedRecipe.Add(cfg.Id);
            }

            //-----------测试用------------
            //塞东西到冰箱
            List<int> rawItems = new List<int>()
            {
                2011,
                2002,//
                2018,
                2001,//
                2015,//
                2020,//
                2012,//
            };
            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
            for (int i = 0; i < rawItems.Count; i++)
            {
                var item = Item.Create(rawItems[i], 100);
                if (flag != null)
                {
                    var obj = flag.GetComponent<InteractableObject_IceBox>();
                    obj.TryAddItem(item);
                }
                else
                {
                    iceBox.TryAdd_Stackable(item);
                }
            }
            //解锁所有食谱
            //foreach (var cfg in ConfigManager.GetItemCfgList())
            //{
            //    if (cfg.Type == 3 && !obtainedRecipe.Contains(cfg.Id))
            //        obtainedRecipe.Add(cfg.Id);
            //}
            //-------------End-------------

            //------------菜店初始数据----------------
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                if (cfg.Type == 2 && cfg.Level == vegetableShopLevel)
                {
                    if (!cfg.IsNumLimit)
                    {
                        var item = ShelfItem.Create(Item.Create(cfg.Id, 9999), cfg.SellPrice);
                        vegetableShelfItems.Add(item);
                    }
                    else
                    {
                        var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                        item.limit = true;
                        vegetableShelfItems.Add(item);
                    }
                }
            }
            //-------------书店初始数据-------------
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                if (cfg.Level == bookStoreLevel && cfg.IsSellRecipe)
                {
                    var item = ShelfItem.Create(Item.Create(cfg.Id, 1), cfg.RecipeSellPrice);
                    bookStoreShelfItems.Add(item);                 
                }
            }

            //----------礼物背包初始数据--------------
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                if (cfg.Type == 6)
                {
                    var item = Item.Create(cfg.Id, 1);
                    giftBagItems.Add(item);
                }
            }
        }

        public void ChangeMoney(int changeValue)
        {
            money += changeValue;
            onMoneyChanged?.Invoke();
        }

        public void SetMoney(int value)
        {
            money = value;
            onMoneyChanged?.Invoke();
        }

        public void AddCompletedDialogue(int groupId)
        {
            if (!completedDialogues.Contains(groupId))
                completedDialogues.Add(groupId);
        }

        public bool IsCompletedDialogue(int groupId)
        {
            return completedDialogues.Contains(groupId);
        }

        public bool IsRecipeEnough()
        {
            return obtainedRecipe.Contains(3018)
                && obtainedRecipe.Contains(3019)
                && obtainedRecipe.Contains(3022);
        }
    }
}
