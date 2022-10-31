using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 worldPosition;

    private Plane plane = new Plane(Vector3.up, 0);

    public GameObject planetPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float pos;
            Ray screenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(screenToWorld, out pos))
            {
                worldPosition = screenToWorld.GetPoint(pos);
                Instantiate(planetPrefab,worldPosition , Quaternion.identity);
            }
        }
    }
}
