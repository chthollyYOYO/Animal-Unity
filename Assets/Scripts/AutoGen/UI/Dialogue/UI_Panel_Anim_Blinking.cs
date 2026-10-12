/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Anim_Blinking : GComponent
    {
        public GLoader lod;
        public GButton btn_next;
        public Transition anim;
        public const string URL = "ui://g0c90zefoqil31";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_Anim_Blinking";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Anim_Blinking));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Anim_Blinking>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod = (GLoader)GetChild("lod");
            btn_next = (GButton)GetChild("btn_next");
            anim = GetTransition("anim");
        }
    }
}