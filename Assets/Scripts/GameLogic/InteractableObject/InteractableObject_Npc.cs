using Framework;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(SceneFlag))]

    public class InteractableObject_Npc : InteractableObject
    {
        public SceneFlag sceneFlag;

        private GameObject gameObjectWenhao;

        protected override void Start()
        {
            base.Start();
            sceneFlag = GetComponent<SceneFlag>();
            gameObjectWenhao = transform.Find("wenhao").gameObject;
        }

        private void Update()
        {
            bool activeWenhao = !Record.current.IsCompletedDialogue(1002)
                || (Record.current.IsRecipeEnough() && !Record.current.IsCompletedDialogue(1004));
            if (gameObjectWenhao.activeSelf != activeWenhao)
                gameObjectWenhao.SetActive(activeWenhao);
        }

        public override void OnInteract()
        {
            base.OnInteract();
            if (!Record.current.IsCompletedDialogue(1002))
                DialogueSystem.Instance.StartDialogue(1002);
            else if (Record.current.IsRecipeEnough())
                DialogueSystem.Instance.StartDialogue(1004);
            else
                DialogueSystem.Instance.StartDialogue(1005);
        }
    }
}