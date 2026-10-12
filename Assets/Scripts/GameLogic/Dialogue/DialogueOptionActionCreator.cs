using cfg;
using Framework;

namespace GameLogic
{
    public static class DialogueOptionActionCreator
    {
        public static DialogueOptionAction Create(DialogueCfg cfg)
        {
            switch (cfg.Action)
            {
                case DialogueActionType.option:
                    return null;

                case DialogueActionType.option_ui:
                    switch (cfg.Param1)
                    {
                        case "Panel_Recipe":
                            return new DialogueOptionAction_BuyRecipe(cfg);

                        case "Panel_BuyVegetable":
                            return new DialogueOptionAction_BuyVegetable(cfg);

                        default:
                            LogManager.Error($"未处理的参数：{cfg.Action} {cfg.Param1}");
                            return null;
                    }

                case DialogueActionType.option_gift:
                    return new DialogueOptionAction_Gift(cfg);

                default:
                    LogManager.Error($"未处理的参数：{cfg.Action}");
                    return null;
            }
        }
    }
}
