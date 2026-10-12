/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Record : GComponent
    {
        public Controller ctrl_style;
        public GList list_record;
        public UI_Component_OpLongPress com_opL;
        public const string URL = "ui://jnnt3xizgr001b";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Record";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Record));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Record>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_style = GetController("ctrl_style");
            list_record = (GList)GetChild("list_record");
            com_opL = (UI_Component_OpLongPress)GetChild("com_opL");
        }
    }
}