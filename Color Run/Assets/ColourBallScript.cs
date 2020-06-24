using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourBallScript : MonoBehaviour
{
    public Material ballMat;
    public MeshRenderer meshRenderer;
    public string myTag;

    private void Start()
    {
        meshRenderer.material = ballMat;
        gameObject.tag = myTag;
    }
}
