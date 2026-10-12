/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Component_QTEList : GComponent
    {
        public GList list_qte;
        public const string URL = "ui://jnnt3xizvu0s2n";
        public const string PKG_NAME = "Main";
        public const string RES_NAME = "Component_QTEList";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Component_QTEList));
            UIRegisterInfoManager.Instance.Register<UI_Component_QTEList>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_qte = (GList)GetChild("list_qte");
        }
    }
}