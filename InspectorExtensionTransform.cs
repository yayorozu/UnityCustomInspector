using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(Transform))]
	[CanEditMultipleObjects]
	public class InspectorExtensionTransform : Editor
	{
		private class Contents
		{
			public readonly GUIContent PositionResetButton;
			public readonly GUIContent RotationResetButton = new GUIContent(EditorGUIUtility.Load("icons/d_RotateTool.png") as Texture2D);
			public readonly GUIContent ScaleResetButton = new GUIContent(EditorGUIUtility.Load("icons/d_ScaleTool.png") as Texture2D);
			
			private static readonly string[] IconNames = 
			{
				"icons/MoveTool.png",
				"icons/RotateTool.png",
				"icons/ScaleTool.png",
				// ProSkin
				"icons/d_MoveTool.png",
				"icons/d_RotateTool.png",
				"icons/d_ScaleTool.png",
			};
			
			public Contents()
			{
				var startIndex = EditorGUIUtility.isProSkin ? 3 : 0;
				PositionResetButton = new GUIContent(EditorGUIUtility.Load(IconNames[startIndex++]) as Texture2D);
				RotationResetButton = new GUIContent(EditorGUIUtility.Load(IconNames[startIndex++]) as Texture2D);
				ScaleResetButton = new GUIContent(EditorGUIUtility.Load(IconNames[startIndex++]) as Texture2D);
			}
		}

		private static Contents _Contents;
		private Type _transformRotationGUIType;
		private System.Object _transformRotationGUIObject;
		private MethodInfo _rotationFieldMethod;
		private SerializedProperty _positionProperty;
		private SerializedProperty _scaleProperty;
		private Transform _transform;
		
		private void OnEnable()
		{
			_transform = target as Transform;
			_positionProperty = serializedObject.FindProperty("m_LocalPosition");
			_scaleProperty = serializedObject.FindProperty("m_LocalScale");
			
			_transformRotationGUIType = typeof(EditorApplication).Assembly.GetType("UnityEditor.TransformRotationGUI");
			if (_transformRotationGUIObject == null)
				_transformRotationGUIObject = Activator.CreateInstance(_transformRotationGUIType);
			
			var enableMethod = _transformRotationGUIType
				.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
				.First(m => m.Name == "OnEnable");
			
			// Initialize TransformRotationGUI
			enableMethod.Invoke(_transformRotationGUIObject, new object[]
			{
				serializedObject.FindProperty("m_LocalRotation"),
				EditorGUIUtility.TrTextContent("Rotation")
			});
			
			_rotationFieldMethod = _transformRotationGUIType
				.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
				.Where(m => m.Name == "RotationField")
				.FirstOrDefault(m => m.GetParameters().Length == 0);
		}

		public override void OnInspectorGUI()
		{
			if (_Contents == null)
				_Contents = new Contents();
			
			EditorGUILayout.PropertyField(_positionProperty);
			
			_rotationFieldMethod.Invoke(_transformRotationGUIObject, null);
			
			EditorGUILayout.PropertyField(_scaleProperty);
			
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.LabelField("World (Read Only)");
				EditorGUILayout.Vector3Field("Position", _transform.position);
				EditorGUILayout.Vector3Field("Rotation", _transform.rotation.eulerAngles);
				EditorGUILayout.Vector3Field("Scale", _transform.lossyScale);
			}
			
			
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.PrefixLabel("Reset");
				if (GUILayout.Button("All", EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
					{
						obj.transform.localPosition = Vector3.zero;
						obj.transform.localRotation = Quaternion.identity;
						obj.transform.localScale = Vector3.one;
					}

				if (GUILayout.Button(_Contents.PositionResetButton, EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
						obj.transform.localPosition = Vector3.zero;
				if (GUILayout.Button(_Contents.RotationResetButton, EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
						obj.transform.localRotation = Quaternion.identity;
				if (GUILayout.Button(_Contents.ScaleResetButton, EditorStyles.toolbarButton))
					foreach (var obj in Selection.gameObjects)
						obj.transform.localScale = Vector3.one;
			}
		}
	}
}