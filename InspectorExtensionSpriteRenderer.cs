using UnityEditor;
using UnityEngine;

namespace Yorozu
{
	[CustomEditor(typeof(SpriteRenderer))]
	[CanEditMultipleObjects]
	public class InspectorExtensionSpriteRenderer : InternalEditorExtensionAbstract<SpriteRenderer>
	{
		protected override string GetTypeName()
		{
			return "UnityEditor.SpriteRendererEditor";
		}


		private Color _color = new Color(); 
		private int _alpha;

		protected override void InspectorGUI()
		{
			if (targets.Length <= 1)
				return;
			
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				_color = EditorGUILayout.ColorField("MultiColor", _color);
				if (check.changed)
				{
					foreach (var sr in components)
					{
						var color = sr.color;
						for (var i = 0; i < 3; i++)
							color[i] = _color[i];
						sr.color = color;
					}
				}
			}
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				_alpha = EditorGUILayout.IntSlider("MultiAlpha", _alpha, 0, 255);
				if (check.changed)
				{
					foreach (var sr in components)
					{
						var color = sr.color;
						color.a = _alpha / 255f;
						sr.color = color;
					}
				}
			}
			
		}
	}
}