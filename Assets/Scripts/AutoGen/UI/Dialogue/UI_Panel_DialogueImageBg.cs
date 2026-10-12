/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_DialogueImageBg : GComponent
    {
        public GLoader lod;
        public const string URL = "ui://g0c90zefoqil2z";
        public const string PKG_NAME = "Dialogue";
        public const string RES_NAME = "Panel_DialogueImageBg";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_DialogueImageBg));
            UIRegisterInfoManager.Instance.Register<UI_Panel_DialogueImageBg>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            lod = (GLoader)GetChild("lod");
        }
    }
}