using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Linq;

public static class AdjustAnchorExtention
{
	[MenuItem("Util/AdjustAnchor")]
	static void FitAnchorsToTransform()
	{
		var rectTransforms = from gameObject in Selection.gameObjects
		                     where gameObject.GetComponent<RectTransform>() != null
		                     select gameObject.GetComponent<RectTransform>();

		if (rectTransforms == null || rectTransforms.Count() <= 0)
		{
			Debug.Log("No RectTransform is selected.");
			return;
		}

		Undo.RecordObjects(rectTransforms.ToArray(), "Fit Anchors");

		foreach (var item in rectTransforms)
			FitAnchorsToTransform(item);
	}

	static void FitAnchorsToTransform(RectTransform rectTransform)
	{
		var parent = rectTransform.parent as RectTransform;
		if (parent == null)
            return;

        var s = rectTransform.localScale.x + rectTransform.localScale.y;
        if (s > 2.1f || s < 1.9f)
        {
            Debug.LogError("UGUIオブジェクトのスケールが1じゃない");
            return;
        }


        var parentWidth = parent.rect.width;
        var parentHeight = parent.rect.height;

        var x = rectTransform.localPosition.x + parentWidth * 0.5f;
        var y = rectTransform.localPosition.y + parentHeight * 0.5f;
        var w = rectTransform.rect.width;
        var h = rectTransform.rect.height;
        
        var lx = x - w / 2;
        var ly = y + h / 2;
        var rx = x + w / 2;
        var ry = y - h / 2;
        var min = new Vector2(lx / parentWidth, ly / parentHeight);
        var max = new Vector2(rx / parentWidth, ry / parentHeight);
        
        rectTransform.anchorMin = new Vector2(min.x, max.y);
        rectTransform.anchorMax = new Vector2(max.x, min.y);

        // アンカーからの距離はゼロに設定
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

    }
}