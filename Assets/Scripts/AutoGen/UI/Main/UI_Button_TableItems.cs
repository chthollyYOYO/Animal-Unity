/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_TableItems : GButton
    {
        public Controller ctrl_full;
        public GImage selectedbg;
        public GTextField txt_id;
        public const string URL = "ui://jnnt3xizx8a636";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Button_TableItems";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_TableItems));
            UIRegisterInfoManager.Instance.Register<UI_Button_TableItems>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_full = GetController("ctrl_full");
            selectedbg = (GImage)GetChild("selectedbg");
            txt_id = (GTextField)GetChild("txt_id");
        }
    }
}