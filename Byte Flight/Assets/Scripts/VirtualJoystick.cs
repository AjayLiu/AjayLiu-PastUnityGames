﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    public float sensitivity;
    [SerializeField]
    bool leftOfScreen;

    Image bgImg;
    Image joystickImg;
    Vector3 inputVector;

    void Start() {
        bgImg = GetComponentInChildren<Image>();
        joystickImg = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped) {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            if(!leftOfScreen)
                inputVector = new Vector3(pos.x * 2f + 1, 0, pos.y * 2f - 1);
            else
                inputVector = new Vector3(pos.x * 2f - 1, 0, pos.y * 2f - 1);

            inputVector = (inputVector.magnitude > 1f) ? inputVector.normalized : inputVector;

            joystickImg.rectTransform.anchoredPosition = 
                new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 2), inputVector.z * (bgImg.rectTransform.sizeDelta.y / 2));
        }
    }
    public virtual void OnPointerDown(PointerEventData ped) {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped) {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal() {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical() {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

}
