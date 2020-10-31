#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;

#endif

    // Example demonstrating how to create a custom drawer for an attribute.
    [TypeInfoBox("Here a visualization of a health bar being drawn with with a custom attribute drawer.")]
    public class HealthBarExample : MonoBehaviour
    {
        [HealthBar(100)]
        public float Health;

        [HealthPointBar]
        public HealthPoint HealthPoint;
    }

    [Serializable]
    public class HealthPoint
    {
        private int value = 100;
        [ShowInInspector]
        public int Value 
        { 
            get
            {
                return value;
            }
            set
            {
                if (value > Max)
                {
                    value = Max;
                }
                if (value < 0)
                {
                    return;
                }
                this.value = value;
            }
        }
        private int max = 100;
        [ShowInInspector]
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value < Value)
                {
                    Value = value;
                }
                if (value < 0)
                {
                    return;
                }
                this.max = value;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HealthPointBarAttribute : Attribute
    {
        public HealthPointBarAttribute()
        {
        }
    }

    // Attribute used by HealthBarAttributeDrawer.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HealthBarAttribute : Attribute
    {
        public float MaxHealth { get; private set; }

        public HealthBarAttribute(float maxHealth)
        {
            this.MaxHealth = maxHealth;
        }
    }

#if UNITY_EDITOR

    // Place the drawer script file in an Editor folder or wrap it in a #if UNITY_EDITOR condition.
    public class HealthBarAttributeDrawer : OdinAttributeDrawer<HealthBarAttribute, float>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {

            // Get a rect to draw the health-bar on. You Could also use GUILayout instead, but using rects makes it simpler to draw the health bar.
            Rect rect = EditorGUILayout.GetControlRect();

            // Draw the health bar.
            float width = Mathf.Clamp01(this.ValueEntry.SmartValue / this.Attribute.MaxHealth);
            SirenixEditorGUI.DrawSolidRect(rect, new Color(0f, 0f, 0f, 0.3f), false);
            SirenixEditorGUI.DrawSolidRect(rect.SetWidth(rect.width * width), Color.red, false);
            SirenixEditorGUI.DrawBorders(rect, 1);

            // Call the next drawer, which will draw the float field.
            this.CallNextDrawer(label);

        }
    }

    public class HealthPointBarAttributeDrawer : OdinAttributeDrawer<HealthPointBarAttribute, HealthPoint>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {

            // Get a rect to draw the health-bar on. You Could also use GUILayout instead, but using rects makes it simpler to draw the health bar.
            Rect rect = EditorGUILayout.GetControlRect();

            // Draw the health bar.
            if (this.ValueEntry.SmartValue.Max > 0)
            {
                float width = Mathf.Clamp01(this.ValueEntry.SmartValue.Value / (float)this.ValueEntry.SmartValue.Max);
                SirenixEditorGUI.DrawSolidRect(rect, new Color(0f, 0f, 0f, 0.3f), false);
                SirenixEditorGUI.DrawSolidRect(rect.SetWidth(rect.width * width), Color.green, false);
                SirenixEditorGUI.DrawBorders(rect, 1);
            }

            // Call the next drawer, which will draw the float field.
            this.CallNextDrawer(label);

        }
    }

#endif
}
#endif
