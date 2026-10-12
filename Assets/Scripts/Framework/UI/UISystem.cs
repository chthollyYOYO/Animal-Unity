using Cysharp.Threading.Tasks;
using FairyGUI;
using System.Collections.Generic;

namespace Framework
{
    public class UISystem : Singleton<UISystem>
    {
        /// <summary>
        /// UI资源根目录
        /// </summary>
        public const string UI_ROOT = "Assets/Resources/UI";

        /// <summary>
        /// UI面板命名空间
        /// </summary>
        public const string UI_GCOMPONENT_NAMESPACE = "UI";

        /// <summary>
        /// UI面板注册方法名
        /// </summary>
        public const string UI_GCOMPONENT_REGISTER_FUNC = "Register_UI";

        /// <summary>
        /// UI设计宽度
        /// </summary>
        public const int UI_DESIGN_WIDTH = 2560;

        /// <summary>
        /// UI设计高度
        /// </summary>
        public const int UI_DESIGN_HEIGHT = 1440;

        public List<string> commonPackageNames = new();

        public async UniTask Initialize()
        {
            //UI注册
            UIRegisterInfoManager.Instance.RegisterAll();

            //UI核心模块初始化
            UICamera.Instance.Initialize();
            UIStageController.Instance.Initialize();
            //await UIFontManager.Instance.Initialize();

            //UI功能模块初始化
            UIFullScreenPanelManager.Instance.Initialize();
            UISortingOrderManager.Instance.Initialize();
            UIMainPanelManager.Instance.Initialize();
            UIBlackScreen.Instance.Initialize();

            //加载通用包
            foreach (var packageName in commonPackageNames)
                await UIResourceManager.Instance.LoadByPkgName(packageName);
        }

        public void AddCommonPackage(string packageName)
        {
            commonPackageNames.Add(packageName);
        }
    }
}
