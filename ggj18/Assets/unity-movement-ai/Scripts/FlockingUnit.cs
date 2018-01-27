using UnityEngine;
using System.Collections;

public class FlockingUnit : MonoBehaviour
{
    public float cohesionWeight = 1.5f;
    public float separationWeight = 2f;
    public float velocityMatchWeight = 1f;
	public float arriveWeight = 1f;

    private SteeringBasics steeringBasics;
    private Wander2 wander;
    private Cohesion cohesion;
    private Separation separation;
    private VelocityMatch velocityMatch;
	private Arrive arrive;

    private NearSensor sensor;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        cohesion = GetComponent<Cohesion>();
        separation = GetComponent<Separation>();
        velocityMatch = GetComponent<VelocityMatch>();
		arrive = GetComponent<Arrive>();

        sensor = transform.Find("Sensor").GetComponent<NearSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 accel = Vector3.zero;

		if(cohesion !=null)
			accel += cohesion.getSteering(sensor.targets) * cohesionWeight;
		if(separation !=null)
        	accel += separation.getSteering(sensor.targets) * separationWeight;
		if(velocityMatch !=null)
        	accel += velocityMatch.getSteering(sensor.targets) * velocityMatchWeight;
		if(arrive !=null)
			accel += arrive.getSteering() * arriveWeight;
		
        if (accel.magnitude < 0.005f)
        {
			if(wander != null)
            	accel = wander.getSteering();
        }

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }
}
