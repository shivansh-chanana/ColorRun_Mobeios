using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float forwardSpeed;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator anim;
    public CameraShake cameraShake;
    public Outline outline;

    Rigidbody rb;
    float tempForwardSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempForwardSpeed = forwardSpeed;

        LevelManager.instance.LevelLoaded(this);
    }

    void Update()
    {
        rb.velocity = new Vector3(0, 0, forwardSpeed);
        Vector3 clampPos = new Vector3(Mathf.Clamp(transform.position.x, -2.7f, 2.7f) , 0.5f, transform.position.z);
        transform.position = clampPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.isGameOver) return;

        if (!GameManager.instance.isGodMod)
        {
            if (other.CompareTag("ball_red") || other.CompareTag("ball_yellow") || other.CompareTag("ball_blue"))
            {
                transform.tag = other.tag;
                for (int i = 0; i < ColourBallManager.instance.colourData.Length; i++)
                {
                    if (other.tag == ColourBallManager.instance.colourData[i].tagName)
                    {
                        skinnedMeshRenderer.material = ColourBallManager.instance.colourData[i].material;
                        break;
                    }
                }
            }

            if (other.CompareTag("door_red") || other.CompareTag("door_yellow") || other.CompareTag("door_blue"))
            {
                string[] doorTag = other.tag.Split('_');
                string[] myTag = transform.tag.Split('_');

                if (doorTag[1] != myTag[1])
                {
                    iTween.Stop(gameObject);
                    forwardSpeed = 0;
                    GameManager.instance.isGameOver = true;
                    anim.Play("PlayerFall");
                    outline.enabled = false;
                    StopAllCoroutines();
                    StartCoroutine(cameraShake.Shake(0.5f, 0.2f));
                    transform.Translate(transform.forward * -1f, Space.Self);
                    SoundManager.instance.PlaySfx(2);
                    SoundManager.instance.PlaySfx(6);
                }
                else
                {
                    StartCoroutine(PunchBreak(other.gameObject));
                    SoundManager.instance.PlaySfx(3);
                }
            }

            if (other.CompareTag("Obstacle"))
            {
                iTween.Stop(gameObject);
                forwardSpeed = 0;
                GameManager.instance.isGameOver = true;
                anim.Play("PlayerFall");
                outline.enabled = false;
                StopAllCoroutines();
                StartCoroutine(cameraShake.Shake(0.5f, 0.2f));
                SoundManager.instance.PlaySfx(1);
                SoundManager.instance.PlaySfx(6);
            }
        }

        if (other.CompareTag("finishLine"))
        {
            forwardSpeed = 0;
            GameManager.instance.isGameOver = true;
            StartCoroutine(GameManager.instance.FinishLineCelebrate());
            anim.Play("PlayerDance_1");
            GameManager.instance.nextLevelButton.SetActive(true);
            SoundManager.instance.PlaySfx(5);
        }

        if (other.CompareTag("speedPad")) {
            if (forwardSpeed < tempForwardSpeed + 1)
            {
                Hashtable ht = iTween.Hash("from", forwardSpeed, "to", forwardSpeed + 5f, "time", 1f, "onupdate", "SpeedIncrease", "oncomplete", "SpeedDecrease");
                iTween.ValueTo(gameObject, ht);
                //SoundManager.instance.PlaySfx(0);
            }
        }
    }

    void SpeedIncrease(float newSpeed) {
        forwardSpeed = newSpeed;
    }

    void SpeedDecrease() {
        Hashtable ht = iTween.Hash("from", forwardSpeed, "to", tempForwardSpeed, "time", 5f, "onupdate", "SpeedIncrease");
        iTween.ValueTo(gameObject, ht);
    }

    IEnumerator PunchBreak(GameObject other) {
        //forwardSpeed = 0;
        //anim.Play("PlayerPunch");
        iTween.MoveTo(other, new Vector3(Random.Range(-10, 10), transform.position.y + 10f, transform.position.z + 50f), 5f);
        iTween.ScaleTo(other, Vector3.zero, 5f);
        other.GetComponent<ColourDoor>().anim.Play("Blocker_Fall_Anim");
        //yield return new WaitForSeconds(0.3f);
        //anim.Play("PlayerRun");
        //forwardSpeed = tempForwardSpeed;
        yield return null;
    }
}
