using UnityEditor;
using UnityEngine;

namespace UniLib.UniAttribute
{
	[CustomPropertyDrawer(typeof(Color))]
	public class ColorPropertyDrawer : PropertyDrawer
	{
		private SerializedProperty _r, _g, _b, _a;
		private Color _color;
		private bool _cache;
		private string _colorStr;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!_cache)
			{
				property.Next(true);
				_r = property.Copy();
				property.Next(true);
				_g = property.Copy();
				property.Next(true);
				_b = property.Copy();
				property.Next(true);
				_a = property.Copy();
				_cache = true;
			}

			position.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(position, property.displayName);

			EditorGUI.BeginChangeCheck();
			_color = EditorGUI.ColorField(
				new Rect(position.width / 2 + 5, position.y, position.width / 2 + 5, EditorGUIUtility.singleLineHeight),
				new Color(_r.floatValue, _g.floatValue, _b.floatValue, _a.floatValue));
			if (EditorGUI.EndChangeCheck())
			{
				_r.floatValue = _color.r;
				_g.floatValue = _color.g;
				_b.floatValue = _color.b;
				_a.floatValue = _color.a;
			}

			property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
			if (!property.isExpanded)
				return;
			
			EditorGUI.indentLevel++;
			{
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				EditorGUI.Slider(position, _r, 0f, 1f, "red");
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				EditorGUI.Slider(position, _g, 0f, 1f, "green");
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				EditorGUI.Slider(position, _b, 0f, 1f, "blue");
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				EditorGUI.Slider(position, _a, 0f, 1f, "alpha");
			}

			using (new EditorGUI.DisabledScope())
			{
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				EditorGUI.BeginChangeCheck();
				_colorStr = EditorGUI.TextField(position, "Color Code",
					ColorUtility.ToHtmlStringRGB(new Color(_r.floatValue, _g.floatValue, _b.floatValue, _a.floatValue)));
				if (EditorGUI.EndChangeCheck())
				{
					var c = Color.white;
					if (ColorUtility.TryParseHtmlString("#" + _colorStr, out c))
					{
						_r.floatValue = c.r;
						_g.floatValue = c.g;
						_b.floatValue = c.b;
					}
				}
			}

			EditorGUI.indentLevel--;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
				return EditorGUIUtility.singleLineHeight;
			
			return EditorGUIUtility.singleLineHeight * 6 + EditorGUIUtility.standardVerticalSpacing * 5;
		}
	}
}