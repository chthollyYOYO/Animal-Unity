using Cysharp.Threading.Tasks;
using FairyGUI;
using System.Collections.Generic;
using UI;

namespace Framework
{
    public class UIMainPanelManager : Singleton<UIMainPanelManager>
    {
        private string currentKey;
        private readonly List<string> mainPanelStack = new();
        private bool isOperateByStack;

        public void Initialize()
        {
            UIManager.Instance.OnPanelShow += OnPanelShow;
            UIManager.Instance.OnPanelHide += OnPanelHide;
        }

        public void ClearStack()
        {
            mainPanelStack.Clear();
        }

        private void OnPanelShow(GComponent com)
        {
            if (com is IMainPanel)
            {
                if (isOperateByStack)
                {
                    isOperateByStack = false;
                    return;
                }

                var key = Helper.GetUIKey(com);
                if (key == currentKey)
                    return;

                if (mainPanelStack.Contains(key))
                    mainPanelStack.Remove(key);

                if (!string.IsNullOrEmpty(currentKey))
                {
                    isOperateByStack = true;
                    UIManager.Instance.HidePanel(currentKey);
                    mainPanelStack.Insert(0, currentKey);
                }

                currentKey = key;
            }
        }

        private void OnPanelHide(GComponent com)
        {
            if (com is IMainPanel)
            {
                if (isOperateByStack)
                {
                    isOperateByStack = false;
                    return;
                }

                if (mainPanelStack.Count > 0)
                {
                    currentKey = mainPanelStack[0];
                    mainPanelStack.RemoveAt(0);
                    isOperateByStack = true;
                    UIManager.Instance.ShowPanel(currentKey).Forget();
                }
                else
                {
                    currentKey = null;
                }
            }
        }
    }
}
