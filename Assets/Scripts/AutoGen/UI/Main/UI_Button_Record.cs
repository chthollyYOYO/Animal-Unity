/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Button_Record : GButton
    {
        public Controller ctrl_hasRecord;
        public GTextField txt_chapter;
        public GTextField txt_info;
        public const string URL = "ui://jnnt3xizgr0028";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Button_Record";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Button_Record));
            UIRegisterInfoManager.Instance.Register<UI_Button_Record>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_hasRecord = GetController("ctrl_hasRecord");
            txt_chapter = (GTextField)GetChild("txt_chapter");
            txt_info = (GTextField)GetChild("txt_info");
        }
    }
}