/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Anim_CountDown : GComponent
    {
        public Transition CountDown;
        public const string URL = "ui://jnnt3xizmm9rx4w";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Anim_CountDown";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Anim_CountDown));
            UIRegisterInfoManager.Instance.Register<UI_Anim_CountDown>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            CountDown = GetTransition("CountDown");
        }
    }
}