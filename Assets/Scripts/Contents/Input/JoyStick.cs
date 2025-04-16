using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class JoyStick : MonoBehaviour
{
    [SerializeField] private RectTransform joystickBackground; // 조이스틱 배경
    [SerializeField] private RectTransform joystickHandle;     // 조이스틱 핸들
    [SerializeField] private RectTransform joystickArea;       // 조이스틱이 활성화될 우측 하단 영역
    [SerializeField] private float moveRange = 100f;           // 조이스틱 이동 반경
    [SerializeField] private OnScreenStick onScreenStick;      // 입력 시스템용 조이스틱

    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    private Vector2 initialPosition;
    private bool isUsingMouse = false;
    private bool isActive = false; // 조이스틱 활성화 여부

    private void Start()
    {
        initialPosition = joystickBackground.anchoredPosition;
    }

    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            HandleTouchInput();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            HandleMouseInput();
        }
        else
        {
            ResetJoystick();
        }
    }

    private void HandleTouchInput()
    {
        var touch = Touchscreen.current.primaryTouch;
        Vector2 touchPosition = touch.position.ReadValue();

        if (!isActive)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touchPosition))
            {
                ActivateJoystick(touchPosition);
            }
        }
        else
        {
            MoveJoystick(touchPosition);
        }
    }

    private void HandleMouseInput()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (!isUsingMouse)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joystickArea, mousePosition))
            {
                ActivateJoystick(mousePosition);
                isUsingMouse = true;
            }
        }
        else
        {
            MoveJoystick(mousePosition);
        }
    }

    private void ActivateJoystick(Vector2 position)
    {
        joystickBackground.position = position;
        joystickBackground.gameObject.SetActive(true);
        isActive = true;
    }

    private void MoveJoystick(Vector2 position)
    {
        //if (IsPointerOverUI() && IsRaycastHittingUIObject(position))
        //{
        //    return;
        //}

        Vector2 direction = position - (Vector2)joystickBackground.position;
        Vector2 clampedPosition = Vector2.ClampMagnitude(direction, moveRange);
        joystickHandle.anchoredPosition = clampedPosition;

        onScreenStick.OnDrag(new PointerEventData(EventSystem.current)
        {
            position = position
        });


        if (!RectTransformUtility.RectangleContainsScreenPoint(joystickArea, position))
        {
            ResetJoystick();
        }

    }

    private void ResetJoystick()
    {
        isActive = false;
        isUsingMouse = false;
        joystickBackground.anchoredPosition = initialPosition;
        joystickHandle.anchoredPosition = Vector2.zero;

        onScreenStick.OnPointerUp(new PointerEventData(EventSystem.current));
    }
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsRaycastHittingUIObject(Vector2 position)
    {
        if (pointerEventData == null)
            pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = position;
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        return raycastResults.Count > 0;
    }
}