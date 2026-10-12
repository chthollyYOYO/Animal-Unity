/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_TableManual : GComponent
    {
        public Controller state_choice;
        public GImage Background;
        public GList list_item;
        public const string URL = "ui://jnnt3xizx8a62z";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_TableManual";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_TableManual));
            UIRegisterInfoManager.Instance.Register<UI_Component_TableManual>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            state_choice = GetController("state_choice");
            Background = (GImage)GetChild("Background");
            list_item = (GList)GetChild("list_item");
        }
    }
}