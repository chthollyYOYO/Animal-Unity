using cfg;

namespace GameLogic
{
    public static class DialogueActionCreator
    {
        public static DialogueAction Create(DialogueCfg cfg)
        {
            return cfg.Action switch
            {
                DialogueActionType.chat => new DialogueAction_Chat(cfg),
                DialogueActionType.caption => new DialogueAction_Caption(cfg),
                DialogueActionType.sound => new DialogueAction_Sound(cfg),
                DialogueActionType.image => new DialogueAction_Image(cfg),
                DialogueActionType.process => cfg.Param1 switch
                {
                    "blinking" => new DialogueAction_Process_Blinking(cfg),
                    "naming" => new DialogueAction_Process_Naming(cfg),
                    "moving" => new DialogueAction_Process_Moving(cfg),
                    "cooking" => new DialogueAction_Process_Cooking(cfg),
                    "Panel_Bookstore" => new DialogueAction_Process_Panel_Bookstore(cfg),
                    "Panel_VegetableShop" => new DialogueAction_Process_Panel_VegetableShop(cfg),
                    _ => null
                },
                DialogueActionType.enterBlackScreen => new DialogueAction_EnterBlackScreen(cfg),
                DialogueActionType.exitBlackScreen => new DialogueAction_ExitBlackScreen(cfg),
                DialogueActionType.bgm => new DialogueAction_Bgm(cfg),
                DialogueActionType.bgmStop => new DialogueAction_BgmStop(cfg),
                DialogueActionType.image_bg => new DialogueAction_ImageBg(cfg),
                DialogueActionType.image_bgClose => new DialogueAction_ImageBgClose(cfg),
                DialogueActionType.ambience => new DialogueAction_Ambience(cfg),
                DialogueActionType.ambienceStop => new DialogueAction_AmbienceStop(cfg),
                _ => null
            };
        }
    }
}
