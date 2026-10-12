using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public partial class StallSystem
    {
        public Dictionary<int, Diner> diners = new();

        private int nextDinerId;
        private float dinerCreatorTimer;

        public void InitDiners()
        {
            nextDinerId = 1;
            ResetDinerCreatorTimer();
        }

        public void OnLogicUpdate_DinerCreator(float deltaTime)
        {
            if (liningDinerIds.Count < liningMax)
            {
                dinerCreatorTimer -= deltaTime;
                if (dinerCreatorTimer <= 0)
                {
                    int dinerId = nextDinerId++;
                    CreateDiner(dinerId);
                    ResetDinerCreatorTimer();
                }
            }
        }

        public Diner CreateDiner(int dinerId, int roleId = 307)
        {
            var diner = Diner.Create(dinerId, roleId: roleId);
            diners.Add(dinerId, diner);
            return diner;
        }

        public void ForceDinerLeave()
        {
            var dinerIds = diners.Keys.ToList();
            foreach (var dinerId in dinerIds)
            {
                diners[dinerId].OnStallStop();
            }
        }

        public void OnDinerDestroy(int dinerId)
        {
            diners.Remove(dinerId);
        }

        public Diner GetDinerByTableId(int tableId)
        {
            if (tables.TryGetValue(tableId, out var table))
            {
                int dinerId = table.dinerId;
                diners.TryGetValue(dinerId, out var diner);
                return diner;
            }
            return null;
        }

        private void ResetDinerCreatorTimer()
        {
            dinerCreatorTimer = UnityEngine.Random.Range(3, 5);
        }
    }
}
