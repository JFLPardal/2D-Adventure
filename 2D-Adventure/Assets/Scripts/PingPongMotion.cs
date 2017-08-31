using System.Collections;
using UnityEngine;

public class PingPongMotion : MonoBehaviour {

	private Transform transform;
	private Vector3 originPos = Vector3.zero;
	public Vector3 moveAxes = Vector2.zero;

	public float distance = 3f;

	void Awake()
	{
		transform = GetComponent<Transform> ();
		originPos = transform.position;
	}

	void Update()
	{
		transform.position = originPos + moveAxes * Mathf.PingPong(Time.time, distance);
	}
}
