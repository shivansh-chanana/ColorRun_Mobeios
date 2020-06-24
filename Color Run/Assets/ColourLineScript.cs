using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourLineScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public string myTag;
    [SerializeField] List<Color> colourList;
    [SerializeField] int currentColour;

    void Start()
    {
        colourList =  ColourBallManager.instance.colourList;
        currentColour = Random.Range(0, colourList.Count);
        transform.tag = myTag;
        InvokeRepeating("ColourChangerCoroutine" , 0f,0.2f);
    }

    void ColourChangerCoroutine() {
        meshRenderer.material.color = colourList[currentColour];
        currentColour++;
        if (currentColour >= colourList.Count) currentColour = 0;
    }
}
