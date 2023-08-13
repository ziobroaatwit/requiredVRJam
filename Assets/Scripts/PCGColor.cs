using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGColor : MonoBehaviour
{
    public Material targetMaterial;
    // Start is called before the first frame update
    void Start()
    {
        targetMaterial = GetComponent<Renderer>().material;
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        targetMaterial.color = randomColor;
    }
}
