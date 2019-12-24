using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(RectTransform))]
	[CanEditMultipleObjects]
	public class InspectorExtensionRectTransform : InternalEditorExtensionAbstract<RectTransform>
	{
		protected override string GetType()
		{
			return "UnityEditor.RectTransformEditor";
		}

		protected override void InspectorGUI()
		{
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.LabelField("Read Only");
				EditorGUILayout.Vector3Field("Position", targetComponent.transform.position);
				EditorGUILayout.Vector3Field("Rotation", targetComponent.transform.rotation.eulerAngles);
				EditorGUILayout.Vector3Field("Scale", targetComponent.transform.lossyScale);
				EditorGUILayout.Vector3Field("LocalPosition", targetComponent.transform.localPosition);
				EditorGUILayout.Vector2Field("Anchored Position", targetComponent.anchoredPosition);
				EditorGUILayout.Vector2Field("SizeDelta", targetComponent.sizeDelta);
				EditorGUILayout.RectField("Rect", targetComponent.rect);
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

				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_MoveTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localPosition = Vector3.zero;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_RotateTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localRotation = Quaternion.identity;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_ScaleTool.png") as Texture2D), EditorStyles.toolbarButton))
					foreach (var t in targets)
						((RectTransform) t).localScale = Vector3.one;
			}

			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("↖", EditorStyles.toolbarButton))
					SetAnchor(RectAnchor.TopLeft, targetComponent);
				if (GUILayout.Button("↗", EditorStyles.toolbarButton))
					SetAnchor(RectAnchor.TopRight, targetComponent);
				if (GUILayout.Button("↙", EditorStyles.toolbarButton))
					SetAnchor(RectAnchor.BottomLeft, targetComponent);
				if (GUILayout.Button("↘", EditorStyles.toolbarButton))
					SetAnchor(RectAnchor.BottomRight, targetComponent);

				if (GUILayout.Button("↑↔", EditorStyles.toolbarButton))
					SetAnchorFit(RectAnchor.TopFit, targetComponent);
				if (GUILayout.Button("↓↔", EditorStyles.toolbarButton))
					SetAnchorFit(RectAnchor.BottomFit, targetComponent);
				if (GUILayout.Button("←↕", EditorStyles.toolbarButton))
					SetAnchorFit(RectAnchor.LeftFit, targetComponent);
				if (GUILayout.Button("→↕", EditorStyles.toolbarButton))
					SetAnchorFit(RectAnchor.RightFit, targetComponent);
				if (GUILayout.Button("↔↕", EditorStyles.toolbarButton))
				{
					var parent = targetComponent.parent as RectTransform;
					targetComponent.SetSizeDelta(parent.sizeDelta);
					targetComponent.SetAnchoredPosition(Vector2.zero);
				}
			}
		}

		private void SetAnchor(RectAnchor anchor, RectTransform rect)
		{
			if (rect.sizeDelta.x != 0f && rect.sizeDelta.y != 0f)
				rect.SetAnchor(RectAnchor.TopLeft);
			else if (rect.sizeDelta.x == 0f)
				rect.SetAnchor(anchor, rect.sizeDelta.y);
			else if (rect.sizeDelta.y == 0f)
				rect.SetAnchor(anchor, rect.sizeDelta.x);
			rect.SetAnchor(anchor, 100f);
		}

		private void SetAnchorFit(RectAnchor anchor, RectTransform rect)
		{
			if (anchor == RectAnchor.BottomFit || anchor == RectAnchor.TopFit)
				if (rect.sizeDelta.y == 0f)
				{
					rect.SetAnchor(anchor, 100f);
					return;
				}

			if (anchor == RectAnchor.LeftFit || anchor == RectAnchor.RightFit)
				if (rect.sizeDelta.x == 0f)
				{
					rect.SetAnchor(anchor, 100f);
					return;
				}

			rect.SetAnchor(anchor);
		}
	}
}