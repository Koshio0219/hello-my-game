using Cysharp.Threading.Tasks;
using Game.Base;
using Game.Data;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamePadCursor : MonoBehaviour
{//https://www.youtube.com/watch?v=Y3WNwl1ObC8
    //[SerializeField] private PlayerInput playerInput;

    [SerializeField] private RectTransform cursorTransform;

    [SerializeField] private Canvas canvas;

    [SerializeField] private RectTransform canvasRactTransform;

    [SerializeField] private float cursorSpeed = 1000f;

    [SerializeField] private float padding = 50f;

    private bool previousMouseState;
    private Camera mainCamera;
    private GameObject hit_target;
    private Vector3 screenPoint;

    private string previousControlScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";
    private float cursorSpeedDefault = 1000f;

    private Game.Test.PlayerParameter _PlayerParameter;

    private BlockBase currentSelected = null;
    private BlockBase CurrentSelected
    {
        get => currentSelected;
        set
        {
            if(value == null)
            {
                var rb = currentSelected.GetComponent<Rigidbody>();
                rb.mass = 10000;
            }
            else
            {
                var rb = value.GetComponent<Rigidbody>();
                rb.mass = 1;
            }
            currentSelected = value;
        }
    }

    //private Vector2 lastPos;
    //private Vector2 dragDir;

    private void Awake()
    {
        cursorSpeedDefault = cursorSpeed;
        _PlayerParameter =  GameData.Instance.PlayerParameter;
    }

    // Start is called before the first frame update
    private Mouse virtualMouse;
    private Mouse currentMouse;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        if(virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        } else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        //InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        //playerInput.onControlsChanged += OnControlsChanged;
        //lastPos = virtualMouse.position.ReadValue();
    }

    private void OnDisable()
    {
        if(virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        //playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || _PlayerParameter==null|| Gamepad.all[_PlayerParameter.GamepadNumber_D] == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        /*bool aButtonIsPressed = Gamepad.all[_PlayerParameter.GamepadNumber_D].aButton.IsPressed();
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }*/
        AnchorCursor(newPosition);
    }

    private void FixedUpdate()
    {
        if (_PlayerParameter == null) return;
        cursorSpeed = cursorSpeedDefault;
        if (Gamepad.all[_PlayerParameter.GamepadNumber_D].leftTrigger.isPressed)
        {
            if (CurrentSelected != null) 
            {
                SelectedAction();
                return;
            }

            cursorSpeed = cursorSpeedDefault * 0.15f;
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(currentPosition);
            RaycastHit hit_info = new RaycastHit();
            float max_distance = 100f;

            bool is_hit = Physics.Raycast(ray, out hit_info, max_distance);

            if (is_hit)
            {
                
                if (LayerMask.LayerToName(hit_info.collider.gameObject.layer) != "Ground") return;
                Game.Base.BlockBase blockBase = hit_info.collider.gameObject.GetComponent<Game.Base.BlockBase>();
                Transform blockPosition = hit_info.collider.gameObject.transform;
                //Game.Test.testBlock _testBlock = hit_info.collider.gameObject.GetComponent<Game.Test.testBlock>();
                //if (_testBlock == null) return;
                if (blockBase == null) return;
                Debug.Log("Selected BlockBaseType: " + blockBase.BlockUnitData.baseType);
                if (blockBase.BlockUnitData.baseType == Game.Base.BlockBaseType.Null) return;

                //一つのBlockの選択を限定
                CurrentSelected = blockBase;
                SelectedAction();
            }
        }
        else
        {
            if (CurrentSelected != null)
                CurrentSelected = null;            
        }
    }

    private void SelectedAction()
    {
        Vector2 currentPosition = virtualMouse.position.ReadValue();
        //dragDir = currentPosition - lastPos;
        //lastPos = currentPosition;
        Vector3 collision = Vector3.zero;

        var rb = CurrentSelected.GetComponent<Rigidbody>();
        //screenPoint = Camera.main.WorldToScreenPoint(hit_info.transform.position);
        screenPoint = Camera.main.WorldToScreenPoint(CurrentSelected.transform.position);

        //float screenX = currentPosition.x;
        //float screenY = currentPosition.y;
        //float screenZ = screenPoint.z;

        Vector3 currentScreenPoint = properMatch(CurrentSelected.BlockUnitData.baseType, screenPoint, currentPosition);
        //Vector3 currentScreenPoint = new Vector3(screenX, screenY, screenZ);
        Vector3 currentHitsPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);
        Debug.Log("NextPosition: " + currentHitsPosition);
        if (collision.magnitude == 0)
        {
            //currentSelected.transform.position = currentHitsPosition;
            rb.MovePosition(currentHitsPosition);
            Debug.Log("Move! to " + transform.position);
        }
        else if ((CurrentSelected.transform.position - collision).sqrMagnitude < (currentHitsPosition - collision).sqrMagnitude)
        {
            Debug.Log("Collision: " + collision);
            //currentSelected.transform.position = currentHitsPosition;
            //rb.position = currentHitsPosition;
            rb.MovePosition(currentHitsPosition);
        }
        //hit_info.transform.position = currentHitsPosition;
    }

    private Vector3 properMatch(Game.Base.BlockBaseType blockBase, Vector3 origin, Vector2 change)
    {
        float newX = origin.x;
        float newY = origin.y;
        switch (blockBase)
        {
            case Game.Base.BlockBaseType.AutoMoving:
                newX = change.x;
                newY = change.y;
                break;
            case Game.Base.BlockBaseType.LeftRightAble:
                newX = change.x;
                break;
            case Game.Base.BlockBaseType.Null:
                break;
            case Game.Base.BlockBaseType.Static:
                break;
            case Game.Base.BlockBaseType.UpDownAble:
                newY = change.y;
                break;
        }
        return new Vector3(newX, newY, origin.z);
    }
    public void SetPosition()
    {
        if (virtualMouse == null ||_PlayerParameter ==null|| Gamepad.all[_PlayerParameter.GamepadNumber_D] == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = new Vector2(0f, 0f);

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.all[_PlayerParameter.GamepadNumber_D].aButton.IsPressed();
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRactTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    /*private void OnControlsChanged(PlayerInput input)
    {
        if(playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        } else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
        }
    }*/
}
