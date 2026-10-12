using UnityEngine;
using UnityEngine.Rendering;

public class OutlineOccluderProxy : MonoBehaviour
{
    public GameObject source;
    public GameObject proxy;
    public Material maskMaterial;
    public int occluderLayer;

    private SpriteRenderer sourceRenderer;
    private SpriteRenderer proxyRenderer;
    public Transform sortPoint;
    private SortingGroup sortingGroup;
    private bool occluderEnabled = true;

    public int SourceSortingOrder
    {
        get
        {
            if (source != null)
            {
                if (sortingGroup == null)
                    sortingGroup = source.GetComponentInParent<SortingGroup>();

                if (sortingGroup != null)
                    return sortingGroup.sortingOrder;
            }

            if (sourceRenderer == null && source != null)
                sourceRenderer = source.GetComponent<SpriteRenderer>();

            return sourceRenderer != null ? sourceRenderer.sortingOrder : 0;
        }
    }

    public Vector3 SourcePosition
    {
        get
        {
            if (sortPoint != null)
                return sortPoint.position;

            if (source != null)
                return source.transform.position;

            return transform.position;
        }
    }

    public void CreateProxy()
    {
        if (source == null)
            source = gameObject;

        if (proxy != null)
            return;

        sourceRenderer = source.GetComponent<SpriteRenderer>();

        if (sourceRenderer == null)
            return;

        if (maskMaterial == null)
            maskMaterial = Resources.Load<Material>("Material/M_interactableMask");

        if (occluderLayer <= 0)
            occluderLayer = LayerMask.NameToLayer("OutlineOccluder");

        if (occluderLayer == -1)
            return;

        proxy = new GameObject(source.name + "_OccluderProxy");
        proxy.layer = occluderLayer;

        proxy.transform.SetParent(source.transform.parent, false);

        proxyRenderer = proxy.AddComponent<SpriteRenderer>();
        proxyRenderer.sharedMaterial = maskMaterial;
        proxyRenderer.shadowCastingMode = ShadowCastingMode.Off;
        proxyRenderer.receiveShadows = false;

        SyncProxy();
    }

    private void LateUpdate()
    {
        SyncProxy();
    }

    private void SyncProxy()
    {
        if (source == null || proxy == null)
            return;

        if (sourceRenderer == null)
            sourceRenderer = source.GetComponent<SpriteRenderer>();

        if (proxyRenderer == null)
            proxyRenderer = proxy.GetComponent<SpriteRenderer>();

        if (sourceRenderer == null || proxyRenderer == null)
            return;

        proxy.SetActive(source.activeInHierarchy && occluderEnabled);

        proxy.transform.position = source.transform.position;
        proxy.transform.rotation = source.transform.rotation;
        proxy.transform.localScale = source.transform.lossyScale;

        proxyRenderer.sprite = sourceRenderer.sprite;
        proxyRenderer.flipX = sourceRenderer.flipX;
        proxyRenderer.flipY = sourceRenderer.flipY;

        proxyRenderer.sortingLayerID = sourceRenderer.sortingLayerID;
        proxyRenderer.sortingOrder = sourceRenderer.sortingOrder;

        proxyRenderer.color = Color.white;
        proxyRenderer.sharedMaterial = maskMaterial;
    }

    public void SetOccluderActive(bool active)
    {
        occluderEnabled = active;

        if (proxy != null)
            proxy.SetActive(source != null && source.activeInHierarchy && occluderEnabled);
    }

    private void OnDestroy()
    {
        if (proxy != null)
        {
            if (Application.isPlaying)
                Destroy(proxy);
            else
                DestroyImmediate(proxy);
        }
    }
}