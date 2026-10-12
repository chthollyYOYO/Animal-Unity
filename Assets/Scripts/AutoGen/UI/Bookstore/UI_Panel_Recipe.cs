/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_Recipe : GComponent
    {
        public GList list_recipe;
        public GButton btn_close;
        public const string URL = "ui://zyfv00glsxl1d";
        public const string PKG_NAME = "Bookstore";
        public const string RES_NAME = "Panel_Recipe";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_Recipe));
            UIRegisterInfoManager.Instance.Register<UI_Panel_Recipe>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            list_recipe = (GList)GetChild("list_recipe");
            btn_close = (GButton)GetChild("btn_close");
        }
    }
}