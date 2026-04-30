using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{
    // Input Action Assets
    SerializedProperty _inputsystem_actions;
    SerializedProperty _player_input;
    private bool inputActionAssetFoldout;

    // Input Action Maps Name Reference
    SerializedProperty _player_actionmap_name;
    SerializedProperty _ui_actionmap_name;
    private bool inputActionMapsFoldout;

    // Input Action Names Reference

    // Player Action Map
    #region
    SerializedProperty _player_move_action_name;
    SerializedProperty _player_look_action_name;
    SerializedProperty _player_attack_action_name;
    SerializedProperty _player_interact_action_name;
    SerializedProperty _player_crouch_action_name;
    SerializedProperty _player_jump_action_name;
    SerializedProperty _player_previous_action_name;
    SerializedProperty _player_next_action_name;
    SerializedProperty _player_sprint_action_name;
    SerializedProperty _player_scrollwheel_action_name;
    SerializedProperty _player_decrease_action_name;
    SerializedProperty _player_increase_action_name;
    SerializedProperty _player_reset_action_name;
    SerializedProperty _player_tutorial_action_name;
    SerializedProperty _player_red_minus_action_name;
    SerializedProperty _player_red_plus_action_name;
    SerializedProperty _player_green_minus_action_name;
    SerializedProperty _player_green_plus_action_name;
    SerializedProperty _player_blue_minus_action_name;
    SerializedProperty _player_blue_plus_action_name;
    SerializedProperty _player_skip_action_name;
    private bool playerActionMapFoldout;
    #endregion

    // UI Action Map
    #region
    SerializedProperty _ui_navigate_action_name;
    SerializedProperty _ui_submit_action_name;
    SerializedProperty _ui_cancel_action_name;
    SerializedProperty _ui_exit_action_name;
    SerializedProperty _ui_point_action_name;
    SerializedProperty _ui_click_action_name;
    SerializedProperty _ui_rightclick_action_name;
    SerializedProperty _ui_middleclick_action_name;
    SerializedProperty _ui_scrollwheel_action_name;
    SerializedProperty _ui_trackeddeviceposition_action_name;
    SerializedProperty _ui_trackeddeviceorientation_action_name;
    SerializedProperty _ui_reset_action_name;
    private bool uiActionMapFoldout;
    #endregion

    private class Variables
    {
        // Input Action Assets
        public static GUIContent inputsystem_actions =
            EditorGUIUtility.TrTextContent("Input System Actions",
            "The Input Action Asset that contains the input actions for the player and UI.");
        public static GUIContent player_input =
            EditorGUIUtility.TrTextContent("Player Input",
            "The Player Input component that manages the player's input actions.");

        // Input Action Maps Name Reference
        public static GUIContent player_actionmap_name =
            EditorGUIUtility.TrTextContent("Player",
            "The name of the action map in the Input Action Asset that contains the player's input actions.");
        public static GUIContent ui_actionmap_name =
            EditorGUIUtility.TrTextContent("UI",
            "The name of the action map in the Input Action Asset that contains the UI's input actions.");

        // Input Action Names Reference
        // Player Action Map
        #region
        public static GUIContent player_move_action_name =
            EditorGUIUtility.TrTextContent("Player Move",
            "The name of the move action in the player's action map.");
        public static GUIContent player_look_action_name =
            EditorGUIUtility.TrTextContent("Player Look",
            "The name of the look action in the player's action map.");
        public static GUIContent player_attack_action_name =
            EditorGUIUtility.TrTextContent("Player Attack",
            "The name of the attack action in the player's action map.");
        public static GUIContent player_interact_action_name =
            EditorGUIUtility.TrTextContent("Player Interact",
            "The name of the interact action in the player's action map.");
        public static GUIContent player_crouch_action_name =
            EditorGUIUtility.TrTextContent("Player Crouch",
            "The name of the crouch action in the player's action map.");
        public static GUIContent player_jump_action_name =
            EditorGUIUtility.TrTextContent("Player Jump",
            "The name of the jump action in the player's action map.");
        public static GUIContent player_previous_action_name =
            EditorGUIUtility.TrTextContent("Player Previous",
            "The name of the previous action in the player's action map.");
        public static GUIContent player_next_action_name =
            EditorGUIUtility.TrTextContent("Player Next",
            "The name of the next action in the player's action map.");
        public static GUIContent player_sprint_action_name =
            EditorGUIUtility.TrTextContent("Player Sprint",
            "The name of the sprint action in the player's action map.");
        public static GUIContent player_scrollwheel_action_name =
            EditorGUIUtility.TrTextContent("Player Scroll Wheel",
            "The name of the scroll wheel action in the player's action map.");
        public static GUIContent player_decrease_action_name =
            EditorGUIUtility.TrTextContent("Player Decrease",
            "The name of the decrease action in the player's action map.");
        public static GUIContent player_increase_action_name =
            EditorGUIUtility.TrTextContent("Player Increase",
            "The name of the increase action in the player's action map.");
        public static GUIContent player_reset_action_name =
            EditorGUIUtility.TrTextContent("Player Reset",
            "The name of the reset action in the player's action map.");
        public static GUIContent player_tutorial_action_name =
            EditorGUIUtility.TrTextContent("Player Tutorial",
            "The name of the tutorial action in the player's action map.");
        public static GUIContent player_red_minus_action_name =
            EditorGUIUtility.TrTextContent("Player Red Minus",
            "The name of the decrease red saturation action in the player's action map.");
        public static GUIContent player_red_plus_action_name =
            EditorGUIUtility.TrTextContent("Player Red Plus",
            "The name of the increase red saturation action in the player's action map.");
        public static GUIContent player_green_minus_action_name =
            EditorGUIUtility.TrTextContent("Player Green Minus",
            "The name of the decrease green saturation action in the player's action map.");
        public static GUIContent player_green_plus_action_name =
            EditorGUIUtility.TrTextContent("Player Green Plus",
            "The name of the increase green saturation action in the player's action map.");
        public static GUIContent player_blue_minus_action_name =
            EditorGUIUtility.TrTextContent("Player Blue Minus",
            "The name of the decrease blue saturation action in the player's action map.");
        public static GUIContent player_blue_plus_action_name =
            EditorGUIUtility.TrTextContent("Player Blue Plus",
            "The name of the increase blue saturation action in the player's action map.");
        public static GUIContent player_skip_action_name =
            EditorGUIUtility.TrTextContent("Player Skip",
            "The name of the skip action in the player's action map.");
        #endregion

        // UI Action Map
        #region
        public static GUIContent ui_navigate_action_name =
            EditorGUIUtility.TrTextContent("UI Navigate",
            "The name of the navigate action in the UI's action map.");
        public static GUIContent ui_submit_action_name =
            EditorGUIUtility.TrTextContent("UI Submit",
            "The name of the submit action in the UI's action map.");
        public static GUIContent ui_cancel_action_name =
            EditorGUIUtility.TrTextContent("UI Cancel",
            "The name of the cancel action in the UI's action map.");
        public static GUIContent ui_exit_action_name =
            EditorGUIUtility.TrTextContent("UI Exit",
            "The name of the exit action in the UI's action map.");
        public static GUIContent ui_point_action_name =
            EditorGUIUtility.TrTextContent("UI Point",
            "The name of the point action in the UI's action map.");
        public static GUIContent ui_click_action_name =
            EditorGUIUtility.TrTextContent("UI Click",
            "The name of the left click action in the UI's action map.");
        public static GUIContent ui_rightclick_action_name =
            EditorGUIUtility.TrTextContent("UI Right Click",
            "The name of the right click action in the UI's action map.");
        public static GUIContent ui_middleclick_action_name =
            EditorGUIUtility.TrTextContent("UI Middle Click",
            "The name of the middle click action in the UI's action map.");
        public static GUIContent ui_scrollwheel_action_name =
            EditorGUIUtility.TrTextContent("UI Scroll Wheel",
            "The name of the scroll wheel action in the UI's action map.");
        public static GUIContent ui_trackeddeviceposition_action_name =
            EditorGUIUtility.TrTextContent("UI Tracked Device Position",
            "The name of the tracked device position action in the UI's action map.");
        public static GUIContent ui_trackeddeviceorientation_action_name =
            EditorGUIUtility.TrTextContent("UI Tracked Device Orientation",
            "The name of the tracked device orientation action in the UI's action map.");
        public static GUIContent ui_reset_action_name =
            EditorGUIUtility.TrTextContent("UI Reset",
            "The name of the reset action in the UI's action map.");
        #endregion
    }

    protected virtual void InputActionAssetFoldout()
    {
        EditorGUI.indentLevel++;
        inputActionAssetFoldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), inputActionAssetFoldout,
            "Input Action Assets", true, EditorStyles.foldoutHeader);
        if (inputActionAssetFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_inputsystem_actions, Variables.inputsystem_actions);
            EditorGUILayout.PropertyField(_player_input, Variables.player_input);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void InputActionMapsFoldout()
    {
        EditorGUI.indentLevel++;
        inputActionMapsFoldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), inputActionMapsFoldout,
            "Action Maps Name Reference", true, EditorStyles.foldoutHeader);
        if (inputActionMapsFoldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_player_actionmap_name, Variables.player_actionmap_name);
            EditorGUILayout.PropertyField(_ui_actionmap_name, Variables.ui_actionmap_name);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void PlayerActionMapFoldout()
    {
        EditorGUI.indentLevel++;
        playerActionMapFoldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), playerActionMapFoldout,
            "Player", true, EditorStyles.foldoutHeader);
        if (playerActionMapFoldout)
        {
            EditorGUI.indentLevel++;
            #region
            EditorGUILayout.PropertyField(_player_move_action_name, Variables.player_move_action_name);
            EditorGUILayout.PropertyField(_player_look_action_name, Variables.player_look_action_name);
            EditorGUILayout.PropertyField(_player_attack_action_name, Variables.player_attack_action_name);
            EditorGUILayout.PropertyField(_player_interact_action_name, Variables.player_interact_action_name);
            EditorGUILayout.PropertyField(_player_crouch_action_name, Variables.player_crouch_action_name);
            EditorGUILayout.PropertyField(_player_jump_action_name, Variables.player_jump_action_name);
            EditorGUILayout.PropertyField(_player_previous_action_name, Variables.player_previous_action_name);
            EditorGUILayout.PropertyField(_player_next_action_name, Variables.player_next_action_name);
            EditorGUILayout.PropertyField(_player_sprint_action_name, Variables.player_sprint_action_name);
            EditorGUILayout.PropertyField(_player_scrollwheel_action_name, Variables.player_scrollwheel_action_name);
            EditorGUILayout.PropertyField(_player_decrease_action_name, Variables.player_decrease_action_name);
            EditorGUILayout.PropertyField(_player_increase_action_name, Variables.player_increase_action_name);
            EditorGUILayout.PropertyField(_player_reset_action_name, Variables.player_reset_action_name);
            EditorGUILayout.PropertyField(_player_tutorial_action_name, Variables.player_tutorial_action_name);
            EditorGUILayout.PropertyField(_player_red_minus_action_name, Variables.player_red_minus_action_name);
            EditorGUILayout.PropertyField(_player_red_plus_action_name, Variables.player_red_plus_action_name);
            EditorGUILayout.PropertyField(_player_green_minus_action_name, Variables.player_green_minus_action_name);
            EditorGUILayout.PropertyField(_player_green_plus_action_name, Variables.player_green_plus_action_name);
            EditorGUILayout.PropertyField(_player_blue_minus_action_name, Variables.player_blue_minus_action_name);
            EditorGUILayout.PropertyField(_player_blue_plus_action_name, Variables.player_blue_plus_action_name);
            EditorGUILayout.PropertyField(_player_skip_action_name, Variables.player_skip_action_name);
            #endregion
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void UIActionMapFoldout()
    {
        EditorGUI.indentLevel++;
        uiActionMapFoldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), uiActionMapFoldout,
            "UI", true, EditorStyles.foldoutHeader);
        if (uiActionMapFoldout)
        {
            EditorGUI.indentLevel++;
            #region
            EditorGUILayout.PropertyField(_ui_navigate_action_name, Variables.ui_navigate_action_name);
            EditorGUILayout.PropertyField(_ui_submit_action_name, Variables.ui_submit_action_name);
            EditorGUILayout.PropertyField(_ui_cancel_action_name, Variables.ui_cancel_action_name);
            EditorGUILayout.PropertyField(_ui_exit_action_name, Variables.ui_exit_action_name);
            EditorGUILayout.PropertyField(_ui_point_action_name, Variables.ui_point_action_name);
            EditorGUILayout.PropertyField(_ui_click_action_name, Variables.ui_click_action_name);
            EditorGUILayout.PropertyField(_ui_rightclick_action_name, Variables.ui_rightclick_action_name);
            EditorGUILayout.PropertyField(_ui_middleclick_action_name, Variables.ui_middleclick_action_name);
            EditorGUILayout.PropertyField(_ui_scrollwheel_action_name, Variables.ui_scrollwheel_action_name);
            EditorGUILayout.PropertyField(_ui_trackeddeviceposition_action_name, Variables.ui_trackeddeviceposition_action_name);
            EditorGUILayout.PropertyField(_ui_trackeddeviceorientation_action_name, Variables.ui_trackeddeviceorientation_action_name);
            EditorGUILayout.PropertyField(_ui_reset_action_name, Variables.ui_reset_action_name);
            #endregion
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    public void OnEnable()
    {
        // Input Action Assets
        _inputsystem_actions = serializedObject.FindProperty("inputsystem_actions");
        _player_input = serializedObject.FindProperty("player_input");

        // Input Action Maps Name Reference
        _player_actionmap_name = serializedObject.FindProperty("player");
        _ui_actionmap_name = serializedObject.FindProperty("ui");

        // Input Action Names Reference
        // Player Action Map
        #region
        _player_move_action_name = serializedObject.FindProperty("player_move");
        _player_look_action_name = serializedObject.FindProperty("player_look");
        _player_attack_action_name = serializedObject.FindProperty("player_attack");
        _player_interact_action_name = serializedObject.FindProperty("player_interact");
        _player_crouch_action_name = serializedObject.FindProperty("player_crouch");
        _player_jump_action_name = serializedObject.FindProperty("player_jump");
        _player_previous_action_name = serializedObject.FindProperty("player_previous");
        _player_next_action_name = serializedObject.FindProperty("player_next");
        _player_sprint_action_name = serializedObject.FindProperty("player_sprint");
        _player_scrollwheel_action_name = serializedObject.FindProperty("player_scrollwheel");
        _player_decrease_action_name = serializedObject.FindProperty("player_decrease");
        _player_increase_action_name = serializedObject.FindProperty("player_increase");
        _player_reset_action_name = serializedObject.FindProperty("player_reset");
        _player_tutorial_action_name = serializedObject.FindProperty("player_tutorial");
        _player_red_minus_action_name = serializedObject.FindProperty("player_red_minus");
        _player_red_plus_action_name = serializedObject.FindProperty("player_red_plus");
        _player_green_minus_action_name = serializedObject.FindProperty("player_green_minus");
        _player_green_plus_action_name = serializedObject.FindProperty("player_green_plus");
        _player_blue_minus_action_name = serializedObject.FindProperty("player_blue_minus");
        _player_blue_plus_action_name = serializedObject.FindProperty("player_blue_plus");
        _player_skip_action_name = serializedObject.FindProperty("player_skip");
        #endregion

        // UI Action Map
        #region
        _ui_navigate_action_name = serializedObject.FindProperty("ui_navigate");
        _ui_submit_action_name = serializedObject.FindProperty("ui_submit");
        _ui_cancel_action_name = serializedObject.FindProperty("ui_cancel");
        _ui_exit_action_name = serializedObject.FindProperty("ui_exit");
        _ui_point_action_name = serializedObject.FindProperty("ui_point");
        _ui_click_action_name = serializedObject.FindProperty("ui_click");
        _ui_rightclick_action_name = serializedObject.FindProperty("ui_rightclick");
        _ui_middleclick_action_name = serializedObject.FindProperty("ui_middleclick");
        _ui_scrollwheel_action_name = serializedObject.FindProperty("ui_scrollwheel");
        _ui_trackeddeviceposition_action_name = serializedObject.FindProperty("ui_trackeddeviceposition");
        _ui_trackeddeviceorientation_action_name = serializedObject.FindProperty("ui_trackeddeviceorientation");
        _ui_reset_action_name = serializedObject.FindProperty("ui_reset");
        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        InputActionAssetFoldout();
        EditorGUILayout.Space();
        InputActionMapsFoldout();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Action Name References", EditorStyles.boldLabel);
        PlayerActionMapFoldout();
        UIActionMapFoldout();
        serializedObject.ApplyModifiedProperties();
    }
}