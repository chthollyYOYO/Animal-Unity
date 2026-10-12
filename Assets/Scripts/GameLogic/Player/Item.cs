using cfg;
using Framework;

namespace GameLogic
{
    public class Item
    {
        public int id;
        public int count;
        public int quality;

        public static Item Create(int id, int count)
        {
            Item item = new();
            item.id = id;
            item.count = count;
            return item;
        }

        public ItemCfg GetCfg()
        {
            return ConfigManager.GetItemCfg(id);
        }

        public string GetIcon()
        {
            return GetCfg()?.Icon;
        }

        public string GetName()
        {
            return GetCfg()?.Name;
        }

        public string GetDesc()
        {
            return GetCfg()?.Desc;
        }

        public int GetItemType()
        {
            var cfg = GetCfg();
            return cfg == null ? 0 : cfg.Type;
        }

        public int GetCookingTime()
        {
            var cfg = GetCfg();
            return cfg == null ? 0 : cfg.CookingTime;
        }

        public Item Clone()
        {
            return MemberwiseClone() as Item;
        }

        public bool Stackable(Item item)
        {
            return item.id == id;
        }
    }
}
