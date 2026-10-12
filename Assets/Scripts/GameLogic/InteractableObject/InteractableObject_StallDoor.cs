using UnityEngine;
namespace GameLogic
{
    [RequireComponent(typeof(SceneFlag))]

    public class InteractableObject_StallDoor : InteractableObject// IInteractableObject_Box
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
            return GuideSystem.Instance.guideId == 0;
        }

        public override void OnInteract()
        {
            base.OnInteract();
            PlayerController.Instance.MoveToShopScene();
        }
    }
}