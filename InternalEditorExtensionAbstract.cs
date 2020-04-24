using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yorozu
{
	public abstract class InternalEditorExtensionAbstract<T> : Editor where T : Component
	{
		private Editor _editor;
		protected T component;
		protected T[] components;
		protected abstract string GetTypeName();
		
		private void OnEnable()
		{
			var type = typeof(EditorApplication).Assembly.GetType(GetTypeName());
			component = target as T;
			components = targets.Cast<T>().ToArray();
			_editor = CreateEditor(targets, type);
			Enable();
		}

		private void OnDisable()
		{
			DestroyImmediate(_editor);
			Disable();
		}
		
		public sealed override void OnInspectorGUI()
		{
			_editor.OnInspectorGUI();
			InspectorGUI();
		}
		
		protected virtual void Enable()
		{
		}
		
		protected virtual void Disable()
		{
		}

		protected virtual void InspectorGUI()
		{
		}
	}
}