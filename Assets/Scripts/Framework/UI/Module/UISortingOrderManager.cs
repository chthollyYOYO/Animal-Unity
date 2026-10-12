using FairyGUI;
using UI;

namespace Framework
{
    public class UISortingOrderManager : Singleton<UISortingOrderManager>
    {
        public void Initialize()
        {
            UIManager.Instance.OnPanelShow += OnPanelShow;
        }

        private void OnPanelShow(GComponent com)
        {
            var parent = com.parent;
            if (parent != null)
            {
                int targetOrder = GetSortingOrder(com);
                for (int i = 0; i < parent.numChildren; i++)
                {
                    var obj = parent.GetChildAt(i);
                    var order = GetSortingOrder(obj);
                    if (order > targetOrder)
                    {
                        com.RemoveFromParent();
                        parent.AddChildAt(com, i);
                        return;
                    }
                }
            }
        }

        private int GetSortingOrder(GObject obj)
        {
            int order = 0;
            if (obj is ISortable iSortable)
                order = iSortable.SortingOrder;
            return order;
        }
    }
}
