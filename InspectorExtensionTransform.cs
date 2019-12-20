using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(Transform))]
	[CanEditMultipleObjects]
	public class InspectorExtensionTransform : Editor
	{
		private Editor _transformEditor;
		private Transform _transform;

		private void OnEnable()
		{
			var type = typeof(EditorApplication).Assembly.GetType("UnityEditor.TransformInspector");
			_transform = target as Transform;
			_transformEditor = CreateEditor(_transform, type);
		}

		private void OnDisable()
		{
			DestroyImmediate(_transformEditor);
		}

		public override void OnInspectorGUI()
		{
			_transformEditor.OnInspectorGUI();

			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.LabelField("World (Read Only)");
				EditorGUILayout.Vector3Field("Position", _transform.position);
				EditorGUILayout.Vector3Field("Rotation", _transform.rotation.eulerAngles);
				EditorGUILayout.Vector3Field("Scale", _transform.lossyScale);
			}
			
			using (new GUILayout.HorizontalScope())
			{
				var compositeScale = _transform.localScale.x;
				EditorGUI.BeginChangeCheck();
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