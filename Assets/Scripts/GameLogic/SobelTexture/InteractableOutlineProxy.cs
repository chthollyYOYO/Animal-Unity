using UnityEngine;
using static Unity.VisualScripting.Member;

public class InteractableOutlineProxy : MonoBehaviour
{
    private GameObject proxyObject;
    private SpriteRenderer proxyRenderer;
    private SpriteRenderer sourceRenderer;

    private int interactableLayer;

    private void Awake()
    {
        sourceRenderer = GetComponent<SpriteRenderer>();
        interactableLayer = LayerMask.NameToLayer("InteractableObject");

        CreateProxy();
    }

    public void CreateProxy()
    {
        if (proxyObject != null)
            return;

        sourceRenderer = GetComponent<SpriteRenderer>();

        if (sourceRenderer == null)
            return;

        proxyObject = new GameObject(name + "_OutlineProxy");
        proxyObject.transform.SetParent(transform.parent, false);
        if (interactableLayer == -1)
            interactableLayer = LayerMask.NameToLayer("InteractableObject");

        proxyObject.layer = interactableLayer;

        proxyRenderer = proxyObject.AddComponent<SpriteRenderer>();
        proxyRenderer.enabled = false;

        SyncProxy();
    }

    private void LateUpdate()
    {
        SyncProxy();
    }

    private void SyncProxy()
    {
        if (proxyObject == null || proxyRenderer == null || sourceRenderer == null)
            return;

        proxyObject.transform.position = transform.position;
        proxyObject.transform.rotation = transform.rotation;
        proxyObject.transform.localScale = transform.lossyScale;

        proxyRenderer.sprite = sourceRenderer.sprite;
        proxyRenderer.flipX = sourceRenderer.flipX;
        proxyRenderer.flipY = sourceRenderer.flipY;
        proxyRenderer.color = Color.white;

        proxyRenderer.sortingLayerID = sourceRenderer.sortingLayerID;
        proxyRenderer.sortingOrder = sourceRenderer.sortingOrder;
    }

    public void SetOutline(bool enable)
    {
        if (proxyRenderer != null)
        {
            proxyRenderer.enabled = enable;
        }
    }

    private void OnDestroy()
    {
        if (proxyObject != null)
        {
            if (Application.isPlaying)
                Destroy(proxyObject);
            else
                DestroyImmediate(proxyObject);
        }
    }
}