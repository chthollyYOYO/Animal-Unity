using GameLogic;

namespace UI
{
    public partial class UI_Button_GiftBagItems
    {
        public void Update(Item item)
        {
            lod_item.url = item?.GetIcon();
        }
    }
}