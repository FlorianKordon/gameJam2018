using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//Inspired by https://www.youtube.com/watch?v=uSnZuBhOA2U

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image backgroundImg, joystickImg;

    //public bool controlsInverted = false;

    public Vector3 InputDirection { get; set; }

    private void Start()
    {
        backgroundImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        InputDirection = Vector3.zero;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
            (backgroundImg.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out pos))
        {

            pos.x = (pos.x / backgroundImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImg.rectTransform.sizeDelta.y);

            InputDirection = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);

            if (InputDirection.magnitude > 1.0f)
            {
                InputDirection = InputDirection.normalized;
            }
            //Debug.Log(InputDirection);

            //Move Joystick
            joystickImg.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (int)(backgroundImg.rectTransform.sizeDelta.x / 2.5),
                                                                     InputDirection.z * (int)(backgroundImg.rectTransform.sizeDelta.y / 2.5));

            // 45° rotation to fit Isometric View
            InputDirection = Quaternion.Euler(0, 45, 0) * InputDirection;
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        //Resset Joystickposition if dropped
        InputDirection = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

}