using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct levelChanges {
    public int playerSpeed;
    public bool spawnSpeedPad;
    public bool spawnBomb;
    public float levelLength;
}

public class LevelManager : MonoBehaviour
{
    public int currentLevel;
    public PlayerScript playerScript;
    public levelChanges[] levelChanges;

    public static LevelManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);


            SoundManager.instance.PlayMusic();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 0;
        GameManager.instance.playButton.SetActive(true);
    }

    public void LevelLoaded(PlayerScript thisPlayer)
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel",0);
        GameManager.instance.curLevelText.text = currentLevel.ToString();
        GameManager.instance.nextLevelText.text = (currentLevel + 1).ToString();
        if (currentLevel > levelChanges.Length) currentLevel = levelChanges.Length - 2;
        playerScript = thisPlayer;
        playerScript.forwardSpeed = levelChanges[currentLevel].playerSpeed;
        ColourBallManager.instance.spawnSpeedPad = levelChanges[currentLevel].spawnSpeedPad;
        ColourBallManager.instance.spawnBomb = levelChanges[currentLevel].spawnBomb;
        Transform finishLine = GameObject.FindGameObjectWithTag("finishLine").transform;
        finishLine.position = new Vector3(finishLine.position.x, finishLine.position.y, levelChanges[currentLevel].levelLength);
        GameManager.instance.finishLineDistance = finishLine.transform.position.z - GameManager.instance.playerTransform.position.z;
    }
}
