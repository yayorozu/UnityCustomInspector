using UnityEditor;
using UnityEngine;

namespace Yorozu
{
	[CustomPropertyDrawer(typeof(Color))]
	public class ColorPropertyDrawer : PropertyDrawer
	{
		private const int ColorMax = 4;
		private SerializedProperty[] _rgba;
		private Color _color = new Color();
		private bool _isCached;
		private string _colorStr;
		private static readonly string[] Labels = {
			"Red", "Green", "Blue", "Alpha"
		};

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!_isCached)
			{
				_rgba = new SerializedProperty[ColorMax];
				_rgba[0] = property.FindPropertyRelative("r");
				_rgba[1] = property.FindPropertyRelative("g");
				_rgba[2] = property.FindPropertyRelative("b");
				_rgba[3] = property.FindPropertyRelative("a");
				_isCached = true;
			}

			position.height = EditorGUIUtility.singleLineHeight;
			
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				for (var i = 0; i < ColorMax; i++)
					_color[i] = _rgba[i].floatValue;
				
				_color = EditorGUI.ColorField(position, label, _color);
				
				if (check.changed)
					for (var i = 0; i < ColorMax; i++)
						_rgba[i].floatValue = _color[i];
			}

			property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
			if (!property.isExpanded)
				return;

			using (new EditorGUI.IndentLevelScope(1))
			{
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					for (var i = 0; i < ColorMax; i++)
					{
						position.y += EditorGUIUtility.singleLineHeight;
						_color[i] = _rgba[i].floatValue;
						_color[i] = EditorGUI.Slider(position, Labels[i], _color[i], 0f, 1f);
					}
					if (check.changed)
						for (var i = 0; i < ColorMax; i++)
							_rgba[i].floatValue = _color[i];
				}

				position.y += EditorGUIUtility.singleLineHeight;
				using (var check = new EditorGUI.ChangeCheckScope())
				{
					for (var i = 0; i < ColorMax; i++)
						_color[i] = _rgba[i].floatValue;
					
					_colorStr = EditorGUI.DelayedTextField(position, "HTML Color Code", ColorUtility.ToHtmlStringRGB(_color));
					if (check.changed)
						if (ColorUtility.TryParseHtmlString("#" + _colorStr, out var color))
							for (var i = 0; i < ColorMax - 1; i++)
								_rgba[i].floatValue = color[i];
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
				return EditorGUIUtility.singleLineHeight;
			
			return EditorGUIUtility.singleLineHeight * 6;
		}
	}
}