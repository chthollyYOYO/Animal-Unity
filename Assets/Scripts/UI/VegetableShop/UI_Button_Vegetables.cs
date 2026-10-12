using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Button_Vegetables
    {
        public void Update(ShelfItem shelfitem)
        {
            ctrl_priceChange.selectedIndex = 0;
            lod_item.url = shelfitem.item?.GetIcon();
            txt_price.SetVar("Price", shelfitem.itemPrice.ToString()).FlushVars();
            txt_num.SetVar("Count", shelfitem.item.count.ToString()).FlushVars();
            txt_name.text = shelfitem.name;
            if (shelfitem.priceChanged == ShelfItem.PriceChanged.priceUp)
                ctrl_priceChange.SetSelectedPage("Up");
            else if (shelfitem.priceChanged == ShelfItem.PriceChanged.priceDown)
                ctrl_priceChange.SetSelectedPage("Down");

            ctrl_specialItem.SetSelectedBool(shelfitem.limit);
            ctrl_affordable.SetSelectedBool(Record.current.money > shelfitem.itemPrice);
            ctrl_soldOut.SetSelectedBool(shelfitem.item.count == 0);
        }
    }
}
