using UnityEngine;
using Sirenix.OdinInspector;

namespace GameLogic
{
    public class CameraController : MonoBehaviour
    {
        [LabelText("玩家标签")]
        public string playerTag = "Player";

        [LabelText("相机偏移量")]
        public Vector3 offset = new Vector3(0, 0, -10);

        [LabelText("边界碰撞器")]
        public BoxCollider2D boundaryCollider;

        private Transform playerTransform;
        private Bounds bounds;

        private void Start()
        {
            FindPlayer();

            if (boundaryCollider != null)
            {
                bounds = boundaryCollider.bounds;
            }
        }

        private void LateUpdate()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return;
            }

            UpdateCameraPosition();
        }

        /// <summary>
        /// 通过标签寻找玩家
        /// </summary>
        private void FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        /// <summary>
        /// 更新相机位置
        /// </summary>
        private void UpdateCameraPosition()
        {
            Vector3 targetPosition = playerTransform.position + offset;
            targetPosition.z = offset.z;

            if (boundaryCollider != null)
            {
                bounds = boundaryCollider.bounds;
                targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
                targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);
            }

            transform.position = targetPosition;
        }
    }
}