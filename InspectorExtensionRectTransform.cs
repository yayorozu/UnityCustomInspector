using UnityEngine;
using UnityEditor;

namespace UniLib
{
	[CustomEditor(typeof(RectTransform))]
	public class InspectorExtensionRectTransform : Editor
	{
		private Editor _rectTransformEditor;

		private void OnEnable()
		{
			var type = typeof(EditorApplication).Assembly.GetType("UnityEditor.RectTransformEditor");
			var rectTransform = target as RectTransform;
			_rectTransformEditor = CreateEditor(rectTransform, type);
		}

		private void OnDisable()
		{
			DestroyImmediate(_rectTransformEditor);
		}

		public override void OnInspectorGUI()
		{
			_rectTransformEditor.OnInspectorGUI();

			var rectTransform = target as RectTransform;
			GUI.enabled = false;
			EditorGUILayout.LabelField("Read Only");
			EditorGUILayout.Vector3Field("Position", rectTransform.transform.position);
			EditorGUILayout.Vector3Field("Rotation", rectTransform.transform.rotation.eulerAngles);
			EditorGUILayout.Vector3Field("Scale", rectTransform.transform.lossyScale);
			EditorGUILayout.Vector3Field("LocalPosition", rectTransform.transform.localPosition);
			EditorGUILayout.Vector2Field("Anchored Position", rectTransform.anchoredPosition);
			EditorGUILayout.Vector2Field("SizeDelta", rectTransform.sizeDelta);
			EditorGUILayout.RectField("Rect", rectTransform.rect);
			GUI.enabled = true;

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("Reset");
				if (GUILayout.Button("All", GUILayout.Height(23)))
				{
					rectTransform.localPosition = Vector3.zero;
					rectTransform.localRotation = Quaternion.identity;
					rectTransform.localScale = Vector3.one;
				}

				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_MoveTool.png") as Texture2D)))
					rectTransform.localPosition = Vector3.zero;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_RotateTool.png") as Texture2D)))
					rectTransform.localRotation = Quaternion.identity;
				if (GUILayout.Button(new GUIContent(EditorGUIUtility.Load("icons/d_ScaleTool.png") as Texture2D)))
					rectTransform.localScale = Vector3.one;
			}

			using (new GUILayout.HorizontalScope())
			{
				if (GUILayout.Button("↖"))
					SetAnchor(RectAnchor.TopLeft, rectTransform);
				if (GUILayout.Button("↗"))
					SetAnchor(RectAnchor.TopRight, rectTransform);
				if (GUILayout.Button("↙"))
					SetAnchor(RectAnchor.BottomLeft, rectTransform);
				if (GUILayout.Button("↘"))
					SetAnchor(RectAnchor.BottomRight, rectTransform);

				if (GUILayout.Button("↑↔"))
					SetAnchorFit(RectAnchor.TopFit, rectTransform);
				if (GUILayout.Button("↓↔"))
					SetAnchorFit(RectAnchor.BottomFit, rectTransform);
				if (GUILayout.Button("←↕"))
					SetAnchorFit(RectAnchor.LeftFit, rectTransform);
				if (GUILayout.Button("→↕"))
					SetAnchorFit(RectAnchor.RightFit, rectTransform);
				if (GUILayout.Button("↔↕"))
				{
					var parent = rectTransform.parent as RectTransform;
					rectTransform.SetSizeDelta(parent.sizeDelta);
					rectTransform.SetAnchoredPosition(Vector2.zero);
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