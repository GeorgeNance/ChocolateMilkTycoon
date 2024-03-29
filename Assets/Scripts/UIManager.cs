﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	//
    //info bar
	//
	public Text moneyDisplay;
	//money ticker
	public float newMoney;
	public float oldMoney;
	public float moneySpeed;

    public Text centerDisplay;
    public Text tempDisplay;

    public Text[] milkInventoryDisplay;
    public Text milkPriceDisplay;


    public Text[] sugarInventoryDisplay;
    public Text sugarPriceDisplay;

    public Text[] cocoaInventoryDisplay;
    public Text cocoaPriceDisplay;
    
    
    //StartMenu


    public GameObject recipeMenu;

    public Text[] LocationNameTexts;
    public Text[] RentPriceTexts;
    public Text[] WeatherDisplayTexts;


    public Slider cocoaAmtSlider;
    public Slider sugarAmtSlider;
    public Text[] cocoaRecipeDisplay;
    public Text[] sugarRecipeDisplay;

    //Message box
    public GameObject messageBox;
    public Text messageBoxTitle;
    public Text messageBoxText;
    public Button messageBoxButton;

    //InGameBox
    public Animator inGameBoxAnim;
    public Text popDisplay;

    //Main Game
    public Text earnedDisplay;
    public Slider priceSlider;
    public Text priceDisplay;


    //Finances
    public Text cupsSold;
    public Text moneyEarned;


	private GameManager gameManager;
    private WeatherManager weather;


    //Settings
    public GameObject settingsMenu;




    //Location Selecter


    //bools
    bool recipeMenuOpened = true;
    void Awake()
    {

    }



    

	// Use this for initialization
	void Start ()
	{

	    weather = GameObject.FindObjectOfType<WeatherManager>();
		gameManager = gameObject.GetComponent<GameManager> ();
		oldMoney=gameManager.money;

	}
	
	// Update is called once per frame
	void Update () {
        //Cocoa and sugar slider
        if (gameManager != null)
        {

            //
            //Sliders
            //
            if (cocoaAmtSlider)
            {
                gameManager.cocoaAmt = (int)Mathf.Round(cocoaAmtSlider.value);
            }
            if (sugarAmtSlider)
            {
                gameManager.sugarAmt = (int)Mathf.Round(sugarAmtSlider.value);
            }

            //
            //Recipe Displays
            //
            if (cocoaRecipeDisplay.Length >0)
            {
                foreach (Text displayText in cocoaRecipeDisplay)
                {
                    displayText.text = gameManager.cocoaAmt.ToString();
                }
               
            }
            if (sugarRecipeDisplay.Length > 0)
            {
                foreach (Text displayText in sugarRecipeDisplay)
                {
                    displayText.text = gameManager.sugarAmt.ToString();
                 }
            }

            if (moneyDisplay != null)
            {
				newMoney=gameManager.money;

				if(oldMoney!=newMoney){
					oldMoney=Mathf.Lerp(oldMoney,newMoney,moneySpeed*Time.deltaTime);
				}
				moneyDisplay.text =oldMoney.ToString("C2");
            }

            ///
            /// LocationInfro Start menu
            /// 
            if (LocationNameTexts.Length > 0)
            {
                foreach (Text displayText in LocationNameTexts)
                {
                    displayText.text = gameManager.currentLocation.name.ToString();
                }
                {
                    
                }
            }
            if (RentPriceTexts.Length > 0)
            {
                foreach (Text displayText in RentPriceTexts)
                {
                    displayText.text = gameManager.currentLocation.rent.ToString("C2")+" / day";
                }
                {

                }
            }
            if (WeatherDisplayTexts.Length > 0)
            {
                foreach (Text displayText in WeatherDisplayTexts)
                {
                    displayText.text = gameManager.currentLocation.GetCondition().ToString()+", "+gameManager.currentLocation.GetTemperature().ToString();
                }
                {

                }
            }
            //
            //Inventory Displays
            //
            if (milkInventoryDisplay.Length > 0)
            {
                foreach (Text displayText in milkInventoryDisplay)
                {
                    displayText.text = gameManager.milkInventory.ToString();
                }
               
   
            }
            if (cocoaInventoryDisplay.Length > 0)
            {
                foreach (Text displayText in cocoaInventoryDisplay)
                {
                    displayText.text = gameManager.cocoaInventory.ToString();
                }
            }
            if (sugarInventoryDisplay.Length > 0)
            {
                foreach (Text displayText in sugarInventoryDisplay)
                {
                    displayText.text = gameManager.sugarInventory.ToString();
                }
            }
            //
            //Price Diplays
            //
            if (milkPriceDisplay != null && cocoaPriceDisplay != null && sugarPriceDisplay != null)
            {
                milkPriceDisplay.text =  gameManager.milkPrice.ToString("C2");
                sugarPriceDisplay.text = gameManager.sugarPrice.ToString("C2");
                cocoaPriceDisplay.text = gameManager.cocoaPrice.ToString("C2");

            }



            if (centerDisplay != null)
            {
                if (recipeMenuOpened==true)
                {
                    centerDisplay.text = "Day "+gameManager.day+" - Prep";
                }
                else
                {
                    centerDisplay.text = gameManager.GetTime();
                }
                
            }
            //Temps
            if (tempDisplay != null)
            {

                tempDisplay.text = weather.GetTemperature().ToString();
            }

            if (priceDisplay != null && priceSlider != null)
            {
                gameManager.salePrice = priceSlider.value*0.25f;

                   
                priceDisplay.text =(priceSlider.value * 0.25f).ToString("C2");

                
            }

            if (popDisplay != null) {
                popDisplay.text = "Popularity: " + gameManager.popularity;
            }

        }
	}

    public void OpenCloseRecipeMenu()
    {
        Animator anim=recipeMenu.GetComponent<Animator>();;
        if (anim.GetBool("Opened")==true){
            anim.SetBool("Opened",false);
            recipeMenuOpened = false;
        }
        else
        {
            anim.SetBool("Opened",true);
            recipeMenuOpened = true;
        }
    }

    // message box stuff below

    public void OpenMessageBox(string title, string text,UnityEngine.Events.UnityAction clickAction)
    {
        messageBox.SetActive(true);
        messageBoxTitle.text = title;
        messageBoxText.text = text;
       gameManager.paused = true;
        if (clickAction != null)
        {
            
           
            messageBoxButton.onClick.AddListener(() => clickAction());
            messageBoxButton.onClick.AddListener(() => CloseMessageBox());
            Debug.Log("Opened with 2 commands");
        }
        else
        {
            messageBoxButton.onClick.AddListener(()=>CloseMessageBox());
            Debug.Log("Opened with 1 command");
        }

    }


    public void CloseMessageBox()
    {
        messageBoxButton.onClick.RemoveAllListeners();
        messageBox.SetActive(false);
        if (recipeMenu.activeSelf == false)
        {
            gameManager.paused = false;
        }


    }

    public void OpenCloseInGameBox()
    {
        if(inGameBoxAnim.GetBool("isHidden")==true){
            inGameBoxAnim.SetBool("isHidden",false);

        }else{
            inGameBoxAnim.SetBool("isHidden",true);

        }
    }


    public void OpenCloseSettings()
    {
        if (settingsMenu.activeSelf == true)
        {
            settingsMenu.SetActive(false);
        }
        else
        {
            settingsMenu.SetActive(true);
        }
    }
}

