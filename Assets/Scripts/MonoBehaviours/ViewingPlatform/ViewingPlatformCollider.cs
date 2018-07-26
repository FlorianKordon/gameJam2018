using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingPlatformCollider : MonoBehaviour
{
    public bool replenishable = true;
    public bool Active { get; set; }

    private ViewingPlatformController _viewingPlatformController;

    private void Awake()
    {
        Active = true;
        _viewingPlatformController = GetComponentInParent<ViewingPlatformController>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (Active)
        {
            GetComponentInParent<ViewingPlatformController>().viewing = true;
            Active = false;
            if(replenishable)
                StartCoroutine(WaitForActivation());
        }
    }

    private IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(_viewingPlatformController.viewingTime + 1);
        Active = true;
    }
}
