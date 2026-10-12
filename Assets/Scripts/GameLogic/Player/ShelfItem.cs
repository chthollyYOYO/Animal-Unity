using System;
using Framework;

namespace GameLogic
{
    public class ShelfItem
    {
        public Item item;
        public string name;
        public int itemPrice;
        public bool limit;
        public enum PriceChanged
        {
            None,
            priceUp,
            priceDown,
        }
        public PriceChanged priceChanged;
        public static ShelfItem Create(Item item, int itemPrice)
        {
            ShelfItem shelfItem = new();
            shelfItem.item = item;
            shelfItem.itemPrice = itemPrice;
            shelfItem.priceChanged = PriceChanged.None;
            foreach (var cfg in ConfigManager.GetItemCfgList())
            {
                if (cfg.Id == shelfItem.item.id)
                {
                    shelfItem.name = cfg.Name;
                    break;
                }
            }
            return shelfItem;
        }
    }   
}