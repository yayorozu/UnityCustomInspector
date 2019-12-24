using UnityEditor;
using UnityEngine;

namespace UniLib
{
	[CustomPropertyDrawer(typeof(Color))]
	public class ColorPropertyDrawer : PropertyDrawer
	{
		private SerializedProperty _red, _green, _blue, _alpha;
		private Color _color;
		private bool _cache;
		private string _colorStr;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!_cache)
			{
				property.Next(true);
				_red = property.Copy();
				property.Next(true);
				_green = property.Copy();
				property.Next(true);
				_blue = property.Copy();
				property.Next(true);
				_alpha = property.Copy();
				_cache = true;
			}

			position.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(position, property.displayName);

			using (var check = new EditorGUI.ChangeCheckScope())
			{
				_color = EditorGUI.ColorField(
					new Rect(position.width / 2 + 5, position.y, position.width / 2 + 5, EditorGUIUtility.singleLineHeight),
					new Color(_red.floatValue, _green.floatValue, _blue.floatValue, _alpha.floatValue));
				if (check.changed)
				{
					_red.floatValue = _color.r;
					_green.floatValue = _color.g;
					_blue.floatValue = _color.b;
					_alpha.floatValue = _color.a;
				}
			}

			property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
			if (!property.isExpanded)
				return;

			using (new EditorGUI.IndentLevelScope(1))
			{
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					var r = _red.floatValue;
					r = EditorGUI.Slider(position, "red", r, 0f, 1f);
					position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					var g = _green.floatValue;
					g = EditorGUI.Slider(position, "green", g, 0f, 1f);
					position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					var b = _blue.floatValue;
					b = EditorGUI.Slider(position, "blue", b, 0f, 1f);
					if (check.changed)
					{
						_red.floatValue = r;
						_green.floatValue = g;
						_blue.floatValue = b;
					}
				}
				
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					var a = _alpha.floatValue;
					a = EditorGUI.Slider(position, "alpha", a, 0f, 1f);
					if (check.changed)
					{
						_alpha.floatValue = a;
					}
				}
				using (new EditorGUI.DisabledScope())
				{
					position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
					using (var check = new EditorGUI.ChangeCheckScope())
					{
						_colorStr = EditorGUI.TextField(position, "Color Code", ColorUtility.ToHtmlStringRGB(new Color(_red.floatValue, _green.floatValue, _blue.floatValue, _alpha.floatValue)));
						if (check.changed)
						{
							if (ColorUtility.TryParseHtmlString("#" + _colorStr, out var color))
							{
								_red.floatValue = color.r;
								_green.floatValue = color.g;
								_blue.floatValue = color.b;
							}
						}
					}
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
				return EditorGUIUtility.singleLineHeight;
			
			return EditorGUIUtility.singleLineHeight * 6 + EditorGUIUtility.standardVerticalSpacing * 5;
		}
	}
}