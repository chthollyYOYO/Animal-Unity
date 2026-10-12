/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Action_Gold : GComponent
    {
        public GImage Gold;
        public Transition Waiting;
        public Transition Get;
        public const string URL = "ui://jnnt3xiz1073x39";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Action_Gold";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Action_Gold));
            UIRegisterInfoManager.Instance.Register<UI_Action_Gold>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            Gold = (GImage)GetChild("Gold");
            Waiting = GetTransition("Waiting");
            Get = GetTransition("Get");
        }
    }
}