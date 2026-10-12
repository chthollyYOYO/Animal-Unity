/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Dialogue : GComponent
    {
        public Controller ctrl_showName;
        public GLoader lod_role;
        public GTextField txt_name;
        public GTextField txt_content;
        public GButton btn_next;
        public const string URL = "ui://g0c90zefs27k2t";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_Dialogue";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Dialogue));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Dialogue>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_showName = GetController("ctrl_showName");
            lod_role = (GLoader)GetChild("lod_role");
            txt_name = (GTextField)GetChild("txt_name");
            txt_content = (GTextField)GetChild("txt_content");
            btn_next = (GButton)GetChild("btn_next");
        }
    }
}