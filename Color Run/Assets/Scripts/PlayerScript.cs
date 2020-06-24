using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float forwardSpeed;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator anim;
    public CameraShake cameraShake;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = new Vector3(0, 0, forwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball_red") || other.CompareTag("ball_yellow") || other.CompareTag("ball_blue")) {
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

        if (other.CompareTag("door_red") || other.CompareTag("door_yellow") || other.CompareTag("door_blue")) {
            string[] doorTag = other.tag.Split('_');
            string[] myTag = transform.tag.Split('_');

            if (doorTag[1] != myTag[1]) {
                forwardSpeed = 0;
                GameManager.instance.isGameOver = true;
                anim.Play("PlayerFall");

                StartCoroutine(cameraShake.Shake(0.5f, 0.2f));
                transform.Translate(transform.forward * -1f,Space.Self);
            }
        }

        if (other.CompareTag("finishLine"))
        {
            forwardSpeed = 0;
            GameManager.instance.isGameOver = true;
            StartCoroutine(GameManager.instance.FinishLineCelebrate());
            anim.Play("PlayerDance_1");
        }
    }
}
