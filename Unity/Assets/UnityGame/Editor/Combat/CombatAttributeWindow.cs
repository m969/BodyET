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
		//[Button("+")]
		private void AddAttributeConfig()
		{
			var arr = System.DateTime.Now.Ticks.ToString().Reverse();
			AttributeConfigs.Add(new AttributeConfig() { Guid = string.Concat(arr) });
		}

		public List<StateConfig> StateConfigs = new List<StateConfig>();
		//[Button("+")]
		private void AddStateConfig()
		{
			var arr = System.DateTime.Now.Ticks.ToString().Reverse();
			StateConfigs.Add(new StateConfig() { Guid = string.Concat(arr) });
		}


		[TableMatrix(HorizontalTitle = "Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 16)]
		public bool[,] CustomCellDrawing;

		//[ShowInInspector, DoNotDrawAsReference]
		//[TableMatrix(HorizontalTitle = "Transposed Custom Cell Drawing", DrawElementMethod = "DrawColoredEnumElement", ResizableColumns = false, RowHeight = 16, Transpose = true)]
		//public bool[,] Transposed { get { return CustomCellDrawing; } set { CustomCellDrawing = value; } }

		private static bool DrawColoredEnumElement(Rect rect, bool value)
		{
			if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
			{
				value = !value;
				GUI.changed = true;
				Event.current.Use();
			}

			UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));

			return value;
		}

		[Button("Set")]
		public void TransposeTableMatrixExample()
		{
			// =)
			this.CustomCellDrawing = new bool[15, 15];
			//this.CustomCellDrawing[6, 5] = true;
			//this.CustomCellDrawing[6, 6] = true;
			//this.CustomCellDrawing[6, 7] = true;
			//this.CustomCellDrawing[8, 5] = true;
			//this.CustomCellDrawing[8, 6] = true;
			//this.CustomCellDrawing[8, 7] = true;
			//this.CustomCellDrawing[5, 9] = true;
			//this.CustomCellDrawing[5, 10] = true;
			//this.CustomCellDrawing[9, 9] = true;
			//this.CustomCellDrawing[9, 10] = true;
			//this.CustomCellDrawing[6, 11] = true;
			//this.CustomCellDrawing[7, 11] = true;
			//this.CustomCellDrawing[8, 11] = true;
		}


		[MenuItem("Tools/UnityGame/战斗属性编辑界面")]
		private static void ShowWindow()
		{
			var window = GetWindowWithRect<CombatAttributeWindow>(new Rect(0, 0, 800, 600), true, "战斗属性编辑界面");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
		}

   //     protected override void OnGUI()
   //     {
			//base.OnGUI();
   //     }
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