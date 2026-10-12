using Framework;
using GameLogic;

namespace UI
{
    public partial class UI_Button_BoxItem
    {
        public void Update(int itemId)
        {
            lod_icon.SetItemIcon(itemId);
            ctrl_hasCount.SetSelectedBool(false);
        }

        public void Update(Item item, bool showCount = true)
        {
            if (item != null)
            {
                lod_icon.SetItemIcon(item.id);
                ctrl_hasCount.SetSelectedBool(showCount);
                if (showCount)
                {
                    txt_count.text = item.count.ToString();
                }
            }
            else
            {
                lod_icon.url = null;
                ctrl_hasCount.SetSelectedBool(false);
            }
        }
    }
}
