using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
	public string EligableForMaterialChangeTag = "EligableForMaterialChange";
    // GLOBAL CONTROLLERS
    private GameLogicController _glc;

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _glc.DelusionActivityEvent += OnDilusionActive;
    }

    private void OnDilusionActive(bool active)
    {
        if (active)
        {
            //sensedObjects = GameObject.FindGameObjectsWithTag(EligableForMaterialChangeTag);
        }
        else
        {

        }
    }

    private void OnDisable()
    {
        _glc.DelusionActivityEvent -= OnDilusionActive;
    }
}
