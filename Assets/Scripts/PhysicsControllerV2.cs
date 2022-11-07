using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsControllerV2 : MonoBehaviour
{
    private GameObject[] planetGameObjectArray;
    private const float massScale = 1e-24f;
    public List<_Planet> planets;
    private Rigidbody m1;
    private void Awake()
    {
        m1 = GetComponent<Rigidbody>();
        Time.timeScale = 1f;
        planetGameObjectArray = GameObject.FindGameObjectsWithTag("Planet");
        for (int i = 0; i < planetGameObjectArray.Length; i++)
        {
            var p = new _Planet
            {
                _trailRenderer = planetGameObjectArray[i].GetComponent<TrailRenderer>(),
                _planet = planetGameObjectArray[i],
                _Rigidbody = planetGameObjectArray[i].GetComponent<Rigidbody>(),
                sunRigidbody = gameObject.GetComponent<Rigidbody>()
            };
            StartPlanet(p);
            planets.Add(p);
        }
    }
    public void StartPlanet(_Planet _planetManager)
    {
        _planetManager._trailRenderer.enabled = true;
    }

    public void RegisterPlanet(GameObject planet)
    {
        var p = new _Planet()
        {
            _trailRenderer = planet.GetComponent<TrailRenderer>(),
            _planet = planet,
            _Rigidbody = planet.GetComponent<Rigidbody>(),
            sunRigidbody = m1
        };
        StartPlanet(p);
        planets.Add(p);
        StartCoroutine(PlanetGrav(planets));
    }

    public int _planetListIdentifier;
    IEnumerator PlanetGrav(List<_Planet> planets)
    {
        //Loops through every planet in the Global Planet List
        for(_planetListIdentifier = 0; _planetListIdentifier < planets.Count; _planetListIdentifier++)
        {
            //Assigns the active planet in the for loop to the variable planet
            _Planet _planet = planets[_planetListIdentifier];
            
            //Gets Planet Handler Script >>THIS IS FOR THE PURPOSES OF THE PLAYABLE DEMO<<
            _planet._planet.GetComponent<PlanetHandler>().Distance = planets[_planetListIdentifier].distance.magnitude;
            
            //Calls the active planets Gravitational Force Method
            _planet.PlanetUpdate();
            //Calls the active planets Centripetal Force Method
            _planet.PlanetRotate();
            //Delays the loop by 0.08th of a second
            yield return new WaitForSeconds(0.08f);
            
            //Resets the planet index to negative one to restart the loop
            if (_planetListIdentifier >= planets.Count - 1)
            {
                _planetListIdentifier = -1; 
            }
        }
    }
}
[Serializable]
public class _Planet
{
    public TrailRenderer _trailRenderer;
    public GameObject _planet;
    public Rigidbody _Rigidbody;
    public Rigidbody sunRigidbody;
    public float IndvVelocity;
    public float G = 6.674f;
    public float f;
    public float PhysicsMultiplier = 10f;
    public Vector3 distance;

    public void PlanetUpdate()
    {
        //Assigns the active planet to a referable Transform Value
        Transform other = _planet.transform;
        //Then Casts it to a Vector3
        Vector3 otherPos = other.position;
        //Gets a Reference to the suns position at the center of the world
        Vector3 SunPos = Vector3.zero;
        
        //Calculates the difference between the Active Planet and the sun
        distance = SunPos - otherPos;
        //Casts the distance vector to a float
        float r = distance.magnitude;
        //Gets the other planets Rigidbody
        Rigidbody m2 = _Rigidbody;
        
        //Main Equation for Gravitational Constant
        f = G * sunRigidbody.mass * m2.mass * PhysicsMultiplier / (r*r);
        
        //Calculates the Force Vector by Using the Distance Normalized Multiplied by F * Time
        Vector3 FORCE = distance.normalized * (f * Time.deltaTime);
        
        //Applies Resulting Force
        m2.AddForce(FORCE);
    }

    public void PlanetRotate()
    {
        //Assigns the active planet to a referable Transform Value
        Transform other = _planet.transform;
        //Rotates the planet to face the sun on its forward axis
        other.LookAt(Vector3.zero,Vector3.up);
        //Casts the Distance.SqrtMag to a float
        float r = distance.sqrMagnitude;
        
        //Calculates the Centripetal Velocity of the active planet
        IndvVelocity = Mathf.Sqrt(f * sunRigidbody.mass * PhysicsMultiplier / r);
        
        //Calculates the motion vector by multiplying the transform.right
                            //by the result of the centripetal calculation
        Vector3 motion = other.transform.right * IndvVelocity;
        
        //Assigns the Active planet rigidbody to a referable local variable
        Rigidbody rb = _Rigidbody;
        
        //this if statement applies the absolute value of motion
                    //when the planet moves beyond the positive or negative world space Coordinates
        if (other.position.x > 0 && other.position.y > 0)
        {
            motion.x = Math.Abs(motion.x);
            motion.y = -Math.Abs(motion.y);
        }
        else if (other.position.x > 0 && other.position.y < 0)
        {
            motion.x = -Math.Abs(motion.x);
            motion.y = -Math.Abs(motion.y);
        }
        
        //Applies the motion vector to the rigidbodies velocity value
        rb.velocity = motion;
    }
}