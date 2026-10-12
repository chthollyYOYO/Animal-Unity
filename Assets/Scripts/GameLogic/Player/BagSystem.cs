using Framework;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameLogic
{
    public class BagSystem : Singleton<BagSystem>
    {
        public event UnityAction onBagChanged;

        public Item[] items => Record.current.bag;
        
        public bool TryAddItem(int itemId)
        {
            bool successed = items.TryAdd_NonStackable(itemId);
            if (successed)
            {
                onBagChanged?.Invoke();
            }
            return successed;
        }

        public bool TryAddItem(Item item)
        {
            bool successed = items.TryAdd_NonStackable(item);
            if (successed)
            {
                onBagChanged?.Invoke();
            }
            return successed;
        }

        /// <returns> -1:全都没放进去 0:放进去了一部分 1:全放进去了</returns>
        public int TryAddItem(IEnumerable<int> itemIds)
        {
            int result = -1;
            foreach (var itemId in itemIds)
            {
                if (items.TryAdd_NonStackable(itemId))
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            if (result >= 0)
            {
                onBagChanged?.Invoke();
            }

            return result;
        }

        /// <returns> -1:全都没放进去 0:放进去了一部分 1:全放进去了</returns>
        public int TryAddItem(IEnumerable<Item> items)
        {
            int result = -1;
            foreach (var itemId in items)
            {
                if (this.items.TryAdd_NonStackable(itemId))
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            if (result >= 0)
            {
                onBagChanged?.Invoke();
            }

            return result;
        }

        public Item RemoveItem(int index)
        {
            var item = items.RemoveOne(index);
            onBagChanged?.Invoke();
            return item;
        }

        public List<Item> RemoveAllItems()
        {
            var items = this.items.RemoveAll();
            onBagChanged?.Invoke();
            return items;
        }

        public Item GetItem(int index)
        {
            return items[index];
        }

        public bool HasItem(int index)
        {
            return GetItem(index) != null;
        }
    }
}
