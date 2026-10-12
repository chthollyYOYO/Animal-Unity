using Cysharp.Threading.Tasks;
using FairyGUI;
using GameLogic;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Framework
{
    public partial class Helper
    {
        public static ValueDropdownList<int> GetItemList()
        {
            ConfigManager.Initialize().Forget();
            ValueDropdownList<int> result = new();
            foreach (var cfg in ConfigManager.GetItemCfgList())
                result.Add($"{cfg.Id}_{cfg.Name}", cfg.Id);
            return result;
        }

        public static bool TryAdd_NonStackable(this Item[] items, int id)
        {
            Item item = Item.Create(id, 1);
            return items.TryAdd_NonStackable(item);
        }

        public static bool TryAdd_NonStackable(this Item[] items, Item item)
        {
            if (item == null)
                return false;

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item.Clone();
                    items[i].count = 1;
                    return true;
                }
            }
            return false;
        }

        public static bool TryAdd_Stackable(this Item[] items, int id, int count)
        {
            Item item = Item.Create(id, count);
            return items.TryAdd_Stackable(item);
        }

        public static bool TryAdd_Stackable(this Item[] items, Item item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    return true;
                }

                if (items[i].Stackable(item))
                {
                    items[i].count += item.count;
                    return true;
                }
            }
            return false;
        }

        public static Item RemoveOne(this Item[] items, int index)
        {
            var item = items[index];
            if (item.count == 1)
            {
                items[index] = null;
            }
            else
            {
                items[index].count--;

                item = item.Clone();
                item.count = 1;
            }
            return item;
        }

        public static List<Item> RemoveAll(this Item[] items)
        {
            List<Item> result = new();
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item != null)
                {
                    result.Add(item);
                    items[i] = null;
                }
            }
            return result;
        }

        public static int GetNearestIndex(this Item[] items, int index)
        {
            for (int i = index; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    return i;
                }
            }

            for (int i = index - 1; i >= 0; i--)
            {
                if (items[i] != null)
                {
                    return i;
                }
            }

            return -1;
        }

        public static GComponent GetNearestItem(this GList list, Item[] items, int index)
        {
            int rIndex = items.GetNearestIndex(index);
            if (rIndex >= 0)
                return list.GetChildAt(rIndex).asCom;
            return null;
        }

        public static void SetItemIcon(this GLoader loader, int itemId)
        {
            var cfg = ConfigManager.GetItemCfg(itemId, false);
            loader.url = cfg?.Icon;
        }
    }
}
