using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public partial class StallSystem
    {
        public event Action onLiningChanged;
        public int liningMax
        {
            get => Mathf.Max(_liningMax, PointManager.Instance.GetMaxId(PointType.排队点));
            set => _liningMax = value;
        }
        private int _liningMax = 4;

        //排队顾客索引
        private List<int> liningDinerIds = new();

        public void Enqueue(int dinerId)
        {
            liningDinerIds.Add(dinerId);
            onLiningChanged?.Invoke();
        }

        public int Dequeue()
        {
            int dinerId = liningDinerIds[0];
            liningDinerIds.RemoveAt(0);
            onLiningChanged?.Invoke();
            return dinerId;
        }

        public bool IsPeekReady()
        {
            if (liningDinerIds.Count == 0)
                return false;

            int dinerId = liningDinerIds[0];
            var diner = diners[dinerId];
            return diner.currAction == DinerAction.候座;
        }

        public void ClearLining()
        {
            liningDinerIds.Clear();
            onLiningChanged?.Invoke();
        }

        public int GetLiningIndex(int dinerId)
        {
            return liningDinerIds.FindIndex(it => it == dinerId);
        }

        public void NotifyPeekReady()
        {
            onLiningChanged?.Invoke();
        }
    }
}
