using UnityEngine;
using System.Collections;

public class Arrive : MonoBehaviour {

	public Vector3 targetPosition;

	private SteeringBasics steeringBasics;

	// Use this for initialization
	void Start()
	{
		steeringBasics = GetComponent<SteeringBasics>();
	}

	// Update is called once per frame
	public Vector3 getSteering()
	{
		Vector3 accel = steeringBasics.arrive(targetPosition);

		return accel;
	}
}
