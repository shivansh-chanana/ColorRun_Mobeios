using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform playerTrail;
    public Vector3 trailOffset;

    [Space]
    public Camera mainCam;
    public VacuumShaders.CurvedWorld.CurvedWorld_Controller curveController;

    [Space]
    public GameObject restartButton;
    public Image barImg;
    public GameObject nextLevelButton;
    public Text curLevelText, nextLevelText;
    public GameObject playButton;

    [Space]
    public Transform fireWorks;

    [Space]
    [SerializeField] float currentTime;
    public float finishLineDistance;

    [Header("Testing")]
    [Space]
    public bool isGodMod;

    Transform cameraTransform;
    Vector3 camToPlayerOffset;
    Vector2 targetCurve;
    ParticleSystem fireworksParticle;
    float finishLineDistanceTemp;

    public bool isGameOver = false;

    PlayerScript playerScript;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cameraTransform = mainCam.transform;
        camToPlayerOffset = cameraTransform.position - playerTransform.position;

        playerScript = playerTransform.GetComponent<PlayerScript>();

        targetCurve = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
    }

    private void Update()
    {
        if (isGameOver)
        {
            restartButton.SetActive(true);
            return;
        }

        #region Camera Follow Player and trail follow

        #region move with pos assign
        //Camera
        cameraTransform.position = new Vector3(cameraTransform.position.x,
            cameraTransform.position.y, playerTransform.position.z + camToPlayerOffset.z);

        //Trail
        playerTrail.position = playerTransform.position + trailOffset;
        #endregion

        #region move with smooth damp
        /* Smooth Damp
        Vector3 velocity = Vector3.zero;

        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, new Vector3(cameraTransform.position.x ,
            cameraTransform.position.y, playerTransform.position.z + camToPlayerOffset.z) , ref velocity , 0.1f);
        */
        #endregion

        #endregion

        //Roads Curved Timer
        RoadCurveTimer();
        curveController.SetBend(iTween.Vector2Update(curveController.GetBend(), targetCurve, 0.2f));

        #region FillBar Logic

        barImg.fillAmount = playerTransform.position.z/finishLineDistance;

        #endregion
    }

    void RoadCurveTimer()
    {

        if (currentTime < playerScript.forwardSpeed / 3)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            targetCurve = new Vector2(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
        }

    }

    public void PlayButton() {
        Time.timeScale = 1;
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator FinishLineCelebrate() {
        do
        {
            Vector3 newPos = playerTransform.position + Random.insideUnitSphere * 3;
            newPos.y = Mathf.Clamp(newPos.y,1f,1000f);
            fireWorks.position = newPos;
            if (fireworksParticle == null) fireworksParticle = fireWorks.GetComponent<ParticleSystem>();
            fireworksParticle.Play();
            yield return new WaitForSeconds(1f);
        } while (true);
    }
}
