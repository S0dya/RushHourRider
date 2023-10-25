using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public int index;
    public int type; //0 - color of bike, 1 - bg color

    public int colorOfBikeI;
    public int colorOfBGI;

    [SerializeField] GameObject buyObj;
    [SerializeField] TextMeshProUGUI priceText;

    void Start()
    {
        int price = Settings.itemPrices[index];
        if (price == 0) buyObj.SetActive(false);
        else priceText.text = price.ToString();
    }

    public void BuyItemButton()
    {
        if (Settings.money >= Settings.itemPrices[index])
        {
            Settings.money -= Settings.itemPrices[index];
            Settings.itemPrices[index] = 0;
            MenuUI.I.SetMoneyText();

            buyObj.SetActive(false);

            SetItemButton();
        }
    }

    public void SetItemButton()
    {
        switch (type)
        {
            case 0:
                Settings.currentColorOfBikeI = colorOfBikeI;
                MenuUI.I.SetBikeMaterial();
                break;
            case 1:
                Settings.currentColorOfBGI = colorOfBGI;
                GameManager.I.SetSkyBox();
                break;
            default: break;
        }
    }
}
