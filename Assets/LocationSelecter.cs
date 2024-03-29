﻿using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocationSelecter : MonoBehaviour {
    private LocationManager locoMan;
    public GameObject locationSlot;
    private List<Location> locations;


    bool loadedLocations=false;

	// Use this for initialization
	void Start () {
        locoMan = GameObject.FindObjectOfType<LocationManager>().GetComponent<LocationManager>();
  

	}

    private void AddLocationSlot(Location loc)
    {
        GameObject locoSlot = Instantiate(locationSlot);

        locoSlot.transform.Find("NameLabel").GetComponent<Text>().text=loc.name;
        locoSlot.transform.Find("PriceLabel").GetComponent<Text>().text = loc.rent.ToString("C2")+" / day";
        locoSlot.name = loc.name;


        if (loc.name == locoMan.GetCurrentLocation().name)
        {
            locoSlot.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            locoSlot.GetComponent<Toggle>().isOn = false;
        }

        locoSlot.GetComponent<Toggle>().group = gameObject.GetComponent<ToggleGroup>();
        locoSlot.transform.SetParent(gameObject.transform, false);
        locoSlot.transform.SetAsFirstSibling();
    }

    public void ChangeLocation()
    {
        foreach (Toggle tog in gameObject.GetComponent<ToggleGroup>().ActiveToggles())
        {
            if (tog.isOn == true)
            {
                locoMan.SetLocation(tog.name);
            }
        }
    }

    void LateUpdate()
    {
        if (loadedLocations==false){
            locations = locoMan.GetLocations().ToList();
            locations = locations.OrderBy(o => o.rent).Reverse().ToList();
            Debug.Log(locations);
            foreach (Location location in locations)
            {
                Debug.Log(locoMan.GetCurrentLocation().name);
                AddLocationSlot(location);
            }
            loadedLocations = true;
        }
    }
	

}
