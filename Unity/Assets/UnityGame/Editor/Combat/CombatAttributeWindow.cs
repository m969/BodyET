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

	public class CombatAttributeWindow : OdinEditorWindow
	{
		[EnumToggleButtons]
		public ViewTool SomeField;


		[MenuItem("Tools/UnityGame/战斗属性编辑界面")]
		private static void ShowWindow()
		{
			var window = GetWindowWithRect<CombatAttributeWindow>(new Rect(0, 0, 800, 600), true, "战斗属性编辑界面");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
		}

		//private void OnGUI()
		//{
		//}
	}
}