using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page
{
    private Texture2D whiteCanvas;
    public int TexWidth  { get { return whiteCanvas.width; } }
    public int TexHeight { get { return whiteCanvas.height; } }

    public Page()
    {
        whiteCanvas = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Clear();
    }

    public void WritePixel(int pixelX, int pixelY, Color color)
    {
        whiteCanvas.SetPixel(pixelX, pixelY, color);
        whiteCanvas.Apply();
    }
    public void WriteSquare(int pixelX, int pixelY, int width, int height, Color[] colors)
    {
        whiteCanvas.SetPixels(pixelX, pixelY, width, height, colors);
        whiteCanvas.Apply();
    }
    public void WriteCircleNotApply(int x, int y, Color color, int radius)
    {
        var center = new Vector2(x, y);
        int r = radius;
        for (int iy = -r; iy < r; iy++)
        {
            for (int ix = -r; ix < r; ix++)
            {
                if (ix * ix + iy * iy < r * r)
                {
                    var tx = x + ix;
                    var ty = y + iy;
                    if (tx >= 0 && ty >= 0 && tx < TexWidth && ty < TexHeight)
                        whiteCanvas.SetPixel(x + ix, y + iy, color);
                }
            }
        }
    }
    public void WriteCircle(int x, int y, Color color, int radius)
    {
        WriteCircleNotApply(x, y, color, radius);
        whiteCanvas.Apply();
    }
    public void Clear()
    {
        for (int x = 0; x < Screen.width; ++x)
        {
            for (int y = 0; y < Screen.height; ++y)
                whiteCanvas.SetPixel(x, y, Color.white);
        }
        whiteCanvas.Apply();
    }
    public void SetCanvas(RawImage rawImage)
    {
        rawImage.texture = whiteCanvas;
    }
}
