using UnityEngine;
using UnityEditor;

namespace Yorozu
{
	[CustomEditor(typeof(RectTransform))]
	[CanEditMultipleObjects]
	public class InspectorExtensionRectTransform : InternalEditorExtensionAbstract<RectTransform>
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
		
		protected override string GetTypeName()
		{
			return "UnityEditor.RectTransformEditor";
		}

		protected override void InspectorGUI()
		{
			if (_Contents == null)
				_Contents = new Contents();
			
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.LabelField("Debug");
				EditorGUILayout.Vector3Field("Position", component.transform.position);
				EditorGUILayout.Vector3Field("Rotation", component.transform.rotation.eulerAngles);
				EditorGUILayout.Vector3Field("Scale", component.transform.lossyScale);
				EditorGUILayout.Vector3Field("LocalPosition", component.transform.localPosition);
				EditorGUILayout.Vector2Field("Anchored Position", component.anchoredPosition);
				EditorGUILayout.Vector2Field("SizeDelta", component.sizeDelta);
				EditorGUILayout.RectField("Rect", component.rect);
			}

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Reset");
				if (GUILayout.Button("All", EditorStyles.toolbarButton))
				{
					foreach (var t in targets)
					{
						((RectTransform) t).localPosition = Vector3.zero;
						((RectTransform) t).localRotation = Quaternion.identity;
						((RectTransform) t).localScale = Vector3.one;
					}
				}
				
				if (GUILayout.Button(_Contents.PositionResetButton, EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localPosition = Vector3.zero;
				if (GUILayout.Button(_Contents.RotationResetButton, EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localRotation = Quaternion.identity;
				if (GUILayout.Button(_Contents.ScaleResetButton, EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localScale = Vector3.one;
			}
		}
	}
}