using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ZhwScreenCapture : MonoBehaviour {

    public RawImage a;
    public RawImage b;

    Texture2D aTex,bTex;
	void Start () {
        aTex = a.texture as Texture2D;
        bTex = b.texture as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            //ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/screenShot.png", 0);
            TwoToOne(aTex, bTex);
        }
    }

    public Texture2D TwoToOne(Texture2D source,Texture2D target)
    {
        int startWidth = source.width / 2 - target.width / 2;
        int startHeight = 35;

        source.SetPixels32(startWidth, startHeight, target.width, target.height, target.GetPixels32());
        source.Apply();

        string path = Application.streamingAssetsPath + "/zhwCombine.png";
        File.WriteAllBytes(path, source.EncodeToPNG());
        return source;
    }
}
