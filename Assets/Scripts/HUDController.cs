using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TMP_Text Text;
    public GameObject ContextualHUD;
    public GameObject ControlsMenu;
    public Vector3 ActivePlanetPos;
    
    void Start()
    {
        ContextualHUD.SetActive(false);
        ShowControlsMenu();
    }

    void Update()
    {
        FollowPlanet();
    }
    
    public void ShowControlsMenu()
    {
        
        ControlsMenu.SetActive(true);
    }

    public void HideControlMenu()
    {
        ControlsMenu.SetActive(false);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    void FollowPlanet()
    {
        Vector3 followThis = Camera.main.WorldToScreenPoint(ActivePlanetPos);
        ContextualHUD.transform.position = followThis;
    }
    
}
