using FairyGUI;
using UnityEngine;

namespace Framework
{
    public class UIBlackScreen : Singleton<UIBlackScreen>
    {
        private GGraph graph;

        public void Initialize()
        {
            graph = new();
            graph.SetSize(1440, 2560);
            graph.DrawRect(1440, 2560, 0, Color.black, Color.black);
            GRoot.inst.AddChildAt(graph, 0);
            SetVisible(false);
        }

        public void SetVisible(bool visible)
        {
            graph.visible = visible;
        }
    }
}
