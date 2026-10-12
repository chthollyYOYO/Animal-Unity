/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_DialogueOption : GComponent
    {
        public GList list_option;
        public const string URL = "ui://g0c90zefq9tg2y";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_DialogueOption";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_DialogueOption));
            UIRegisterInfoManager.Instance.Register<UI_Panel_DialogueOption>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_option = (GList)GetChild("list_option");
        }
    }
}