using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourLineScript : MonoBehaviour
{
    public string myTag;

    void Start()
    {
        transform.tag = myTag;
    }
}
