/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Caption : GComponent
    {
        public GTextField txt_content;
        public GButton btn_next;
        public Transition anim_show;
        public Transition anim_hide;
        public const string URL = "ui://g0c90zefhsf81";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_Caption";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Caption));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Caption>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            txt_content = (GTextField)GetChild("txt_content");
            btn_next = (GButton)GetChild("btn_next");
            anim_show = GetTransition("anim_show");
            anim_hide = GetTransition("anim_hide");
        }
    }
}