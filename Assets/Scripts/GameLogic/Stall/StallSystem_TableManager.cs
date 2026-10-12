using System;
using System.Collections.Generic;

namespace GameLogic
{
    public partial class StallSystem
    {
        public event Action onTableChanged;
        public Dictionary<int, InteractableObject_Table> tables = new();

        public void InitTables()
        {
            for (int i = 1; i <= 7; i++)
            {
                var table = GetTable(i);
                tables[i] = table;
                table.dinerId = 0;
            }
        }

        public void AssignDinerTable(int tableId)
        {
            int dinerId = Dequeue();
            //先将顾客安排到对应的桌子ID
            diners[dinerId].AssignTable(tableId);
            //给桌子的dictionary添加被安排排队顾客的ID
            tables[tableId].dinerId = dinerId;
            onTableChanged?.Invoke();
        }

        /// <summary>
        /// 桌面已经被清洁完成，将对应ID的桌子空出来可供安排顾客
        /// </summary>
        /// <param name="tableId"></param>
        public void TableClean(int tableId)
        {
            //int mealitemid = diners[tables[tableId]].mealItemId;
            var table = GetTable(tableId);
            money += table.mealsellprice;
            //把桌子状态空出来
            tables[tableId].dinerId = 0;
            //TableEmpty?.Invoke(tableId);
            onTableChanged?.Invoke();
        }

        public bool IsSitDownable(int tableId)
        {
            var table = GetTable(tableId);
            return table.Clean && table.dinerId == 0;
        }

        public InteractableObject_Table GetTable(int tableId)
        {
            var flag = SceneFlagManager.Instance.GetFlag(SceneFlagType.餐桌, tableId);
            var table = flag.GetComponent<InteractableObject_Table>();
            return table;
        }
    }
}
