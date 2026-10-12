/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Anim_Moving : GComponent
    {
        public Transition anim;
        public const string URL = "ui://g0c90zef9hjb2";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_Anim_Moving";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Anim_Moving));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Anim_Moving>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            anim = GetTransition("anim");
        }
    }
}