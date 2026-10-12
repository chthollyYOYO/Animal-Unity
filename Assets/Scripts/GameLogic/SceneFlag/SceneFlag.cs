using UnityEngine;

namespace GameLogic
{
    public enum SceneFlagType
    {
        None = 0,
        菜板 = 1,
        炒锅 = 2,
        炖锅 = 3,
        烤架 = 4,
        蒸锅 = 5,
        冰箱 = 6,
        饮料柜 = 7,
        保温箱 = 8,
        垃圾桶 = 9,
        餐桌 = 10,
        收银台 = 11,
        房车_传送 = 12,
        摊位_传送 = 13,
        房车_传送点 = 14,
        摊位_传送点 = 15,
        菜店 = 16,
        书店 = 17,
        街道0_街道1_传送出发区域 = 18,
        街道0_街道1_传送到达点 = 19,
        街道1_街道0_传送出发区域 = 20,
        街道1_街道0_传送到达点 = 21,
        NPC = 22,
    }

    public class SceneFlag : MonoBehaviour
    {
        public SceneFlagType flagType;
        public int id;

        private void Start()
        {
            switch (flagType)
            {
                case SceneFlagType.菜板:
                case SceneFlagType.炒锅:
                case SceneFlagType.炖锅:
                case SceneFlagType.烤架:
                case SceneFlagType.蒸锅:
                    gameObject.AddComponent<InteractableObject_Kitchenware>();
                    break;

                case SceneFlagType.冰箱:
                    gameObject.AddComponent<InteractableObject_IceBox>();
                    break;

                case SceneFlagType.饮料柜:
                    gameObject.AddComponent<InteractableObject_DrinkBox>();
                    break;

                case SceneFlagType.保温箱:
                    gameObject.AddComponent<InteractableObject_WarmBox>();
                    break;

                case SceneFlagType.垃圾桶:
                    gameObject.AddComponent<InteractableObject_Trashcan>();
                    break;

                case SceneFlagType.收银台:
                    gameObject.AddComponent<InteractableObject_Till>();
                    break;

                case SceneFlagType.餐桌:
                    gameObject.AddComponent<InteractableObject_Table>();
                    break;

                case SceneFlagType.房车_传送:
                    gameObject.AddComponent<InteractableObject_Car>();
                    break;

                case SceneFlagType.摊位_传送:
                    gameObject.AddComponent<InteractableObject_StallDoor>();
                    break;

                case SceneFlagType.书店:
                case SceneFlagType.菜店:
                    gameObject.AddComponent<InteractableObject_Shop>();
                    break;

                case SceneFlagType.街道0_街道1_传送出发区域:
                case SceneFlagType.街道1_街道0_传送出发区域:
                    gameObject.AddComponent<TransferBox>();
                    break;

                case SceneFlagType.NPC:
                    gameObject.AddComponent<InteractableObject_Npc>();
                    break;
            }
        }

        private void OnEnable()
        {
            SceneFlagManager.Instance.RegisterFlag(this);
        }

        private void OnDisable()
        {
            SceneFlagManager.Instance.UnregisterFlag(this);
        }
    }
}
