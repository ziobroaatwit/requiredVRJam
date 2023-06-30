using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public int score = 0;
    public TextMeshPro mText;

    void OnGUI()
    {
        GUIStyle scoreStyle = new GUIStyle();
        scoreStyle.fontSize = 24;
        scoreStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 200, 50), "Score: " + score, scoreStyle);
        mText.text = ("Score: "+score);
    }

}
