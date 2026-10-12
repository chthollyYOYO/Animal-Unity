/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Ulty : GComponent
    {
        public GImage Ulty;
        public Transition anim_show;
        public const string URL = "ui://ywqkltialynmx3s";
        public const string PKG_NAME = "Stall";
        public const string RES_NAME = "Panel_Ulty";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Ulty));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Ulty>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            Ulty = (GImage)GetChild("Ulty");
            anim_show = GetTransition("anim_show");
        }
    }
}