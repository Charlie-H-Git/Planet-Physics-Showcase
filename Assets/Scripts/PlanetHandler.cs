using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetHandler : MonoBehaviour
{
    private Quaternion lastParentRotation;
    private PhysicsControllerV2 _physicsControllerV2;
    private MeshRenderer MeshRenderer;
    public List<Material> Materials;
    public Rigidbody rb;
    public TMP_Text Text;
    public TrailRenderer _trailRenderer;
    public GameObject UI;
    public float Distance;
    // Start is called before the first frame update
    void Awake()
    {
        MeshRenderer = gameObject.GetComponent<MeshRenderer>();
        _trailRenderer = gameObject.GetComponent<TrailRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
        _physicsControllerV2 = FindObjectOfType<PhysicsControllerV2>();
    }

    private void OnEnable()
    {
        rb.mass = Random.Range(10, 80);
        int Scale =  Random.Range(1, 8);
        int Colour = Random.Range(0, Materials.Count);
        MeshRenderer.material = Materials[Colour];
        gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
        _physicsControllerV2.RegisterPlanet(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // UI.transform.localRotation = Quaternion.Inverse(transform.localRotation) * lastParentRotation *
        //                              UI.transform.localRotation;
        // lastParentRotation = transform.localRotation;    
        
        Text.text = "Mass = " + rb.mass + "\n" + "Velocity = " + rb.velocity.magnitude + "\n" + "Distance = " + Distance;
    }
}
