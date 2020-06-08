using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace CMF
{
	//This editor script displays some additional information in the mover inspector, like a preview of the current raycast array;
	[CustomEditor(typeof(Mover))]
	public class MoverInspector : Editor {

		private Mover mover;

		void Reset()
		{
			Setup();
		}

		void OnEnable()
		{
			Setup();
		}

		void Setup()
		{
			//Get reference to mover component;
			mover = (Mover)target;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			DrawRaycastArrayPreview();
		}

		//Draw preview of raycast array in inspector;
		void DrawRaycastArrayPreview()
		{
			if(mover.sensorType == Sensor.CastType.RaycastArray)
			{
				Rect _space;
				GUILayout.Space(5);

				_space = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(100));

				Rect background = new Rect(_space.x + (_space.width - _space.height)/2f, _space.y, _space.height, _space.height);
				EditorGUI.DrawRect(background, Color.grey);

				float point_size = 3f;

				Vector3[] _previewPositions = mover.raycastArrayPreviewPositions;

				Vector2 center = new Vector2(background.x + background.width/2f, background.y + background.height/2f);

				if(_previewPositions != null && _previewPositions.Length != 0)
				{
					for(int i = 0; i < _previewPositions.Length; i++)
					{
						Vector2 position = center + new Vector2(_previewPositions[i].x, _previewPositions[i].z) * background.width/2f * 0.9f;

						EditorGUI.DrawRect(new Rect(position.x - point_size/2f, position.y - point_size/2f, point_size, point_size), Color.white);
					}
				}

				if(_previewPositions != null && _previewPositions.Length != 0)
					GUILayout.Label("Number of rays = " + _previewPositions.Length, EditorStyles.centeredGreyMiniLabel );
			}
		}

		
	}
}