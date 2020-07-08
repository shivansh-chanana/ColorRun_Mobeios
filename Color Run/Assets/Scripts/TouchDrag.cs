using UnityEngine;
using System.Collections;

public class TouchDrag : MonoBehaviour
{
	[SerializeField]
	bool touchHasBegan = true;

	Vector3 screenPoint;
	Vector3 offset;

	Camera cam;
	Rigidbody rb;

	float DeadArea;

	// Use this for initialization
	void Start ()
	{
		cam = Camera.main;
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (GameManager.instance.isGameOver) return;
		TouchControl ();
	}

	void TouchControl()
	{
		// If more than 1 Touch
		if (Input.touchCount>0)
		{
			//Touch Has Started
			if (Input.GetTouch(0).phase == TouchPhase.Began)
			{
				touchHasBegan = true;

				//For Drag Start
				#region Drag
				screenPoint = cam.WorldToScreenPoint(rb.transform.position);
				offset = rb.transform.position - cam.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, screenPoint.z));
				#endregion
			}

			//Touch Ended
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				touchHasBegan = false;
			}

			//During Touch Down
			if (touchHasBegan) {
				//For Dragging Ball
				Vector3 curScreenPoint = new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, screenPoint.z);
				Vector3 curPosition = cam.ScreenToWorldPoint (curScreenPoint) + offset;
				Vector3 velocity = Vector3.zero;

				//Drag Using Smooth Damp
				if (!(rb.transform.position.x < curPosition.x + DeadArea && rb.transform.position.x > curPosition.x - DeadArea)){
					//rb.transform.position = Vector3.SmoothDamp (rb.transform.position, new Vector3 (curPosition.x, rb.transform.position.y, rb.transform.position.z), ref velocity, movementSmoth);
					rb.transform.position = new Vector3 (curPosition.x, rb.transform.position.y, rb.transform.position.z);
				}
			}
		}
	}
}

