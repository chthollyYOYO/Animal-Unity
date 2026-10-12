/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_QTE : GComponent
    {
        public Controller ctrl_direction;
        public Controller ctrl_style;
        public const string URL = "ui://jnnt3xizhcuo2d";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_QTE";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_QTE));
            UIRegisterInfoManager.Instance.Register<UI_Component_QTE>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_direction = GetController("ctrl_direction");
            ctrl_style = GetController("ctrl_style");
        }
    }
}