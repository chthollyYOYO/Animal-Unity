using UnityEngine.Events;

namespace GameLogic
{
    public interface IInteractableObject_Box
    {
        Item[] items { get; }

        UnityAction onItemChanged { get; set; }

        SceneFlagType flagType { get; }

        int GetLength() => items.Length;

        Item GetItem(int index) => items[index]?.Clone();

        Item GetItemOne(int index)
        {
            var item = items[index]?.Clone();
            item.count = 1;
            return item;
        }

        bool TryAddItem(Item item);

        Item RemoveItem(int index);

        bool isShowCount => true;
    }
}
