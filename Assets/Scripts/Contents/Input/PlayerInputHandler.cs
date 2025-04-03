using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private bool isInputable = true;

    [HideInInspector]
    public Vector2 inputDirection;

    private PlayerFSM playerFSM;

    public UnityEvent onAttackEvent;
    public UnityEvent onInteractEvent;
    public UnityEvent<Vector2> onMoveAndRotateEvent;

    [SerializeField]
    private GameObject joyStickPrefab;

    private JoyStick joyStick;

    private void OnEnable()
    {
        isInputable = true;
        if (joyStick != null)
        {
            joyStick.enabled = true;
        }
    }

    private void OnDisable()
    {
        isInputable = false;
        if (joyStick != null)
        {
            joyStick.enabled = false;
        }
    }

    private void Awake()
    {
        if (!IsMoveUI())
        {
            GameObject newJoystick = Instantiate(joyStickPrefab);

            joyStick = newJoystick.GetComponentInChildren<JoyStick>();
        }
    }

    private void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }

    // TODO :: TestPlayer -> PlayerInput -> Events -> player -> moveAndRotate에 연결
    public void OnMoveAndRotate(InputAction.CallbackContext context)
    {
        if (!isInputable)
        {
            return;
        }

        inputDirection = context.ReadValue<Vector2>();
        onMoveAndRotateEvent?.Invoke(inputDirection);
    }

    // TODO :: TestPlayer -> PlayerInput -> Events -> player -> mainToolAndAttack에 연결
    public void OnMainToolUsedAndAttck(InputAction.CallbackContext context)
    {
        if (!isInputable)
        {
            return;
        }

        if (playerFSM.CanAttack && context.phase == InputActionPhase.Performed)
        {
            onAttackEvent?.Invoke();
        }
    }

    // TODO :: TestPlayer -> PlayerInput -> Events -> player -> interact에 연결
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!isInputable)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            onInteractEvent?.Invoke();
        }
    }

    // TODO :: TestPlayer -> PlayerInput -> Events -> player -> subTool에 연결
    public void OnSubToolUsed(InputAction.CallbackContext context)
    {
        if (!isInputable)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {

        }
    }

    // TODO :: TestPlayer -> PlayerInput -> Events -> player -> Building에 연결
    public void OnBuild(InputAction.CallbackContext context)
    {
        if (!isInputable)
        {
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {

        }
    }

    // TODO :: UI 유무 판단
    private bool IsMoveUI()
    {
        GameObject moveUI = GameObject.FindWithTag("MoveUI");

        if (moveUI == null)
        {
            return false;
        }

        joyStick = moveUI.GetComponentInChildren<JoyStick>();
        return true;
    }
}
