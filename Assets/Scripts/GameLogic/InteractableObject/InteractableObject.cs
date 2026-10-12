using FairyGUI;
using Framework;
using UnityEngine;

namespace GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractableObject : MonoBehaviour
    {
        private static Material outlineMaterial;

        private SpriteRenderer spriteRenderer;
        private Material originalMaterial;

        public bool Selecting;

        public Transform point;

        protected virtual void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
                originalMaterial = spriteRenderer.material;

            if (outlineMaterial == null)
                outlineMaterial = ResourceManager.LoadAssetSync<Material>("Material/Sprite-Unlit-Inline");

            point = transform.Find("InteractPoint");

            if (point == null)
                point = transform;
        }

        protected virtual void OnEnable()
        {
            InteractableObjectManager.Instance.RegisterObject(this);
        }

        protected virtual void OnDisable()
        {
            InteractableObjectManager.Instance.UnregisterObject(this);
        }

        public virtual void OnSelected()
        {
            InteractableOutlineProxy outlineProxy =
                GetComponentInChildren<InteractableOutlineProxy>(true);

            if (outlineProxy != null)
                outlineProxy.SetOutline(true);

            SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
            int selfOrder = selfRenderer != null ? selfRenderer.sortingOrder : 0;

            OutlineOccluderProxy[] allOccluders =
                FindObjectsOfType<OutlineOccluderProxy>(true);

            foreach (OutlineOccluderProxy occ in allOccluders)
            {
                if (occ == null)
                    continue;

                if (occ.GetComponentInParent<InteractableObject>() == this)
                {
                    occ.SetOccluderActive(false);
                    continue;
                }
                bool isPlayerOccluder =
    occ.GetComponentInParent<PlayerController>() != null;

                if (isPlayerOccluder)
                {
                    occ.SetOccluderActive(true);
                    continue;
                }
                bool inFrontByOrder =
                    occ.SourceSortingOrder > selfOrder;

                bool sameOrderButLowerY =
                    occ.SourceSortingOrder == selfOrder &&
                    occ.SourcePosition.y < transform.position.y;

                bool shouldOcclude =
                    inFrontByOrder || sameOrderButLowerY;

                occ.SetOccluderActive(shouldOcclude);
            }

            Selecting = true;
        }

        public virtual void OnCancelSelected()
        {
            InteractableOutlineProxy outlineProxy =
                GetComponentInChildren<InteractableOutlineProxy>(true);

            if (outlineProxy != null)
                outlineProxy.SetOutline(false);

            OutlineOccluderProxy[] allOccluders =
                FindObjectsOfType<OutlineOccluderProxy>(true);

            foreach (OutlineOccluderProxy occ in allOccluders)
            {
                if (occ != null)
                    occ.SetOccluderActive(true);
            }

            Selecting = false;
        }

        public Vector2Int GetGridPosition()
        {
            return Vector2Int.FloorToInt(transform.position);
        }

        public virtual void OnInteract()
        {
            AudioManager.Instance.PlaySound("Music/jiaohu/dianji");
        }

        public virtual void OnLongPress(float deltaTime)
        {

        }

        public virtual void OnLongPressEnd()
        {

        }

        public virtual bool IsInteractable()
        {
            return true;
        }
    }
}