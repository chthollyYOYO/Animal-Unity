using Framework;
using UnityEngine;

namespace GameLogic
{
    public class TransferBox : MonoBehaviour
    {
        private SceneFlag sceneFlag;

        private void Start()
        {
            sceneFlag = gameObject.GetComponent<SceneFlag>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Record.current.IsCompletedDialogue(1002))
                return;

            switch (sceneFlag.flagType)
            {
                case SceneFlagType.街道0_街道1_传送出发区域:
                    PlayerController.Instance.MoveStreet0ToStreet1();
                    break;

                case SceneFlagType.街道1_街道0_传送出发区域:
                    PlayerController.Instance.MoveStreet1ToStreet0();
                    break;
            }
        }
    }
}
