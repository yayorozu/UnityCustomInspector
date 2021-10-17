using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CanvasRenderer))]
public class InspectorExtensionCanvasRenderer : Editor
{
	private int? _orderInLayer;

	private void OnEnable()
	{
		var c =  target as CanvasRenderer;
		_orderInLayer = GetOrderInLayer(c.transform);
	}

	private int? GetOrderInLayer(Transform transform)
	{
		while (true)
		{
			if (transform == null)
				return null;

			var canvas = transform.GetComponent<Canvas>();
			if (canvas == null)
			{
				transform = transform.transform.parent;
				continue;
			}

			if (canvas.overrideSorting)
			{
				return canvas.sortingOrder;
			}

			if (canvas == canvas.rootCanvas)
				return canvas.sortingOrder;
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (_orderInLayer.HasValue)
		{
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUILayout.IntField("Order In Layer", _orderInLayer.Value);
			}
		}
	}
}
