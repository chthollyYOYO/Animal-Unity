using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace GameLogic
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        // Start is called before the first frame update
        public Camera StreetCamera;
        public Camera StreetCamera1;
        public Camera StallCamera;
        public Camera RythmCamera;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            RythmCamera.transform.position = new(0, 0.54f, -10f);
            RythmCamera.orthographicSize = 8.86f;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }

}
