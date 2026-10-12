using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class MaskCameraMaterialOverride : MonoBehaviour
{
    public LayerMask targetLayer;
    public Material maskMaterial;

    private readonly Dictionary<Renderer, Material[]> originalMaterials = new();

    private void OnPreCull()
    {
        if (maskMaterial == null)
            return;

        originalMaterials.Clear();

        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (((1 << r.gameObject.layer) & targetLayer.value) == 0)
                continue;

            originalMaterials[r] = r.sharedMaterials;

            Material[] maskMaterials = new Material[r.sharedMaterials.Length];

            for (int i = 0; i < maskMaterials.Length; i++)
                maskMaterials[i] = maskMaterial;

            r.sharedMaterials = maskMaterials;
        }
    }

    private void OnPostRender()
    {
        foreach (var pair in originalMaterials)
        {
            if (pair.Key != null)
                pair.Key.sharedMaterials = pair.Value;
        }

        originalMaterials.Clear();
    }
}