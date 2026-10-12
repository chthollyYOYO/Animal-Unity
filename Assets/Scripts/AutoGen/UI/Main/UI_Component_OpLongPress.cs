/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_OpLongPress : GComponent
    {
        public Controller ctrl_name;
        public GProgressBar prog;
        public const string URL = "ui://jnnt3xizgr002a";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_OpLongPress";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_OpLongPress));
            UIRegisterInfoManager.Instance.Register<UI_Component_OpLongPress>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            ctrl_name = GetController("ctrl_name");
            prog = (GProgressBar)GetChild("prog");
        }
    }
}