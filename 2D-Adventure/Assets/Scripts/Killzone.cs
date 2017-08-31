using System.Collections;
using UnityEngine;

public class Killzone : MonoBehaviour {

	public float damage = 100f;

	void OnTriggerStay2D(Collider2D other)
	{
		if (!other.CompareTag ("Player"))
			return;

		if (PlayerController.instance != null)
			PlayerController.health -= damage * Time.deltaTime;
	}
}
