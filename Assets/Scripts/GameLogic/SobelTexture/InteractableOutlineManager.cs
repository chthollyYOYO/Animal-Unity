using UnityEngine;

[ExecuteAlways]
public class InteractableOutlineManager : MonoBehaviour
{
    public static InteractableOutlineManager Instance { get; private set; }

    [Header("Target Mask Settings")]
    public LayerMask maskLayer;
    public Material maskMaterial;
    public Material compositeMaterial;

    [Header("Occluder Mask Settings")]
    public LayerMask occluderLayer;

    private Camera maskCamera;
    private Camera occluderMaskCamera;
    private Camera currentMainCamera;

    private RenderTexture maskRT;
    private RenderTexture occluderMaskRT;

    private int lastWidth;
    private int lastHeight;

    private MaskCameraMaterialOverride materialOverride;
    private MaskCameraMaterialOverride occluderMaterialOverride;

    private bool proxiesBuilt;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void RuntimeInitialize()
    {
        if (Instance != null)
            return;

        GameObject obj = new GameObject("InteractableOutlineManager");
        DontDestroyOnLoad(obj);
        obj.AddComponent<InteractableOutlineManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);

            return;
        }

        Instance = this;

        if (Application.isPlaying)
            DontDestroyOnLoad(gameObject);

        LoadDefaultAssetsIfNeeded();

        CreateMaskCamera();
        CreateOccluderMaskCamera();

        UpdateRenderTexture();
    }

    private void Start()
    {
        BuildAllProxies();
    }

    private void LateUpdate()
    {
        Camera main = Camera.main;

        if (main == null)
            return;

        if (main != currentMainCamera)
        {
            currentMainCamera = main;
            AttachToMainCamera(main);
        }

        SyncCameraSettings(main);

        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateRenderTexture();
        }

        if (!proxiesBuilt)
        {
            BuildAllProxies();
        }
    }

    private void LoadDefaultAssetsIfNeeded()
    {
        if (maskMaterial == null)
            maskMaterial = Resources.Load<Material>("Material/M_interactableMask");

        if (compositeMaterial == null)
            compositeMaterial = Resources.Load<Material>("Material/M_interactableObjectMaterial");

        if (maskLayer.value == 0)
        {
            int layer = LayerMask.NameToLayer("InteractableObject");
            if (layer != -1)
                maskLayer = 1 << layer;
        }

        if (occluderLayer.value == 0)
        {
            int layer = LayerMask.NameToLayer("OutlineOccluder");
            if (layer != -1)
                occluderLayer = 1 << layer;
        }
    }

    private void BuildAllProxies()
    {
        int interactableLayerIndex = LayerMask.NameToLayer("InteractableObject");
        int occluderLayerIndex = LayerMask.NameToLayer("OutlineOccluder");
        int uiLayerIndex = LayerMask.NameToLayer("UI");

        if (interactableLayerIndex == -1)
        {
            Debug.LogError("没有找到 Layer: InteractableObject");
            return;
        }

        if (occluderLayerIndex == -1)
        {
            Debug.LogError("没有找到 Layer: OutlineOccluder");
            return;
        }

        BuildOutlineProxies(interactableLayerIndex);
        BuildOccluderProxies(interactableLayerIndex, occluderLayerIndex, uiLayerIndex);

        proxiesBuilt = true;
    }

    private void BuildOutlineProxies(int interactableLayerIndex)
    {
        SpriteRenderer[] renderers = FindObjectsOfType<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            if (sr == null)
                continue;

            GameObject obj = sr.gameObject;

            if (obj == null)
                continue;

            if (obj.name.EndsWith("_OutlineProxy"))
                continue;

            if (obj.name.EndsWith("_OccluderProxy"))
                continue;

            GameLogic.SceneFlag sceneFlag =
                obj.GetComponentInParent<GameLogic.SceneFlag>();

            if (sceneFlag == null)
                continue;

            //obj.layer = interactableLayerIndex;

            InteractableOutlineProxy proxy =
                obj.GetComponent<InteractableOutlineProxy>();

            if (proxy == null)
                proxy = obj.AddComponent<InteractableOutlineProxy>();

            proxy.CreateProxy();
        }
    }

    private void BuildOccluderProxies(
     int interactableLayerIndex,
     int occluderLayerIndex,
     int uiLayerIndex)
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            GameObject obj = renderer.gameObject;

            if (obj == null)
                continue;

            if (obj.name.EndsWith("_OutlineProxy"))
                continue;

            int defaultLayerIndex = LayerMask.NameToLayer("Default");

            if (defaultLayerIndex != -1 && obj.layer != defaultLayerIndex)
                continue;

            if (uiLayerIndex != -1 && obj.layer == uiLayerIndex)
                continue;

            if (obj.GetComponentInParent<Canvas>() != null)
                continue;

            OutlineOccluderProxy proxy =
                obj.GetComponent<OutlineOccluderProxy>();

            if (proxy == null)
                proxy = obj.AddComponent<OutlineOccluderProxy>();

            proxy.source = obj;
            proxy.maskMaterial = maskMaterial;
            proxy.occluderLayer = occluderLayerIndex;
            proxy.CreateProxy();

            Transform point = obj.transform.Find("InteractPoint");

            if (point != null)
                proxy.sortPoint = point;
            else
                proxy.sortPoint = obj.transform;
        }
    }

    private void CreateMaskCamera()
    {
        if (maskCamera != null)
            return;

        GameObject obj = new GameObject("Runtime_MaskCamera");
        obj.transform.SetParent(transform);

        maskCamera = obj.AddComponent<Camera>();
        maskCamera.enabled = true;
        maskCamera.cullingMask = maskLayer;
        maskCamera.clearFlags = CameraClearFlags.SolidColor;
        maskCamera.backgroundColor = new Color(0, 0, 0, 0);
        maskCamera.allowHDR = false;
        maskCamera.allowMSAA = false;

        materialOverride = obj.AddComponent<MaskCameraMaterialOverride>();
        materialOverride.targetLayer = maskLayer;
        materialOverride.maskMaterial = maskMaterial;
    }

    private void CreateOccluderMaskCamera()
    {
        if (occluderMaskCamera != null)
            return;

        GameObject obj = new GameObject("Runtime_OccluderMaskCamera");
        obj.transform.SetParent(transform);

        occluderMaskCamera = obj.AddComponent<Camera>();
        occluderMaskCamera.enabled = true;
        occluderMaskCamera.cullingMask = occluderLayer;
        occluderMaskCamera.clearFlags = CameraClearFlags.SolidColor;
        occluderMaskCamera.backgroundColor = new Color(0, 0, 0, 0);
        occluderMaskCamera.allowHDR = false;
        occluderMaskCamera.allowMSAA = false;

        occluderMaterialOverride = obj.AddComponent<MaskCameraMaterialOverride>();
        occluderMaterialOverride.targetLayer = occluderLayer;
        occluderMaterialOverride.maskMaterial = maskMaterial;
    }

    private void AttachToMainCamera(Camera main)
    {
        if (maskCamera != null)
        {
            maskCamera.transform.SetParent(main.transform);
            maskCamera.transform.localPosition = Vector3.zero;
            maskCamera.transform.localRotation = Quaternion.identity;
            maskCamera.transform.localScale = Vector3.one;
        }

        if (occluderMaskCamera != null)
        {
            occluderMaskCamera.transform.SetParent(main.transform);
            occluderMaskCamera.transform.localPosition = Vector3.zero;
            occluderMaskCamera.transform.localRotation = Quaternion.identity;
            occluderMaskCamera.transform.localScale = Vector3.one;
        }
    }

    private void SyncCameraSettings(Camera main)
    {
        SyncSingleMaskCamera(maskCamera, main, maskLayer, materialOverride);
        SyncSingleMaskCamera(occluderMaskCamera, main, occluderLayer, occluderMaterialOverride);
    }

    private void SyncSingleMaskCamera(
        Camera targetCamera,
        Camera main,
        LayerMask targetLayer,
        MaskCameraMaterialOverride overrideScript)
    {
        if (targetCamera == null)
            return;

        targetCamera.orthographic = main.orthographic;
        targetCamera.orthographicSize = main.orthographicSize;
        targetCamera.fieldOfView = main.fieldOfView;
        targetCamera.nearClipPlane = main.nearClipPlane;
        targetCamera.farClipPlane = main.farClipPlane;
        targetCamera.rect = main.rect;

        targetCamera.cullingMask = targetLayer;
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = new Color(0, 0, 0, 0);
        targetCamera.allowHDR = false;
        targetCamera.allowMSAA = false;

        if (overrideScript != null)
        {
            overrideScript.targetLayer = targetLayer;
            overrideScript.maskMaterial = maskMaterial;
        }
    }

    private void UpdateRenderTexture()
    {
        lastWidth = Mathf.Max(1, Screen.width);
        lastHeight = Mathf.Max(1, Screen.height);

        ReleaseRenderTexture(maskCamera, ref maskRT);
        ReleaseRenderTexture(occluderMaskCamera, ref occluderMaskRT);

        maskRT = CreateRenderTexture("RT_InteractableMask_Runtime");
        occluderMaskRT = CreateRenderTexture("RT_OccluderMask_Runtime");

        if (maskCamera != null)
            maskCamera.targetTexture = maskRT;

        if (occluderMaskCamera != null)
            occluderMaskCamera.targetTexture = occluderMaskRT;

        if (compositeMaterial != null)
        {
            compositeMaterial.SetTexture("_MaskTex", maskRT);
            compositeMaterial.SetTexture("_OccluderMaskTex", occluderMaskRT);
        }
    }

    private RenderTexture CreateRenderTexture(string rtName)
    {
        RenderTexture rt = new RenderTexture(
            lastWidth,
            lastHeight,
            0,
            RenderTextureFormat.ARGB32)
        {
            name = rtName,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };

        rt.Create();
        return rt;
    }

    private void ReleaseRenderTexture(Camera camera, ref RenderTexture rt)
    {
        if (rt == null)
            return;

        if (camera != null && camera.targetTexture == rt)
            camera.targetTexture = null;

        rt.Release();

        if (Application.isPlaying)
            Destroy(rt);
        else
            DestroyImmediate(rt);

        rt = null;
    }

    private void OnDisable()
    {
        ReleaseRenderTexture(maskCamera, ref maskRT);
        ReleaseRenderTexture(occluderMaskCamera, ref occluderMaskRT);
    }
}