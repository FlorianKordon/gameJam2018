using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollider : MonoBehaviour
{
    public Transform offset;
    private GameObject _portalCtrl;

    void OnTriggerEnter()
    {
        string name = gameObject.name;
        _portalCtrl = GameObject.Find("PortalCtrl");
        _portalCtrl.GetComponent<PortalController>().Porting(name);
    }
}
