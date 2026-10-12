using System;
using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Button_Recipe
    {
        public void Update(ShelfItem shelfItem)
        {
            lod_item.url = shelfItem.item?.GetIcon();
            txt_Price.text = shelfItem.itemPrice.ToString();
            txt_Recipe.text = shelfItem.name;

            ctrl_affordable.SetSelectedBool(Record.current.money > shelfItem.itemPrice);
            ctrl_soldOut.SetSelectedBool(shelfItem.item.count == 0);
        }
    }
}