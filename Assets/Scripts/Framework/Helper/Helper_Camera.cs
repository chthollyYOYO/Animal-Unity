using System;
using UnityEngine;

namespace Framework
{
    public static partial class Helper
    {
        public static void ChangeMainCamera(Camera camera)
        {
            var main = Camera.main;
            if (main != null)
            {
                main.gameObject.SetActive(false);
                main.tag = "Untagged";
                //if (main.GetComponent<AudioListener>() != null)
                //    main.gameObject.RemoveComponent<AudioListener>();
            }
            if (camera != null)
            {
                camera.gameObject.SetActive(true);
                camera.tag = "MainCamera";
                if (camera.GetComponent<AudioListener>() == null)
                    camera.gameObject.AddComponent<AudioListener>();
            }
            EventCenter.Instance.SendEvent("OnMainCameraChanged");
        }

        [Obsolete("Use ChangeMainCamera")]
        public static void SetMainCameraEnable(Camera camera, bool enable)
        {
            if (enable)
            {
                camera.gameObject.SetActive(true);
                camera.tag = "MainCamera";
                if (camera.GetComponent<AudioListener>() == null)
                    camera.gameObject.AddComponent<AudioListener>();
            }
            else
            {
                camera.gameObject.SetActive(false);
                camera.tag = "Untagged";
                //if (camera.GetComponent<AudioListener>() != null)
                //    camera.gameObject.RemoveComponent<AudioListener>();
            }
            EventCenter.Instance.SendEvent("OnMainCameraChanged");
        }
    }
}
