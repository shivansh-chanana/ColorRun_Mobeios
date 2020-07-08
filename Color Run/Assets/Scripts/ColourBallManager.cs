using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct colourStruct
{
    public string tagName;
    public Material material;
}

public class ColourBallManager : MonoBehaviour
{
    public GameObject colourBallPrefab;
    public GameObject colourLinePrefab;
    public bool spawnSpeedPad , spawnBomb;
    public float obstacle_Freq;
    public List<GameObject> Objects;
    public GameObject colourDoor;
    public int totalColours;
    public colourStruct[] colourData;
    [Space]
    public float zDistanceBetweenColourChanger;
    public float[] colourBallPositions;
    public bool shouldSpawnLine;

    float zDistanceBetweenColourChangerTemp;
    Transform finishLine;
    List<int> ball_colour = new List<int>();
    List<int> line_colour = new List<int>();
    List<int> door_colour = new List<int>();

    public List<Color> colourList;

    public static ColourBallManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        finishLine = GameObject.FindGameObjectWithTag("finishLine").transform;

        if(!shouldSpawnLine)CreateRandomBalls();
        else CreateColourLines();
    }

    void CreateRandomBalls()
    {
        zDistanceBetweenColourChangerTemp = zDistanceBetweenColourChanger;

        do
        {
            for (int i = 0; i < totalColours; i++)
            {
                ball_colour.Add(i);
                door_colour.Add(i);
            }

            int ball_colourLoopCount = ball_colour.Count;
            for (int i = 0; i < ball_colourLoopCount; i++)
            {
                int randomBallNum = ball_colour[Random.Range(0, ball_colour.Count)];
                ColourBallScript tempColourBall = Instantiate(colourBallPrefab).GetComponent<ColourBallScript>();
                tempColourBall.transform.position = new Vector3(colourBallPositions[i],
                    GameManager.instance.playerTransform.position.y, zDistanceBetweenColourChangerTemp);
                tempColourBall.ballMat = colourData[randomBallNum].material;
                tempColourBall.myTag = colourData[randomBallNum].tagName;

                ball_colour.Remove(randomBallNum);
                door_colour.Remove(randomBallNum);

                int randomDoorNum = door_colour[Random.Range(0, door_colour.Count)];

                ColourDoor tempDoor = Instantiate(colourDoor).GetComponent<ColourDoor>();
                tempDoor.transform.position = new Vector3(colourBallPositions[i],
                    GameManager.instance.playerTransform.position.y, zDistanceBetweenColourChangerTemp + zDistanceBetweenColourChanger / 3);
                tempDoor.doorMat = colourData[randomDoorNum].material;
                tempDoor.myTag = colourData[randomDoorNum].tagName;

                door_colour.Remove(randomDoorNum);
                door_colour.Add(randomBallNum);
            }

            ball_colour.Clear();
            door_colour.Clear();

            zDistanceBetweenColourChangerTemp += zDistanceBetweenColourChanger;
        } while (zDistanceBetweenColourChangerTemp < finishLine.position.z);
    }

    void CreateColourLines() {
        zDistanceBetweenColourChangerTemp = zDistanceBetweenColourChanger;

        //which objects to spawn and which to not
        GameObject[] objectsTemp = new GameObject[Objects.Count];
        Objects.CopyTo(objectsTemp);
        if (!spawnSpeedPad) Objects.Remove(objectsTemp[0]);
        if (!spawnBomb) Objects.Remove(objectsTemp[1]);

        do
        {
            for (int i = 0; i < colourList.Count; i++)
            {
                line_colour.Add(i);
                door_colour.Add(i);
            }

            int line_colourLoopCount = line_colour.Count;

            int randomLineNum = line_colour[Random.Range(0, line_colour.Count)];
            ColourLineScript tempLineColour = Instantiate(colourLinePrefab).GetComponent<ColourLineScript>();
            tempLineColour.transform.position = new Vector3(0,0, zDistanceBetweenColourChangerTemp);
            tempLineColour.myTag = colourData[randomLineNum].tagName;

            int RandomCorrectDoorPos = randomLineNum;

            for (int i = 0; i < line_colourLoopCount; i++)
            {
                int randomDoorNum = door_colour[Random.Range(0, door_colour.Count)];
                if (i == RandomCorrectDoorPos) randomDoorNum = RandomCorrectDoorPos;

                ColourDoor tempDoor = Instantiate(colourDoor).GetComponent<ColourDoor>();
                tempDoor.transform.position = new Vector3(colourBallPositions[i],
                    GameManager.instance.playerTransform.position.y, zDistanceBetweenColourChangerTemp + zDistanceBetweenColourChanger / 3);
                tempDoor.doorMat = colourData[randomDoorNum].material;
                tempDoor.myTag = colourData[randomDoorNum].tagName;

                //door_colour.Remove(randomDoorNum);
            }

            line_colour.Clear();
            door_colour.Clear();

            //Create Objects if any
            if (Objects.Count > 0)
            {
                for (int i = 0; i < Random.Range(0, obstacle_Freq); i++)
                {
                    float currentSpawnPos = (zDistanceBetweenColourChangerTemp + zDistanceBetweenColourChanger / 3) + Random.Range(7, 9);
                    Debug.Log("about to spawn");
                    if (currentSpawnPos >= zDistanceBetweenColourChangerTemp + zDistanceBetweenColourChanger) break;
                    Transform spawnObj = Instantiate(Objects[Random.Range(0, Objects.Count)]).transform;
                    spawnObj.position = new Vector3(colourBallPositions[Random.Range(0, colourBallPositions.Length)],
                        GameManager.instance.playerTransform.position.y, currentSpawnPos);
                }
            }

            zDistanceBetweenColourChangerTemp += zDistanceBetweenColourChanger;
        } while (zDistanceBetweenColourChangerTemp < finishLine.position.z - zDistanceBetweenColourChanger);
    }
}
