using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCollection : MonoBehaviour
{
    public static ColorCollection access;
    public string camColor;
    public Color green;
    public Color darkgreen;
    public Color BGgreen;
    public Color Texgreen;
    public Color red;
    public Color darkred;
    public Color BGred;
    public Color Texred;
    public Color blue;
    public Color darkblue;
    public Color BGblue;
    public Color Texblue;
    public Color purple;
    public Color darkpurple;
    public Color BGpurple;
    public Color Texpurple;
    public Color orange;
    public Color darkorange;
    public Color BGorange;
    public Color Texorange;
    public Color yellow;
    public Color darkyellow;
    public Color BGyellow;
    public Color Texyellow;
    public Color grey;
    public Color white;
    public Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
    public List<string> colorList = new List<string>();
    void Awake()
    {
        access = this;
        colorDict.Add("green", green);
        colorList.Add("green");
        colorDict.Add("darkgreen", darkgreen);
        colorDict.Add("Texgreen", Texgreen);
        colorDict.Add("BGgreen", BGgreen);
        colorDict.Add("red", red);
        colorList.Add("red");
        colorDict.Add("darkred", darkred);
        colorDict.Add("BGred", BGred);
        colorDict.Add("Texred", Texred);
        colorDict.Add("blue", blue);
        colorList.Add("blue");
        colorDict.Add("darkblue", darkblue);
        colorDict.Add("BGblue", BGblue);
        colorDict.Add("Texblue", Texblue);
        colorDict.Add("purple", purple);
        colorList.Add("purple");
        colorDict.Add("darkpurple", darkpurple);
        colorDict.Add("BGpurple", BGpurple);
        colorDict.Add("Texpurple", Texpurple);
        colorDict.Add("orange", orange);
        colorList.Add("orange");
        colorDict.Add("darkorange", darkorange);
        colorDict.Add("BGorange", BGorange);
        colorDict.Add("Texorange", Texorange);
        colorDict.Add("yellow", yellow);
        colorList.Add("yellow");
        colorDict.Add("darkyellow", darkyellow);
        colorDict.Add("BGyellow", BGyellow);
        colorDict.Add("Texyellow", Texyellow);
        colorDict.Add("white", white);
    }

}
