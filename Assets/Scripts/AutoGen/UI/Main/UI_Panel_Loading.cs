/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Loading : GComponent
    {
        public GProgressBar prog;
        public const string URL = "ui://jnnt3xiz8vg6g";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Panel_Loading";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Loading));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Loading>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            prog = (GProgressBar)GetChild("prog");
        }
    }
}