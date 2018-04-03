using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PNGSave : MonoBehaviour {

	public static Texture2D LoadPNG(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(128, 128,TextureFormat.RGBA32, false);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			tex.wrapMode = TextureWrapMode.Clamp;
			tex.filterMode = FilterMode.Point;
//			for (int y = 0; y < tex.height; y++)
//			{
//				tex.SetPixel(0, y, Color.clear);
//				tex.SetPixel(tex.width, y, Color.clear);
//			}
//			for (int x = 0; x < tex.width; x++)
//			{
//				tex.SetPixel(x, tex.height, Color.clear);
//				tex.SetPixel(x, 0, Color.clear);
//			}
			tex.Apply ();
		}
		return tex;
	}
}
