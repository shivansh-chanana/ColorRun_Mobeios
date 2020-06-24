using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject buildingsPrefab;
    public Transform buildingParent;
    public Vector3 buildingScale;
    public Vector2 buildingYRandomLimit;
    public Vector2 zDistanceRandom;

    // Start is called before the first frame update
    void Start()
    {
        CreateBuildings();    
    }

    void CreateBuildings() {
        float currentPos = 0;
        float endLinePos = GameObject.FindGameObjectWithTag("finishLine").transform.position.z;
        do
        {
            for (float i = -buildingsPrefab.transform.localScale.x; i <= 6; i += buildingsPrefab.transform.localScale.x * 2)
            {
                Transform tempBuidling = Instantiate(buildingsPrefab).transform;
                tempBuidling.position = new Vector3(i,0,currentPos);
                tempBuidling.localScale = new Vector3(buildingScale.x, Random.Range(buildingYRandomLimit.x, buildingYRandomLimit.y), buildingScale.z);
                tempBuidling.parent = buildingParent;
            }
            currentPos += buildingsPrefab.transform.localScale.z + Random.Range(zDistanceRandom.x ,zDistanceRandom.y);
        } while (currentPos < endLinePos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
