using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(Transform))]
	public class InspectorExtensionTransform : Editor
	{
		private Editor _transformEditor;

		private void OnEnable()
		{
			var type = typeof(EditorApplication).Assembly.GetType("UnityEditor.TransformInspector");
			var transform = target as Transform;
			_transformEditor = CreateEditor(transform, type);
		}

		private void OnDisable()
		{
			DestroyImmediate(_transformEditor);
		}

		public override void OnInspectorGUI()
		{
			_transformEditor.OnInspectorGUI();

			var transform = target as Transform;
			GUI.enabled = false;
			EditorGUILayout.LabelField("World (Read Only)");
			EditorGUILayout.Vector3Field("Position", transform.position);
			EditorGUILayout.Vector3Field("Rotation", transform.rotation.eulerAngles);
			EditorGUILayout.Vector3Field("Scale", transform.lossyScale);
			GUI.enabled = true;
			using (new GUILayout.HorizontalScope())
			{
				var compositeScale = transform.lossyScale.x;
				EditorGUI.BeginChangeCheck();
				compositeScale = EditorGUILayout.FloatField("Scale", compositeScale);
				if (EditorGUI.EndChangeCheck())
				{
					var scale = transform.localScale;
					scale.x = compositeScale;
					scale.y = compositeScale;
					scale.z = compositeScale;
					transform.localScale = scale;
				}
			}

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Reset");
				if (GUILayout.Button("All", GUILayout.Height(23)))
				{
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					transform.localScale = Vector3.one;
				}

				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_MoveTool.png") as Texture2D)))
					transform.localPosition = Vector3.zero;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_RotateTool.png") as Texture2D)))
					transform.localRotation = Quaternion.identity;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_ScaleTool.png") as Texture2D)))
					transform.localScale = Vector3.one;
			}
		}
	}
}