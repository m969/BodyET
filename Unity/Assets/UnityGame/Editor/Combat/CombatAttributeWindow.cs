namespace UnityGame.Combat
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Sirenix.OdinInspector.Editor;
	using Sirenix.OdinInspector;
	using Sirenix.Utilities.Editor;
	using Sirenix.Utilities;
    using System;
    using System.Linq;

    public class CombatAttributeWindow : OdinEditorWindow
	{
		[EnumToggleButtons]
		public ViewTool SomeField;

		public List<AttributeConfig> AttributeConfigs = new List<AttributeConfig>();
		[Button("+")]
		private void AddAttributeConfig()
		{
			var arr = System.DateTime.Now.Ticks.ToString().Reverse();
			AttributeConfigs.Add(new AttributeConfig() { Guid = string.Concat(arr) });
		}

		public List<StateConfig> StateConfigs = new List<StateConfig>();
		[Button("+")]
		private void AddStateConfig()
		{
			var arr = System.DateTime.Now.Ticks.ToString().Reverse();
			StateConfigs.Add(new StateConfig() { Guid = string.Concat(arr) });
		}



		[MenuItem("Tools/UnityGame/战斗属性编辑界面")]
		private static void ShowWindow()
		{
			var window = GetWindowWithRect<CombatAttributeWindow>(new Rect(0, 0, 800, 600), true, "战斗属性编辑界面");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
		}

        protected override void OnGUI()
        {
			base.OnGUI();


        }
    }

	[Serializable]
	public class AttributeConfig
    {
		[ToggleGroup("Enable", "@AliasName")]
		public bool Enable;
		[ToggleGroup("Enable")]
		[LabelText("属性名")]
		public string AttributeName = "NewAttribute";
		[ToggleGroup("Enable")]
		[LabelText("属性别名")]
		public string AliasName = "NewAttribute";
		[HideInInspector]
		public string Guid;
	}

	[Serializable]
	public class StateConfig
	{
		[ToggleGroup("Enable", "@AliasName")]
		public bool Enable;
		[ToggleGroup("Enable")]
		[LabelText("状态名")]
		public string StateName = "NewState";
		[ToggleGroup("Enable")]
		[LabelText("状态别名")]
		public string AliasName = "NewState";
		[HideInInspector]
		public string Guid;
	}
}