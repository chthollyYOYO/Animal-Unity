using Framework;
using UnityEngine;

namespace GameLogic
{
    public class InteractableObject_Shop : InteractableObject
    {
        private GameObject highlight;
        private SceneFlag sceneFlag;

        protected override void Start()
        {
            point = transform.Find("InteractPoint");
            if (point == null)
                point = transform;

            highlight = transform.Find("highlight").gameObject;
            highlight.SetActive(false);

            sceneFlag = GetComponent<SceneFlag>();
        }

        public override void OnSelected()
        {
            highlight.SetActive(true);
            Selecting = true;
        }

        public override void OnCancelSelected()
        {
            highlight.SetActive(false);
            Selecting = false;
        }

        public override void OnInteract()
        {
            base.OnInteract();
            switch (sceneFlag.flagType)
            {
                case SceneFlagType.书店:
                    if (!Record.current.IsCompletedDialogue(1006))
                        DialogueSystem.Instance.StartDialogue(1006);
                    else
                        DialogueSystem.Instance.StartDialogue(10201);
                    break;

                case SceneFlagType.菜店:
                    if (!Record.current.IsCompletedDialogue(1003))
                        DialogueSystem.Instance.StartDialogue(1003);
                    else
                        DialogueSystem.Instance.StartDialogue(10301);
                    break;
            }
        }
    }
}
