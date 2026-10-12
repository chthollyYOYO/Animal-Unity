using FairyGUI;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class UICamera : Singleton<UICamera>
    {
        public Camera camera => StageCamera.main;
        public UniversalAdditionalCameraData urp
        {
            get
            {
                if (camera != null)
                    return camera.GetComponent<UniversalAdditionalCameraData>();
                return null;
            }
        }

        public void Initialize()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            EventCenter.Instance.AddEventListener("OnMainCameraChanged", UpdateCamera);
            Stage.inst.onStageResized.Add(OnStageResized);
            UpdateCamera();
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            UpdateCamera();
        }

        private void OnStageResized()
        {
            UpdateCamera();
        }

        public void UpdateCamera()
        {
            StageCamera.CheckMainCamera();
            UnityEngine.Object.DontDestroyOnLoad(StageCamera.main.gameObject);
            if (urp == null && camera != null)
                camera.gameObject.GetOrAddComponent<UniversalAdditionalCameraData>();

            if (urp != null)
            {
                //urp.SetRenderer(0);
                urp.renderPostProcessing = false;
                urp.renderShadows = false;
            }

            if (Camera.main != null && Camera.main.targetTexture == null)
            {
                var urpMainCamera = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                if (!urpMainCamera.cameraStack.Contains(StageCamera.main))
                    urpMainCamera.cameraStack.Add(StageCamera.main);
                urp.renderType = CameraRenderType.Overlay;
            }
            else
            {
                urp.renderType = CameraRenderType.Base;
            }

            if (Camera.main != null)
            {
                urp.gameObject.RemoveComponent<AudioListener>();
            }
            else
            {
                urp.gameObject.GetOrAddComponent<AudioListener>();
            }

            camera.transform.position = new(camera.orthographicSize * Screen.width / Screen.height + UIStageController.stageOffsetX, -camera.orthographicSize);
        }

        public Vector2 WorldPointToUI(Vector3 worldPoint)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
            screenPoint.y = Screen.height - screenPoint.y;
            Vector2 uiPoint = GRoot.inst.GlobalToLocal(screenPoint);
            return uiPoint;
        }
    }
}
