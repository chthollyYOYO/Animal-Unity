using cfg;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;

namespace GameLogic
{
    public struct DialogueOptionContext
    {
        public List<string> contents;
        public Action<int> onChoose;
    }

    public class DialogueOption
    {
        public List<DialogueCfg> cfgs = new();

        public int chooseIndex = -1;
        public bool isCompleted = false;

        public void AddCfg(DialogueCfg cfg)
        {
            cfgs.Add(cfg);
        }

        public void Execute()
        {
            DialogueOptionContext context = new()
            {
                contents = cfgs.Select(it => it.Content).ToList(),
                onChoose = index =>
                {
                    chooseIndex = index;
                    isCompleted = true;
                }
            };
            UIManager.Instance.ShowPanelSync<UI_Panel_DialogueOption, DialogueOptionContext>(context);
        }

        public void Stop()
        {
            UIManager.Instance.HidePanel<UI_Panel_DialogueOption>();
        }

        public static bool IsOption(DialogueCfg cfg)
        {
            return cfg.Action
                is DialogueActionType.option
                or DialogueActionType.option_ui
                or DialogueActionType.option_gift
                ;
        }
    }
}
