using GameLogic;

namespace UI
{
    public partial class UI_Button_KitchenwareItem
    {
        public void Update(Item item)
        {
            lod_icon.url = item?.GetIcon();
        }
    }
}
