using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    //============================ Private Variables ============================\\
    private Vector3 worldPosition;
    private PhysicsRaycaster _physicsRaycaster;
    private HUDController HUDController;
    private Plane plane = new Plane(Vector3.up, 0);
    private RaycastHit hit;
    private GameObject planet;
    private PlanetHandler planetHandler;
    private Rigidbody planetRigidbody;
    private Ray screenToWorld;
    //============================ Public Variables ============================\\
    public GameObject planetPrefab;
    public float CameraSpeed;
    void Start()
    {
        HUDController = GameObject.FindGameObjectWithTag("UI").GetComponent<HUDController>();
        _physicsRaycaster = GetComponent<PhysicsRaycaster>();
    }
    void Update()
    {
        screenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);
        PlayerInputMethod();
        PlanetInfoMethod();
    }
    void PlayerInputMethod()
    {
        MoveCamera();
        SpawnPlanet();
        SpeedControls();
    }
    void MoveCamera()
    {
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * CameraSpeed;
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Vector3.forward * Time.deltaTime * CameraSpeed ;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position += Vector3.back * Time.deltaTime * CameraSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position += Vector3.left * Time.deltaTime * CameraSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Vector3.right * Time.deltaTime * CameraSpeed;
        }
    }
    void SpawnPlanet()
    {
        if (Input.GetMouseButtonDown(0) && !MouseOnUI())
        {
            HUDController.ContextualHUD.SetActive(true);
            float pos;
            if (Physics.Raycast(screenToWorld, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Planet"))
                {
                    print(hit.collider.gameObject.name);
                    planet = hit.collider.gameObject;
                }
            }
            else
            {
                if (plane.Raycast(screenToWorld, out pos))
                {
                    worldPosition = screenToWorld.GetPoint(pos);
                    planet = Instantiate(planetPrefab,worldPosition , Quaternion.identity);
                }
            }
        }
    }
    void SpeedControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 2;
        }
    }
    void PlanetInfoMethod()
    {
        if (planet != null)
        {
            planetHandler = planet.gameObject.GetComponent<PlanetHandler>();
            planetRigidbody = planetHandler.rb;
            HUDController.Text.text =  "Mass = " + planetRigidbody.mass + "\n" 
                                       + "Velocity = " + planetRigidbody.velocity.magnitude + "\n" 
                                       + "Distance = " + planetHandler.Distance;
            HUDController.ActivePlanetPos = planet.transform.position;
        }
    }
    public bool MouseOnUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<MouseIgnore>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        return raycastResults.Count > 0;
    }
}
