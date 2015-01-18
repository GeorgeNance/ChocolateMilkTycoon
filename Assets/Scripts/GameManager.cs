﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public float money;
    public float salePrice;



    private int sales;


	//stats stuff
    private float moneyEarned;
	private float moneySpent;

    public int inventory;

    public int popularity;


    //inventory stuff
    public int cocoaInventory;
    public float cocoaPrice;

    public int sugarInventory;
    public float sugarPrice;

    public int milkInventory;
    public float milkPrice;

    //Time stuff
    private int minute;
    private int hour=6;
    public float timeMult=2;
    public int day=1;

    public int startingHour;

    //recipe stuff
    public int cocoaAmt;
    public int sugarAmt;


	public bool nearEndOfDay;
    public bool endOfDay=false;
	public bool paused=true;

    public float tickTimer;


	//feats
	public bool milkGoesBad;

    //Upgrades
    public int maxMilkSaved;


    //Weather Stuff
    public int temperature=60;



    //Singleton crap

	private UIManager uiManager;


    void Awake()
    {

    }
	void Start () {
		temperature = Random.Range(0, 99);
		uiManager = gameObject.GetComponent<UIManager> ();
	}

    void TimeCycle()
    {
        minute += 1;
        if (minute >= 60)
        {
            hour += 1;
            minute = 0;
            //check if you can still sell stuff
            if (!canMakeMilk())
            {
                EndDay();
            }
        }
    }

    public string GetTime()
    {
		//TODO Fix time display
        if (hour > 12)
        {
            if (minute < 10)
            {
                return hour-12 + ":" + "0" + minute+ " PM";
            }
            else
            {
                return hour - 12 + ":" + minute + " PM";
            }
        }
        else
        {
            if (minute < 10)
            {
                return hour + ":" + "0" + minute + " AM";
            }
            else
            {
                return hour + ":" + minute + " AM";
            }
        }

    }




    //for buttons to buy stuff. Checks how much money has and evulates if it should add more to the inventory
    public void BuyMilk()
    {
        if (money >= milkPrice)
        {
            milkInventory += 10;
            money -= milkPrice;
			moneySpent+=milkPrice;

        }
    }
    public void BuySugar()
    {
        if (money >= sugarPrice)
        {
            sugarInventory += 50;
            money -= sugarPrice;
			moneySpent+=sugarPrice;
        }
    }
    public void BuyCocoa()
    {
        if (money >= cocoaPrice)
        {
            cocoaInventory += 50;
            money -= cocoaPrice;
			moneySpent+=cocoaPrice;
        }
    }

    
    
    


	public void StartDay(){
        if (milkInventory > 0 && cocoaInventory >= cocoaAmt && sugarInventory >= sugarAmt)
        {
            uiManager.OpenCloseRecipeMenu();
			paused=false;
            



        }
        else
        {
            if (money > cocoaPrice || money > sugarPrice || money > milkPrice)
            {
                uiManager.OpenMessageBox("WARNING", "You don't have enough items to start the day with the current recipe!", null);
            }
            else
            {
                uiManager.OpenMessageBox("GAME OVER", "You don't have enough money to buy supplies to coutinue. It's time to pack up and go home.",()=> Application.LoadLevel(0));
            }
        }
		}
    public void EndDay()
    {
		//Milk goes bad and must be thrown out unless you have fridge
		if (milkGoesBad) {
            if (milkInventory > maxMilkSaved)
            {
                milkInventory = maxMilkSaved;
            }
			
				}

        uiManager.OpenMessageBox("End of Day " + day, "Money Earned: \t\t" + moneyEarned.ToString("C2")+
		                                  "\n\nMoney Spent: \t\t"+moneySpent.ToString("C2")+
		                                  "\n\nTotal Earned: \t\t"+(moneyEarned-moneySpent).ToString("C2")
		                                  ,PrepareForDay);

		//Resets positions of all customers
		foreach (CustomerScript customer in (CustomerScript[])GameObject.FindObjectsOfType<CustomerScript>()) {
			customer.EndDay();
				}
        

    }
	void PrepareForDay(){
		hour = startingHour;
		minute = 0;
		day += 1;
		endOfDay = false;
		nearEndOfDay = false;
		moneyEarned = 0;
		moneySpent = 0;
		sales = 0;
		uiManager.OpenCloseRecipeMenu();
        ChangeTemp();

	}

    public bool canMakeMilk()
    {
        if (cocoaInventory >= cocoaAmt && sugarInventory >= sugarAmt && milkInventory != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void ChangeTemp()
    {
        if (temperature < 10)
        {
            temperature += Random.Range(0, 9);
        }
        else if (temperature > 90)
        {
            temperature -= Random.Range(0, 9);

        }
        else
        {
            temperature += Random.Range(-9, 9);

        }
    }

    public void CustomerBuy()
    {
        money += salePrice;
        moneyEarned += salePrice;
        cocoaInventory -= cocoaAmt;
        sugarInventory -= sugarAmt;
        milkInventory -= 1;
        sales += 1;

    }
    public void PauseGame()
    {
        uiManager.OpenMessageBox("PAUSED", "Hit ok to coutinue", null);
    }


	// Update is called once per frame
	void Update () {
        if (paused == false)
        {
            
            tickTimer+= Time.deltaTime*timeMult;
           
            if (tickTimer >= 0.9)
            {
                TimeCycle();
                tickTimer = 0;
            }

            if (hour >= 17)
            {

                EndDay();
					endOfDay=true;
            }
            if (hour >= 16)
            {
                nearEndOfDay = true;
            }


        }
        
	}
}
