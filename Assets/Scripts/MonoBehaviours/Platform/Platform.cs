using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    public bool IsRotatable { get; set; }

    public bool dynamicBaseMaterials = false;

    public MeshRenderer meshRendererRef;
    public Material[] delusionMaterials;
    public Material[] baseMaterials;

    private void Awake()
    {
        // Retrieve MeshRenderer component from all platforms in the scene.
        meshRendererRef = GetComponent<MeshRenderer>();
        if (meshRendererRef == null)
            meshRendererRef = GetComponentInChildren<MeshRenderer>();
        if (meshRendererRef == null)
            return;

        if (dynamicBaseMaterials)
        {
            // Store current base materials from the mesh renderer 
            baseMaterials = new Material[meshRendererRef.materials.Length];
            Array.Copy(meshRendererRef.materials, baseMaterials, meshRendererRef.materials.Length);
        }
    }
}
