using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniLib
{
	public abstract class InternalEditorExtensionAbstract<T> : Editor where T : Component
	{
		private Editor _editor;
		protected T targetComponent;

		protected abstract string GetType();
		
		private void OnEnable()
		{
			var type = typeof(EditorApplication).Assembly.GetType(GetType());
			targetComponent = target as T;
			_editor = CreateEditor(targetComponent, type);
			Enable();
		}

		private void OnDisable()
		{
			DestroyImmediate(_editor);
			Disable();
		}
		
		public sealed override void OnInspectorGUI()
		{
			using (var check = new EditorGUI.ChangeCheckScope())
			{
				_editor.OnInspectorGUI();
				if (check.changed && targets.Length > 1)
				{
					ComponentUtility.CopyComponent(targetComponent);
					foreach (var o in targets)
						ComponentUtility.PasteComponentValues(o as T);
				}
			}
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