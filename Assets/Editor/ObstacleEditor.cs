using System.Linq;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Obstacle))]
public class ObstacleEditor : Editor
{
    SerializedProperty _colourKey;
    SerializedProperty _effectVFX;
    SerializedProperty _action;

    SerializedProperty _camouflage;

    SerializedProperty _pointA;
    SerializedProperty _pointB;
    SerializedProperty _translateSpeed;
    SerializedProperty _translateDelay;
    SerializedProperty _isPlatform;

    SerializedProperty _rotateValue;

    private bool foldout;

    private class Variables
    {
        // General Settings
        public static GUIContent colourKey = EditorGUIUtility.TrTextContent("Colour Key",
            "The colour key that determines when this obstacle is active.");

        public static GUIContent effectVFX = EditorGUIUtility.TrTextContent("Effect VFX", 
            "The visual effect to play when this obstacle is activated.");

        public static GUIContent action = EditorGUIUtility.TrTextContent("Action", 
            "The action that this obstacle performs when activated.");

        // Tangibility Settings
        public static GUIContent camouflage = EditorGUIUtility.TrTextContent("Camouflage", 
            "Whether the obstacle should camouflage itself when inactive.");

        // Translate Settings
        public static GUIContent pointA = EditorGUIUtility.TrTextContent("Point A", 
            "The first point of translation.");
        public static GUIContent pointB = EditorGUIUtility.TrTextContent("Point B", 
            "The second point of translation.");
        public static GUIContent translateSpeed = EditorGUIUtility.TrTextContent("Translate Speed", 
            "The speed at which the obstacle translates between Point A and Point B.");
        public static GUIContent translateDelay = EditorGUIUtility.TrTextContent("Translate Delay", 
            "The delay between translation from Point A to Point B.");
        public static GUIContent isPlatform = EditorGUIUtility.TrTextContent("Is Platform", 
            "Whether the obstacle should be considered a platform for the player to stand on.");

        // Rotate Settings
        public static GUIContent rotateValue = EditorGUIUtility.TrTextContent("Rotate Value", 
            "The rotation value to apply when the obstacle is activated.");
    }

    private void SwapTextColours(Color text, Color focus)
    {
        EditorStyles.label.normal.textColor = EditorStyles.label.onNormal.textColor =
        EditorStyles.label.active.textColor = EditorStyles.label.onActive.textColor =
        EditorStyles.label.hover.textColor = EditorStyles.label.onHover.textColor =
            text;
        EditorStyles.label.focused.textColor = EditorStyles.label.onFocused.textColor =
            focus;
    }

    protected virtual void TangibilityFoldout()
    {
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        foldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), foldout,
            "Tangibility", true, EditorStyles.foldoutHeader);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_camouflage, Variables.camouflage);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void TranslateFoldout()
    {
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        foldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), foldout,
            "Translation", true, EditorStyles.foldoutHeader);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_pointA, Variables.pointA);
            EditorGUILayout.PropertyField(_pointB, Variables.pointB);
            EditorGUILayout.PropertyField(_translateSpeed, Variables.translateSpeed);
            EditorGUILayout.PropertyField(_translateDelay, Variables.translateDelay);
            EditorGUILayout.PropertyField(_isPlatform, Variables.isPlatform);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void RotateFoldout()
    {
        EditorGUILayout.Space();

        EditorGUI.indentLevel++;
        foldout = EditorGUI.Foldout
            (EditorGUILayout.GetControlRect(), foldout,
            "Rotation", true, EditorStyles.foldoutHeader);
        if (foldout)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_rotateValue, Variables.rotateValue);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    public void OnEnable()
    {
        _colourKey = serializedObject.FindProperty("colourKey");
        
        _effectVFX = serializedObject.FindProperty("effectVFX");

        _action = serializedObject.FindProperty("action");

        _camouflage = serializedObject.FindProperty("camouflage");

        _pointA = serializedObject.FindProperty("pointA");
        _pointB = serializedObject.FindProperty("pointB");
        _translateSpeed = serializedObject.FindProperty("translateSpeed");
        _translateDelay = serializedObject.FindProperty("translateDelay");
        _isPlatform = serializedObject.FindProperty("isPlatform");

        _rotateValue = serializedObject.FindProperty("rotateValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        // Convert the colour key from a Vector3Int to a Color for
        // the background of the field
        static float byteToDecimal(int value)
        { 
            return value == 0 ? 0 : (64 * value - 1) / 255f; 
        }
        var colourKeyX = byteToDecimal(_colourKey.FindPropertyRelative("x").intValue);
        var colourKeyY = byteToDecimal(_colourKey.FindPropertyRelative("y").intValue);
        var colourKeyZ = byteToDecimal(_colourKey.FindPropertyRelative("z").intValue);
        Texture2D colourKeyTexture = new(1, 1);
        colourKeyTexture.SetPixel(0, 0, new(colourKeyX, colourKeyY, colourKeyZ, 1));
        colourKeyTexture.Apply();
        GUIStyle colourKeyBackground = new();
        colourKeyBackground.normal.background = colourKeyTexture;

        // https://optional.is/required/2011/01/12/maximum-color-contrast/
        // Determine the appropriate text colour based on
        // the intensity of the background colour
        Color contrastText;
        Color contrastFocus;
        if ((colourKeyX * 299 + colourKeyY * 587 + colourKeyZ * 114) >= 128)
        { 
            contrastText  = Color.black;
            contrastFocus = new Color((98 / 255f), (98 / 255f), (98 / 255f), 1);
            //contrastFocus = new Color((122/255f), (96/255f), (128/255f), 1);
        }
        else
        { 
            contrastText  = EditorStyles.label.normal.textColor;
            contrastFocus = EditorStyles.label.focused.textColor;
        }

        EditorGUILayout.BeginHorizontal(colourKeyBackground);
        Color labelColour = EditorStyles.label.normal.textColor;
        Color labelFocus = EditorStyles.label.focused.textColor;
        SwapTextColours(contrastText, contrastFocus);
        _colourKey.vector3IntValue = EditorGUI.Vector3IntField
            (EditorGUILayout.GetControlRect(), Variables.colourKey, _colourKey.vector3IntValue);
        SwapTextColours(labelColour, labelFocus);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel--;
        
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Effect Settings", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_effectVFX, Variables.effectVFX);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Obstacle Action Type", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_action, Variables.action);
        EditorGUI.indentLevel--;

        switch ((ObstacleAction)_action.enumValueIndex)
        {
            case ObstacleAction.Tangibility:
            case ObstacleAction.Intangibility:
            {
                TangibilityFoldout();
                break;
            }
            case ObstacleAction.Translate:
            case ObstacleAction.NoTranslate:
            {
                TranslateFoldout();
                break;
            }
            case ObstacleAction.Rotate:
            case ObstacleAction.NoRotate:
            {
                RotateFoldout();
                break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
