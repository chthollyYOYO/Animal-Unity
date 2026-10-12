using Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public partial class BookStoreSystem : Singleton<BookStoreSystem>
    {
        private int money {
            get => Record.current.money;
            set => Record.current.SetMoney(value);
        }

        public event Action itemNumChange;
        public List<ShelfItem> shelfItems => Record.current.bookStoreShelfItems;

        public void Destroy()
        {
            shelfItems.Clear();
        }
        public void BuyByItemId(int itemId)
        {
            foreach (var item in shelfItems)
            {
                if (item.item.id == itemId)
                {
                    Buy(item);
                }
            }
        }
        public void BuyByIndex(int itemIndex)
        {
            var shelfitem = shelfItems[itemIndex];
            Buy(shelfitem);
        }
        private void Buy(ShelfItem shelfItem)
        {
            float cost = shelfItem.itemPrice;
            money -= shelfItem.itemPrice;
            shelfItem.item.count -= 1;
            itemNumChange?.Invoke();
            Record.current.obtainedRecipe.Add(shelfItem.item.id);
        }
    }
}