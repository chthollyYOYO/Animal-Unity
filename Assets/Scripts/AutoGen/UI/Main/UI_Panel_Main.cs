/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Main : GComponent
    {
        public Controller ctrl_interactStyle;
        public Controller ctrl_showBag;
        public GImage img_time;
        public GTextField txt_money;
        public GTextField txt_date;
        public GList list_bag;
        public UI_Component_OpClick com_opJ;
        public UI_Component_OpLongPress com_opLongPress;
        public const string URL = "ui://jnnt3xizgr0011";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Main";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Main));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Main>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_interactStyle = GetController("ctrl_interactStyle");
            ctrl_showBag = GetController("ctrl_showBag");
            img_time = (GImage)GetChild("img_time");
            txt_money = (GTextField)GetChild("txt_money");
            txt_date = (GTextField)GetChild("txt_date");
            list_bag = (GList)GetChild("list_bag");
            com_opJ = (UI_Component_OpClick)GetChild("com_opJ");
            com_opLongPress = (UI_Component_OpLongPress)GetChild("com_opLongPress");
        }
    }
}