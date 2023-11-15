using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamePadCursor : MonoBehaviour
{//https://www.youtube.com/watch?v=Y3WNwl1ObC8
    [SerializeField] private PlayerInput playerInput;

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

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if(virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }
        AnchorCursor(newPosition);
    }

    private void FixedUpdate()
    {
        if (Gamepad.current.leftTrigger.isPressed)
        {
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(currentPosition);
            RaycastHit hit_info = new RaycastHit();
            float max_distance = 100f;

            bool is_hit = Physics.Raycast(ray, out hit_info, max_distance);

            if (is_hit)
            {
                
                if (LayerMask.LayerToName(hit_info.collider.gameObject.layer) != "Ground") return;
                Rigidbody rb = hit_info.rigidbody;
                if (rb == null) return;
                Game.Test.testBlock _testBlock = hit_info.collider.gameObject.GetComponent<Game.Test.testBlock>();
                if (_testBlock == null) return;
                Debug.Log("Selected BlockBaseType: " + _testBlock.GetBlockBaseType());
                if (_testBlock.GetBlockBaseType() == Game.Base.BlockBaseType.Null) return;
                Vector3 collision = _testBlock.getNoApproachField();

                screenPoint = Camera.main.WorldToScreenPoint(hit_info.transform.position);

                float screenX = currentPosition.x;
                float screenY = currentPosition.y;
                float screenZ = screenPoint.z;

                Vector3 currentScreenPoint = properMatch(_testBlock.GetBlockBaseType(), screenPoint, currentPosition);
                //Vector3 currentScreenPoint = new Vector3(screenX, screenY, screenZ);
                Vector3 currentHitsPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);

                if (collision.magnitude == 0)
                {
                    rb.MovePosition(currentHitsPosition);
                } else if ((hit_info.transform.position - collision).magnitude < (currentHitsPosition - collision).magnitude)
                {
                    Debug.Log("Collision: " + collision);
                    rb.MovePosition(currentHitsPosition);
                }
                //hit_info.transform.position = currentHitsPosition;
            }
        }
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
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = new Vector2(0f, 0f);

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
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

    private void OnControlsChanged(PlayerInput input)
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
    }
}
