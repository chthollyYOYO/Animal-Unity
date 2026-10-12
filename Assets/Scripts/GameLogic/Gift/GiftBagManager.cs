using System;
using Framework;
using System.Collections.Generic;

namespace GameLogic
{
    public class GiftBagManager : Singleton<GiftBagManager>
    {
        public List<Item> gifts => Record.current.giftBagItems;

        public event Action giftNumChanged;
        public void SendGiftById(int id)
        {
            foreach (var item in gifts)
            {
                if (item.id == id)
                {
                    SendGift(item);
                    break;
                }
            }
        }
        public void SendGiftByIndex(int index)
        {
            var item = gifts[index];
            SendGift(item);
        }
        private void SendGift(Item item)
        {
            gifts.Remove(item);
            giftNumChanged?.Invoke();
        }
    }
}