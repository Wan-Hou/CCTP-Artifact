using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance = null;
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset inputsystem_actions;
    [SerializeField] private PlayerInput player_input;
    [HideInInspector]
    public ActiveDevices active_device = ActiveDevices.gamepad;
    [HideInInspector]
    public GamepadType gamepad_type = GamepadType.xbox;
    [HideInInspector]
    public bool analogMovement = false;

    // Enums
    #region
    public enum ActiveDevices
    {
        keyboard_and_mouse = 0,
        gamepad = 1,
        touch = 2,
        joystick = 3,
        xr = 4
    }

    public enum GamepadType
    {
        xbox = 0,
        playstation = 1,
        android = 2,
        ios = 3,
        nimbus = 4,
        webgl = 5,
    }
    #endregion

    [Header("Action Map Name References")]
    #region
    [SerializeField] private string player = "Player";
    [SerializeField] private string ui = "UI";
    #endregion

    [Header("Action Name References")]
    [Header("Player")]
    #region
    [SerializeField] private string player_move = "Move";
    [SerializeField] private string player_look = "Look";
    [SerializeField] private string player_attack = "Attack";
    [SerializeField] private string player_interact = "Interact";
    [SerializeField] private string player_crouch = "Crouch";
    [SerializeField] private string player_jump = "Jump";
    [SerializeField] private string player_previous = "Previous";
    [SerializeField] private string player_next = "Next";
    [SerializeField] private string player_sprint = "Sprint";
    [SerializeField] private string player_scrollwheel = "ScrollWheel";
    [SerializeField] private string player_decrease = "Decrease";
    [SerializeField] private string player_increase = "Increase";
    #endregion

    [Header("UI")]
    #region
    [SerializeField] private string ui_navigate = "Navigate";
    [SerializeField] private string ui_submit = "Submit";
    [SerializeField] private string ui_cancel = "Cancel";
    [SerializeField] private string ui_exit = "Exit";
    [SerializeField] private string ui_point = "Point";
    [SerializeField] private string ui_click = "Click";
    [SerializeField] private string ui_rightclick = "RightClick";
    [SerializeField] private string ui_middleclick = "MiddleClick";
    [SerializeField] private string ui_scrollwheel = "ScrollWheel";
    [SerializeField] private string ui_trackeddeviceposition = "TrackedDevicePosition";
    [SerializeField] private string ui_trackeddeviceorientation = "TrackedDeviceOrientation";
    #endregion

    // Local references to action maps and actions
    #region
    private InputActionMap player_map;
    private InputAction player_move_action;
    private InputAction player_look_action;
    private InputAction player_attack_action;
    private InputAction player_interact_action;
    private InputAction player_crouch_action;
    private InputAction player_jump_action;
    private InputAction player_previous_action;
    private InputAction player_next_action;
    private InputAction player_sprint_action;
    private InputAction player_scrollwheel_action;
    private InputAction player_decrease_action;
    private InputAction player_increase_action;

    private InputActionMap ui_map;
    private InputAction ui_navigate_action;
    private InputAction ui_submit_action;
    private InputAction ui_cancel_action;
    private InputAction ui_exit_action;
    private InputAction ui_point_action;
    private InputAction ui_click_action;
    private InputAction ui_rightclick_action;
    private InputAction ui_middleclick_action;
    private InputAction ui_scrollwheel_action;
    private InputAction ui_trackeddeviceposition_action;
    private InputAction ui_trackeddeviceorientation_action;
    #endregion

    // Public accessors for input states
    #region
    public Vector2 player_move_input { get; private set; }
    public Vector2 player_look_input { get; private set; }
    public bool player_attack_triggered { get; private set; }
    public bool player_interact_triggered { get; private set; }
    public bool player_crouch_triggered { get; private set; }
    public bool player_jump_triggered { get; private set; }
    public bool player_previous_triggered { get; private set; }
    public bool player_next_triggered { get; private set; }
    public bool player_sprint_triggered { get; private set; }
    public Vector2 player_scrollwheel_input { get; private set; }
    public bool player_decrease_triggered { get; private set; }
    public bool player_increase_triggered { get; private set; }

    public Vector2 ui_navigation_input { get; private set; }
    public bool ui_submit_triggered { get; private set; }
    public bool ui_cancel_triggered { get; private set; }
    public bool ui_exit_triggered { get; private set; }
    public Vector2 ui_point_input { get; private set; }
    public bool ui_click_triggered { get; private set; }
    public bool ui_right_click_triggered { get; private set; }
    public bool ui_middle_click_triggered { get; private set; }
    public Vector2 ui_scroll_wheel_input { get; private set; }
    public Vector3 ui_tracked_device_position_input { get; private set; }
    public Quaternion ui_tracked_device_orientation_input { get; private set; }
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        // player actions
        #region
        player_map = inputsystem_actions.FindActionMap(player);
        player_move_action = inputsystem_actions.FindActionMap(player).FindAction(player_move);
        player_look_action = inputsystem_actions.FindActionMap(player).FindAction(player_look);
        player_attack_action = inputsystem_actions.FindActionMap(player).FindAction(player_attack);
        player_interact_action = inputsystem_actions.FindActionMap(player).FindAction(player_interact);
        player_crouch_action = inputsystem_actions.FindActionMap(player).FindAction(player_crouch);
        player_jump_action = inputsystem_actions.FindActionMap(player).FindAction(player_jump);
        player_previous_action = inputsystem_actions.FindActionMap(player).FindAction(player_previous);
        player_next_action = inputsystem_actions.FindActionMap(player).FindAction(player_next);
        player_sprint_action = inputsystem_actions.FindActionMap(player).FindAction(player_sprint);
        player_scrollwheel_action = inputsystem_actions.FindActionMap(player).FindAction(player_scrollwheel);
        player_decrease_action = inputsystem_actions.FindActionMap(player).FindAction(player_decrease);
        player_increase_action = inputsystem_actions.FindActionMap(player).FindAction(player_increase);
        #endregion

        // ui actions
        #region
        ui_map = inputsystem_actions.FindActionMap(ui);
        ui_navigate_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_navigate);
        ui_submit_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_submit);
        ui_cancel_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_cancel);
        ui_exit_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_exit);
        ui_point_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_point);
        ui_click_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_click);
        ui_rightclick_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_rightclick);
        ui_middleclick_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_middleclick);
        ui_scrollwheel_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_scrollwheel);
        ui_trackeddeviceposition_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_trackeddeviceposition);
        ui_trackeddeviceorientation_action = inputsystem_actions.FindActionMap(ui).FindAction(ui_trackeddeviceorientation);
        #endregion

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        // player actions
        #region
        player_move_action.performed += context => player_move_input = context.ReadValue<Vector2>();
        player_move_action.canceled += context => player_move_input = Vector2.zero;

        player_look_action.performed += context => player_look_input = context.ReadValue<Vector2>();
        player_look_action.canceled += context => player_look_input = Vector2.zero;

        player_attack_action.performed += context => player_attack_triggered = true;
        player_attack_action.canceled += context => player_attack_triggered = false;

        player_interact_action.performed += context => player_interact_triggered = true;
        player_interact_action.canceled += context => player_interact_triggered = false;

        player_crouch_action.performed += context => player_crouch_triggered = true;
        player_crouch_action.canceled += context => player_crouch_triggered = false;

        player_jump_action.performed += context => player_jump_triggered = true;
        player_jump_action.canceled += context => player_jump_triggered = false;

        player_previous_action.performed += context => player_previous_triggered = true;
        player_previous_action.canceled += context => player_previous_triggered = false;

        player_next_action.performed += context => player_next_triggered = true;
        player_next_action.canceled += context => player_next_triggered = false;

        player_sprint_action.performed += context => player_sprint_triggered = true;
        player_sprint_action.canceled += context => player_sprint_triggered = false;

        player_scrollwheel_action.performed += context => player_scrollwheel_input = context.ReadValue<Vector2>();
        player_scrollwheel_action.canceled += context => player_scrollwheel_input = Vector2.zero;

        player_decrease_action.performed += context => player_decrease_triggered = true;
        player_decrease_action.canceled += context => player_decrease_triggered = false;

        player_increase_action.performed += context => player_increase_triggered = true;
        player_increase_action.canceled += context => player_increase_triggered = false;

        #endregion

        // ui actions
        #region
        ui_navigate_action.performed += context => ui_navigation_input = context.ReadValue<Vector2>();
        ui_navigate_action.canceled += context => ui_navigation_input = Vector2.zero;

        ui_submit_action.performed += context => ui_submit_triggered = true;
        ui_submit_action.canceled += context => ui_submit_triggered = false;

        ui_cancel_action.performed += context => ui_cancel_triggered = true;
        ui_cancel_action.canceled += context => ui_cancel_triggered = false;

        ui_exit_action.performed += context => ui_exit_triggered = true;
        ui_exit_action.canceled += context => ui_exit_triggered = false;

        ui_point_action.performed += context => ui_point_input = context.ReadValue<Vector2>();
        ui_point_action.canceled += context => ui_point_input = Vector2.zero;

        ui_click_action.performed += context => ui_click_triggered = true;
        ui_click_action.canceled += context => ui_click_triggered = false;

        ui_rightclick_action.performed += context => ui_right_click_triggered = true;
        ui_rightclick_action.canceled += context => ui_right_click_triggered = false;

        ui_middleclick_action.performed += context => ui_middle_click_triggered = true;
        ui_middleclick_action.canceled += context => ui_middle_click_triggered = false;

        ui_scrollwheel_action.performed += context => ui_scroll_wheel_input = context.ReadValue<Vector2>();
        ui_scrollwheel_action.canceled += context => ui_scroll_wheel_input = Vector2.zero;

        ui_trackeddeviceposition_action.performed += context => ui_tracked_device_position_input = context.ReadValue<Vector3>();
        ui_trackeddeviceposition_action.canceled += context => ui_tracked_device_position_input = Vector3.zero;

        ui_trackeddeviceorientation_action.performed += context => ui_tracked_device_orientation_input = context.ReadValue<Quaternion>();
        ui_trackeddeviceorientation_action.canceled += context => ui_tracked_device_orientation_input = Quaternion.Euler(Vector3.zero);

        #endregion
    }

    private void OnEnable()
    {
        player_map.Enable();
        ui_map.Enable();
    }

    public void EnablePlayerInput()
    {
        ui_map.Disable();
        player_map.Enable();
    }

    public void EnableUIInput()
    {
        player_map.Disable();
        ui_map.Enable();
    }

    private void OnDisable()
    {
        player_map.Disable();
        ui_map.Disable();
    }

    [ContextMenu("Changes active_device to Current Control")]
    public void OnControlChange()
    {
        switch (player_input.currentControlScheme)
        {
            case "Keyboard&Mouse":
                {
                    active_device = ActiveDevices.keyboard_and_mouse;
                    break;
                }
            case "Gamepad":
                {
                    active_device = ActiveDevices.gamepad;
                    break;
                }
            case "Touch":
                {
                    active_device = ActiveDevices.touch;
                    break;
                }
            case "Joystick":
                {
                    active_device = ActiveDevices.joystick;
                    break;
                }
            case "XR":
                {
                    active_device = ActiveDevices.xr;
                    break;
                }
            default:
                {
                    active_device = ActiveDevices.gamepad;
                    break;
                }
        }
    }
}
