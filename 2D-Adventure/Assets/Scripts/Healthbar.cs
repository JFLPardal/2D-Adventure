using System.Collections;
using UnityEngine;

public class Healthbar : MonoBehaviour {

	private RectTransform transform;

	public float maxSpeed = 10f;

	void Awake () 
	{
		transform = GetComponent<RectTransform> ();
	}

	void Start()
	{
		if (PlayerController.instance != null)
			transform.sizeDelta = new Vector2 (Mathf.Clamp (PlayerController.health, 0, 100),
											  transform.sizeDelta.y);
	}

	void Update () 
	{
		float healthUpdate = 0f;

		if (PlayerController.instance != null) 
			healthUpdate = Mathf.MoveTowards (transform.rect.width, PlayerController.health, maxSpeed);

		transform.sizeDelta = new Vector2 (Mathf.Clamp (healthUpdate, 0, 100),
										  transform.sizeDelta.y);
	}
}
