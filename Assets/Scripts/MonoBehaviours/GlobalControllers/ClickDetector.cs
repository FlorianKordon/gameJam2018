using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    // GLOBAL CONTROLLERS
    private GameLogicController _glc;
    private bool _cameraIsMoving = false;

    private void Start()
    {
        _glc = FindObjectOfType<GameLogicController>();
        _glc.CameraIsMovingEvent += OnCameraIsMoving;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // We don't want to detect a raycast hit when we click on the ui elements
            // see for touch difficulties https://answers.unity.com/questions/895861/ui-system-not-blocking-raycasts-on-mobile-only.html
            if (!IsPointerOverUIObject() && Physics.Raycast(ray, out hit, 100)) // or whatever range, if applicable
            {
                GameObject go = hit.transform.gameObject;

                // if the hit gameobject or its parent (empty to fix right handed coordinate system) got tag rotateable
                if (go.tag == "Rotateable")
                    go.GetComponent<RotateablePlatform>().OnInteraction();
                else if (go.transform.parent.tag == "Rotateable")
                    go.GetComponentInParent<RotateablePlatform>().OnInteraction();
            }
            else if (!_cameraIsMoving)
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Rotateable"))
                {
                    item.GetComponent<RotateablePlatform>().IsActivated = false;
                    Outline outline = item.GetComponent<Outline>();
                    if (outline == null)
                        outline = item.GetComponentInChildren<Outline>();
                    outline.enabled = false;
                }
                _glc.NotifyDisabledInputs(false);
            }
        }
    }

    // https://answers.unity.com/questions/1115464/ispointerovergameobject-not-working-with-touch-inp.html
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private void OnCameraIsMoving(bool isMoving)
    {
        _cameraIsMoving = isMoving;
    }

    private void OnDisabled()
    {
        _glc.CameraIsMovingEvent -= OnCameraIsMoving;
    }
}