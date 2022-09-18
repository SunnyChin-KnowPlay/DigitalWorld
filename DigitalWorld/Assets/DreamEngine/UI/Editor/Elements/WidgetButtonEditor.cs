#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DreamEngine.UI;

namespace DreamEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(WidgetButton))]
    public class WidgetButtonEditor : Editor
    {
        private WidgetButton buttonTarget;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            buttonTarget = (WidgetButton)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
          
            DreamUIEditorHandler.DrawComponentHeader(customSkin, "Button Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = DreamUIEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var buttonIcon = serializedObject.FindProperty("buttonIcon");
            var buttonText = serializedObject.FindProperty("buttonText");
            var hoverSound = serializedObject.FindProperty("hoverSound");
            var clickSound = serializedObject.FindProperty("clickSound");
            var normalText = serializedObject.FindProperty("normalText");
            var highlightedText = serializedObject.FindProperty("highlightedText");
            var pressedText = serializedObject.FindProperty("pressedText");
            var normalIcon = serializedObject.FindProperty("normalIcon");
            var highlightedIcon = serializedObject.FindProperty("highlightedIcon");
            var pressedIcon = serializedObject.FindProperty("pressedIcon");
            var soundSource = serializedObject.FindProperty("soundSource");
            var rippleParent = serializedObject.FindProperty("rippleParent");
            var useCustomContent = serializedObject.FindProperty("useCustomContent");
            var enableButtonSounds = serializedObject.FindProperty("enableButtonSounds");
            var useHoverSound = serializedObject.FindProperty("useHoverSound");
            var useClickSound = serializedObject.FindProperty("useClickSound");
            var enableIcon = serializedObject.FindProperty("enableIcon");
            var useRipple = serializedObject.FindProperty("useRipple");
            var renderOnTop = serializedObject.FindProperty("renderOnTop");
            var centered = serializedObject.FindProperty("centered");
            var rippleShape = serializedObject.FindProperty("rippleShape");
            var speed = serializedObject.FindProperty("speed");
            var maxSize = serializedObject.FindProperty("maxSize");
            var startColor = serializedObject.FindProperty("startColor");
            var transitionColor = serializedObject.FindProperty("transitionColor");
            var animationSolution = serializedObject.FindProperty("animationSolution");
            var fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
            var rippleUpdateMode = serializedObject.FindProperty("rippleUpdateMode");
            var hoverEffect = serializedObject.FindProperty("hoverEffect");
            var heSpeed = serializedObject.FindProperty("heSpeed");
            var heShape = serializedObject.FindProperty("heShape");
            var heTransitionAlpha = serializedObject.FindProperty("heTransitionAlpha");
            var useHoverEffect = serializedObject.FindProperty("useHoverEffect");
            var heSize = serializedObject.FindProperty("heSize");
            var autoFitContent = serializedObject.FindProperty("autoFitContent");
            var contentSizeFitter = serializedObject.FindProperty("contentSizeFitter");
            var normalLayout = serializedObject.FindProperty("normalLayout");
            var highlightedLayout = serializedObject.FindProperty("highlightedLayout");
            var contentPadding = serializedObject.FindProperty("contentPadding");
            var contentSpacing = serializedObject.FindProperty("contentSpacing");

            switch (currentTab)
            {
                case 0:
                    DreamUIEditorHandler.DrawHeader(customSkin, "Content Header", 6);

                    if (enableIcon.boolValue == true)
                    {
                        DreamUIEditorHandler.DrawProperty(buttonIcon, customSkin, "Button Icon");

                        if (useCustomContent.boolValue == false)
                        {
                            if (buttonTarget.normalIcon != null) { buttonTarget.normalIcon.sprite = buttonTarget.buttonIcon; }
                            if (buttonTarget.highlightedIcon != null) { buttonTarget.highlightedIcon.sprite = buttonTarget.buttonIcon; }
                        }

                        else if (useCustomContent.boolValue == false && buttonTarget.normalIcon == null)
                            EditorGUILayout.HelpBox("'Icon Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                    }

                    if (useCustomContent.boolValue == false)
                    {
                        DreamUIEditorHandler.DrawProperty(buttonText, customSkin, "Button Text");

                        if (buttonTarget.normalText != null) { buttonTarget.normalText.text = buttonText.stringValue; }
                        if (buttonTarget.highlightedText != null) { buttonTarget.highlightedText.text = buttonText.stringValue; }
                    }

                    if (enableButtonSounds.boolValue == true && useHoverSound.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(hoverSound, customSkin, "Hover Sound");

                    if (enableButtonSounds.boolValue == true && useClickSound.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(clickSound, customSkin, "Click Sound");

                    if (GUILayout.Button("Update Content", customSkin.button))
                        buttonTarget.UpdateUI();

                    break;

                case 1:
                    DreamUIEditorHandler.DrawHeader(customSkin, "Core Header", 6);

                    if (useCustomContent.boolValue == false)
                    {
                        DreamUIEditorHandler.DrawProperty(normalText, customSkin, "Normal Text");
                        DreamUIEditorHandler.DrawProperty(highlightedText, customSkin, "Highlighted Text");
                        DreamUIEditorHandler.DrawProperty(pressedText, customSkin, "Pressed Text");
                    }

                    if (enableIcon.boolValue == true)
                    {
                        DreamUIEditorHandler.DrawProperty(normalIcon, customSkin, "Normal Icon");
                        DreamUIEditorHandler.DrawProperty(highlightedIcon, customSkin, "Highlighted Icon");
                        DreamUIEditorHandler.DrawProperty(pressedIcon, customSkin, "Pressed Icon");
                    }

                    if (enableButtonSounds.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(soundSource, customSkin, "Sound Source");

                    if (useRipple.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(rippleParent, customSkin, "Ripple Parent");

                    if (useHoverEffect.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(hoverEffect, customSkin, "Hover Effect");

                    DreamUIEditorHandler.DrawProperty(contentSizeFitter, customSkin, "Content Size Fitter");
                    DreamUIEditorHandler.DrawProperty(normalLayout, customSkin, "Normal Layout");
                    DreamUIEditorHandler.DrawProperty(highlightedLayout, customSkin, "Highlighted Layout");
                    break;

                case 2:
                    DreamUIEditorHandler.DrawHeader(customSkin, "Options Header", 6);

                    DreamUIEditorHandler.DrawProperty(animationSolution, customSkin, "Animation Solution");
                    DreamUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier");
                    DreamUIEditorHandler.DrawProperty(contentPadding, customSkin, "Content Padding");

                    if (enableIcon.boolValue == true)
                        DreamUIEditorHandler.DrawProperty(contentSpacing, customSkin, "Content Spacing");

                    useCustomContent.boolValue = DreamUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content");
                    autoFitContent.boolValue = DreamUIEditorHandler.DrawToggle(autoFitContent.boolValue, customSkin, "Auto Fit Content");
                    enableIcon.boolValue = DreamUIEditorHandler.DrawToggle(enableIcon.boolValue, customSkin, "Enable Icon");
                    enableButtonSounds.boolValue = DreamUIEditorHandler.DrawToggle(enableButtonSounds.boolValue, customSkin, "Enable Button Sounds");

                    if (enableButtonSounds.boolValue == true)
                    {
                        useHoverSound.boolValue = DreamUIEditorHandler.DrawToggle(useHoverSound.boolValue, customSkin, "Enable Hover Sound");
                        useClickSound.boolValue = DreamUIEditorHandler.DrawToggle(useClickSound.boolValue, customSkin, "Enable Click Sound");

                        if (buttonTarget.soundSource == null)
                        {
                            EditorGUILayout.HelpBox("'Sound Source' is not assigned. Go to Resources tab or click the button to create a new audio source.", MessageType.Info);

                            if (GUILayout.Button("Create a new one", customSkin.button))
                            {
                                buttonTarget.soundSource = buttonTarget.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                                currentTab = 2;
                            }
                        }
                    }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-2);
                    useRipple.boolValue = DreamUIEditorHandler.DrawTogglePlain(useRipple.boolValue, customSkin, "Use Ripple");
                    GUILayout.Space(4);

                    if (useRipple.boolValue == true)
                    {
                        renderOnTop.boolValue = DreamUIEditorHandler.DrawToggle(renderOnTop.boolValue, customSkin, "Render On Top");
                        centered.boolValue = DreamUIEditorHandler.DrawToggle(centered.boolValue, customSkin, "Centered");
                        DreamUIEditorHandler.DrawProperty(rippleShape, customSkin, "Shape");
                        DreamUIEditorHandler.DrawProperty(speed, customSkin, "Speed");
                        DreamUIEditorHandler.DrawProperty(maxSize, customSkin, "Max Size");
                        DreamUIEditorHandler.DrawProperty(startColor, customSkin, "Start Color");
                        DreamUIEditorHandler.DrawProperty(transitionColor, customSkin, "Transition Color");
                    }

                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-2);
                    useHoverEffect.boolValue = DreamUIEditorHandler.DrawTogglePlain(useHoverEffect.boolValue, customSkin, "Use Hover Effect");
                    GUILayout.Space(4);

                    if (useHoverEffect.boolValue == true)
                    {
                        DreamUIEditorHandler.DrawProperty(heShape, customSkin, "Shape");
                        DreamUIEditorHandler.DrawProperty(heSpeed, customSkin, "Transition Speed");
                        DreamUIEditorHandler.DrawProperty(heSize, customSkin, "Shape Size");
                        DreamUIEditorHandler.DrawProperty(heTransitionAlpha, customSkin, "Transition Alpha");
                    }

                    GUILayout.EndVertical();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            this.Repaint();
        }
    }
}
#endif