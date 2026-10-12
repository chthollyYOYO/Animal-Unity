/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_ProgressBar_Run : GProgressBar
    {
        public GProgressBar prog;
        public const string URL = "ui://jnnt3xizvu0s2o";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "ProgressBar_Run";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_ProgressBar_Run));
            UIRegisterInfoManager.Instance.Register<UI_ProgressBar_Run>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            prog = (GProgressBar)GetChild("prog");
        }
    }
}