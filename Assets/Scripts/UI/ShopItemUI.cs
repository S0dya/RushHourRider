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
        priceText.text = Settings.itemPrices[index].ToString();
    }

    public void BuyItemButton()
    {
        if (Settings.money >= Settings.itemPrices[index])
        {
            Settings.money -= Settings.itemPrices[index];
            Settings.itemPrices[index] = 0;

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
                break;
            case 1:
                Settings.currentColorOfBGI = colorOfBGI;
                break;
            default: break;
        }
    }
}
