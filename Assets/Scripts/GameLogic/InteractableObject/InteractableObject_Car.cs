using Framework;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(SceneFlag))]

    public class InteractableObject_Car : InteractableObject
    {
        public SceneFlag sceneFlag;
        public SceneFlagType flagType => sceneFlag.flagType;

        protected override void Start()
        {
            base.Start();
            sceneFlag = GetComponent<SceneFlag>();
        }

        public override bool IsInteractable()
        {
            return Record.current.IsCompletedDialogue(1004);
        }

        public override void OnInteract()
        {
            base.OnInteract();
            PlayerController.Instance.MoveStreet0ToStreet1();
            PlayerController.Instance.MoveToStallScene();
        }
    }
}