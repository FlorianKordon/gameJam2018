using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public Material skyBoxBaseMaterial;
    public Material skyBoxDelusionMaterial;

    public bool affectSkyBox = true;
    public bool affectPlatforms = true;
    //public bool affectPlayerCharacter = true;

    // GLOBAL CONTROLLERS
    private GameLogicController _glc;
    private Platform[] platforms;

    private void Awake()
    {
        platforms = GameObject.FindObjectsOfType<Platform>();
    }

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _glc.DelusionActivityEvent += OnDilusionActive;
    }

    // EVENT LISTENER
    private void OnDilusionActive(bool active)
    {
        // On dilusion state, swap out materials for both skybox and platform
        if (active)
        {
            // Set skybox material
            if (affectSkyBox)
            {
                RenderSettings.skybox = skyBoxDelusionMaterial;
            }

            // Set platform material
            if (affectPlatforms)
            {
                foreach (Platform p in platforms)
                {
                    p.meshRendererRef.materials = p.delusionMaterials;
                    HandleOutlineEffectDuringMaterialSwap(p);
                }
            }
        }
        // On non-dilusion state, swap out materials back to the base materials
        else
        {
            // Set skybox material
            if (affectSkyBox)
            {
                RenderSettings.skybox = skyBoxBaseMaterial;
            }

            // Set platform material
            if (affectPlatforms)
            {
                foreach (Platform p in platforms)
                {
                    p.meshRendererRef.materials = p.baseMaterials;
                    HandleOutlineEffectDuringMaterialSwap(p);
                }
            }
        }
    }

    private void HandleOutlineEffectDuringMaterialSwap(Platform p)
    {
        // The material swap eliminates the outline effect. We therefore need to set it back manually
        if (p.tag == "Rotateable")
        {
            RotateablePlatform rotPlat = ((RotateablePlatform)p);
            if (rotPlat != null && rotPlat.IsActivated)
            {
                rotPlat.outlineEffect.enabled = false;
                rotPlat.outlineEffect.enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        _glc.DelusionActivityEvent -= OnDilusionActive;
    }
}
