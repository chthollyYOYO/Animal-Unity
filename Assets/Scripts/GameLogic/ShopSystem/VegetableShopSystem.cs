using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public partial class VegetableShopSystem : Singleton<VegetableShopSystem>
    {
        private int money {
            get => Record.current.money;
            set => Record.current.SetMoney(value);
        }

        public event Action itemNumChange;
        public List<ShelfItem> shelfItems => Record.current.vegetableShelfItems;

        List <float> discountRate = new List<float> {0, 0.8f, 0.7f, 0.5f};
        List <float> priceIncreateRate = new List<float> {0, 1.5f, 1.2f };

        public void Destroy()
        {
            shelfItems.Clear();
        }

        public void Update()
        {
            Destroy();
            OnStock();
            ItemSaleOrUp();
            itemNumChange?.Invoke();
        }
        public void OnStock()
        {
            if (Record.current.vegetableShopLevel == 1)
            {
                foreach (var cfg in ConfigManager.GetItemCfgList())
                {
                    if (cfg.Type == 2 && cfg.Level == 1)
                    {
                        if (!cfg.IsNumLimit)
                        {
                            var item = ShelfItem.Create(Item.Create(cfg.Id, 9999),cfg.SellPrice);
                            shelfItems.Add(item);
                        }
                        else
                        {
                            var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                            item.limit = true;
                            shelfItems.Add(item);
                        }
                    }
                }
            }

            if (Record.current.vegetableShopLevel == 2)
            {
                foreach (var cfg in ConfigManager.GetItemCfgList())
                {
                    if (cfg.Type == 2 && cfg.Level < 3)
                    {
                        if (!cfg.IsNumLimit)
                        {
                            var item = ShelfItem.Create(Item.Create(cfg.Id, 9999), cfg.SellPrice);
                            shelfItems.Add(item);
                        }
                        else
                        {
                            if (cfg.Level == 1)
                            {
                                var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                                item.limit = true;
                                shelfItems.Add(item);
                            }
                            else if (cfg.Level == 2)
                            {
                                var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                                item.limit = true;
                                shelfItems.Add(item);
                            }
                        }
                    }
                }
            }

            if (Record.current.vegetableShopLevel == 3)
            {
                foreach (var cfg in ConfigManager.GetItemCfgList())
                {
                    if (cfg.Type == 2 && cfg.Level <= 3)
                    {
                        if (!cfg.IsNumLimit)
                        {
                            var item = ShelfItem.Create(Item.Create(cfg.Id, 9999), cfg.SellPrice);
                            shelfItems.Add(item);
                        }
                        else
                        {
                            if (cfg.Level == 1)
                            {
                                var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                                item.limit = true;
                                shelfItems.Add(item); 
                            }
                            else if (cfg.Level == 2)
                            {
                                var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                                item.limit = true;
                                shelfItems.Add(item);
                            }
                            else if (cfg.Level == 3)
                            {
                                var item = ShelfItem.Create(Item.Create(cfg.Id, cfg.LimitNum), cfg.SellPrice);
                                item.limit = true;
                                shelfItems.Add(item);
                            }
                        }
                    }
                }
            }
        }
 
        private void ItemSaleOrUp()
        {
            //随机抽取商品
            List<ShelfItem> randomPool = new List<ShelfItem>(shelfItems);
            List<int> onSaleItemId = new List<int>();
            List<int> priceUpItemId = new List<int>();

            int index;
            if (Record.current.date % 3 != 0)
                return;

            if (Record.current.vegetableShopLevel == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    index = UnityEngine.Random.Range(0, randomPool.Count);
                    int itemId = randomPool[index].item.id;
                    randomPool.RemoveAt(index);
                    onSaleItemId.Add(itemId);
                }
                for (int i = 0; i < 1; i++)
                {
                    index = UnityEngine.Random.Range(0, randomPool.Count);
                    int itemId = randomPool[index].item.id;
                    randomPool.RemoveAt(index);
                    priceUpItemId.Add(itemId);
                }
            }
            else if (Record.current.vegetableShopLevel == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    index = UnityEngine.Random.Range(0, randomPool.Count);
                    int itemId = randomPool[index].item.id;
                    randomPool.RemoveAt(index);
                    onSaleItemId.Add(itemId);
                }
                for (int i = 0; i < 3; i++)
                {
                    index = UnityEngine.Random.Range(0, randomPool.Count);
                    int itemId = randomPool[index].item.id;
                    randomPool.RemoveAt(index);
                    priceUpItemId.Add(itemId);
                }
            }
            else if (Record.current.vegetableShopLevel == 3)
            {
                for (int i = 0; i < 4; i++)
                {
                    index = UnityEngine.Random.Range(0, randomPool.Count);
                    int itemId = randomPool[index].item.id;
                    randomPool.RemoveAt(index);
                    onSaleItemId.Add(itemId);
                }
            }

            //改变价格
            if (onSaleItemId == null)
                return;
            int Level = Record.current.vegetableShopLevel;

            for (int i = 0; i < onSaleItemId.Count; i++)
            {
                foreach (var item in shelfItems)
                {
                    if (item.item.id == onSaleItemId[i])
                    {
                        item.itemPrice = Mathf.CeilToInt(item.itemPrice * discountRate[Level]);
                        item.priceChanged = ShelfItem.PriceChanged.priceDown;
                        break;
                    }
                }
            }

            if (Level != 3)
            {
                for (int i = 0; i < priceUpItemId.Count; i++)
                {
                    foreach (var item in shelfItems)
                    {
                        if (item.item.id == priceUpItemId[i])
                        {
                            item.itemPrice = Mathf.CeilToInt(item.itemPrice * priceIncreateRate[Level]);
                            item.priceChanged = ShelfItem.PriceChanged.priceUp;
                            break;
                        }
                    }
                }
            }
        }

        public void BuyByItemId(int itemId, int itemCount)
        {
            foreach (var item in shelfItems)
            {
                if (item.item.id == itemId)
                {
                    Buy(item, itemCount);
                    return;
                }
            }
        }

        public void BuyByIndex(int index, int itemCount)
        {
            var item = shelfItems[index];
            Buy(item, itemCount);
        }

        private void Buy(ShelfItem shelfItem, int itemCount)
        {
            float cost = shelfItem.itemPrice;
            float totalCost = cost * itemCount;
            Item item = new();
            item.quality = shelfItem.item.quality;
            item.count = itemCount;
            item.id = shelfItem.item.id;

            foreach (var goods in shelfItems)
            {
                if (goods == shelfItem)
                {
                    goods.item.count -= itemCount;
                    break;
                }
            }

            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.冰箱, 0);
            if (flag != null)
            {
                if (money >= totalCost)
                {
                    money -= Mathf.CeilToInt(totalCost);
                    var obj = flag.GetComponent<InteractableObject_IceBox>();
                        obj.TryAddItem(item);
                    Debug.Log("Bought ");
                    itemNumChange?.Invoke();
                }
                else
                    Debug.Log("哦豁，么钱啦");
            }
        }
    }   
}