using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// UGUIのアンカーをオブジェクトサイズに合わせる。
/// ただし、localScaleの値がxy共に1のときのみ
/// </summary>
public static class AdjustUGUIAnchorExtention
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

        if(Mathf.RoundToInt(rectTransform.localScale.x * 1000f) != 1000 || Mathf.RoundToInt(rectTransform.localScale.y * 1000f) != 1000)
        {
            Debug.LogError("UGUIオブジェクトのlocalScaleが1じゃない");
            return;
        }

        var canvasWidth = parent.rect.width;
        var canvasHeight = parent.rect.height;

        var x = rectTransform.localPosition.x + canvasWidth * 0.5f;
        var y = rectTransform.localPosition.y + canvasHeight * 0.5f;
        var w = rectTransform.rect.width;
        var h = rectTransform.rect.height;
        
        var lx = x - w / 2;
        var ly = y + h / 2;
        var rx = x + w / 2;
        var ry = y - h / 2;
        var min = new Vector2(lx / canvasWidth, ly / canvasHeight);
        var max = new Vector2(rx / canvasWidth, ry / canvasHeight);
        
        rectTransform.anchorMin = new Vector2(min.x, max.y);
        rectTransform.anchorMax = new Vector2(max.x, min.y);

        // アンカーからの距離はゼロに設定
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}