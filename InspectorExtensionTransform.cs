using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(Transform))]
	[CanEditMultipleObjects]
	public class InspectorExtensionTransform : InternalEditorExtensionAbstract<Transform>
	{
		protected override string GetType()
		{
			return "UnityEditor.TransformInspector";
		}

		protected override void InspectorGUI()
		{
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.LabelField("World (Read Only)");
				EditorGUILayout.Vector3Field("Position", targetComponent.position);
				EditorGUILayout.Vector3Field("Rotation", targetComponent.rotation.eulerAngles);
				EditorGUILayout.Vector3Field("Scale", targetComponent.lossyScale);
			}
			
			using (new GUILayout.HorizontalScope())
			{
				var compositeScale = targetComponent.localScale.x;
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					compositeScale = EditorGUILayout.FloatField("Scale", compositeScale);
					if (check.changed)
					{
						foreach (var obj in Selection.gameObjects)
							obj.transform.localScale = Vector3.one * compositeScale;
					}
				}
			}

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Reset");
				if (GUILayout.Button("All", EditorStyles.toolbarButton))
				{
					foreach (var obj in Selection.gameObjects)
					{
						obj.transform.localPosition = Vector3.zero;
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = Vector3.one;
					}
				}

				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_MoveTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
						obj.transform.localPosition = Vector3.zero;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_RotateTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects) 
						obj.transform.localRotation = Quaternion.identity;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_ScaleTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
						obj.transform.localScale = Vector3.one;
			}
		}
	}
}