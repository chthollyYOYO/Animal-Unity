//using FairyGUI;
//using Framework;
//using UnityEngine;

//namespace GameLogic
//{
//    public partial class GameProcess
//    {
//        private static int screenWidth;
//        private static int screenHeight;
//        private static GGraph graph1;
//        private static GGraph graph2;

//        public static void InitializeScreen()
//        {
//            screenWidth = Screen.width;
//            screenHeight = Screen.height;
//            graph1 = new();
//            graph1.SetSize(100, 100);
//            graph1.DrawRect(100, 100, 0, Color.black, Color.black);
//            GRoot.inst.AddChildAt(graph1, 0);
//            graph2 = new();
//            graph2.SetSize(100, 100);
//            graph2.DrawRect(100, 100, 0, Color.black, Color.black);
//            GRoot.inst.AddChildAt(graph2, 1);

//            UpdateMainCamera();
//            UpdateGRoot();
//            UpdateGraph();

//            EventCenter.Instance.AddEventListener("OnMainCameraChanged", UpdateMainCamera);
//            GameMonoSingleton.Instance.OnUpdate += OnUpdate;
//        }

//        private static void OnUpdate(float _)
//        {
//            if (Screen.width != screenWidth || Screen.height != screenHeight)
//            {
//                screenWidth = Screen.width;
//                screenHeight = Screen.height;
//                UpdateMainCamera();
//                UpdateGRoot();
//                UpdateGraph();
//            }
//        }

//        private static void UpdateMainCamera()
//        {
//            if (Camera.main != null && Camera.main != StageCamera.main)
//            {
//                if (Screen.width * 16 < Screen.height * 9)
//                    Camera.main.orthographicSize = (float)Screen.height / Screen.width * 5.4f;
//                else
//                    Camera.main.orthographicSize = 9.6f;
//            }
//        }

//        private static void UpdateGRoot()
//        {
//            GRoot.inst.xy = (new Vector2(Screen.width, Screen.height) - new Vector2(1440, 2560) * UIContentScaler.scaleFactor) / 2f;
//        }

//        private static void UpdateGraph()
//        {
//            if (Screen.width * 16 == Screen.height * 9)
//            {
//                graph1.visible = false;
//                graph2.visible = false;
//            }
//            else
//            {
//                graph1.visible = true;
//                graph2.visible = true;
//                if (Screen.width * 16 < Screen.height * 9)
//                {
//                    graph1.width = graph2.width = 1440;
//                    graph1.x = graph2.x = 0;
//                    graph1.height = graph2.height = (Screen.height / UIContentScaler.scaleFactor - 2560) / 2;
//                    graph1.y = -graph1.height;
//                    graph2.y = 2560;
//                }
//                else
//                {
//                    graph1.height = graph2.height = 2560;
//                    graph1.y = graph2.y = 0;
//                    graph1.width = graph2.width = (Screen.width / UIContentScaler.scaleFactor - 1440) / 2;
//                    graph1.x = -graph1.width;
//                    graph2.x = 1440;
//                }
//            }
//        }
//    }
//}
