﻿using UnityEngine;
using System.Collections;

public class LocationManager : MonoBehaviour
{

    private Location[] locations;
    private Location currentLocation;
    private WeatherManager weatherManager;

	// Use this for initialization
	void Awake ()
	{
	    locations = gameObject.GetComponentsInChildren<Location>();
	    weatherManager = GameObject.FindObjectOfType<WeatherManager>();
        
	}

    public void SetLocation(string name)
    {
        foreach (Location loco in locations)
        {
            if (loco.name.ToLower() == name.ToLower())
            {
                loco.ActivateLocation();
                currentLocation = loco;
                Debug.Log(currentLocation.name +"-"+weatherManager.GetCondition());
            }
            else
            {
                loco.DeactivateLocation();
            }
        }   
    }

    public void BeginDay()
    {
        foreach (Location loco in locations)
        {
            loco.NewDay();
        }
    }

	public Location GetCurrentLocation(){
		return currentLocation;
	}
    public Location[] GetLocations()
    {
        return locations;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
