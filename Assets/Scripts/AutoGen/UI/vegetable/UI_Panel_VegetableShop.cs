/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using Framework;

namespace UI
{
    public partial class UI_Panel_VegetableShop : GComponent
    {
        public GImage image0;
        public GImage image1;
        public GImage image2;
        public GImage NPC;
        public Transition entryAnimation;
        public const string URL = "ui://46wq0b21oitix3u";
        public const string PKG_NAME = "Vegetable";
        public const string RES_NAME = "Panel_VegetableShop";

        public static void Register_UI()
        {
            UIObjectFactory.SetPackageItemExtension(URL, typeof(UI_Panel_VegetableShop));
            UIRegisterInfoManager.Instance.Register<UI_Panel_VegetableShop>(PKG_NAME, RES_NAME);
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            image0 = (GImage)GetChild("image0");
            image1 = (GImage)GetChild("image1");
            image2 = (GImage)GetChild("image2");
            NPC = (GImage)GetChild("NPC");
            entryAnimation = GetTransition("entryAnimation");
        }
    }
}