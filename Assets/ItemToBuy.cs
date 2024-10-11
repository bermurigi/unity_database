using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ItemToBuy : MonoBehaviour
{
    public int coinsPrice;
    public string itemName;

    public void BuyItem()
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = coinsPrice

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,OnSubtractCoinsSuccess,OnError);
    }

    void OnSubtractCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Bought: "+itemName);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error: " + error.ErrorMessage);
    }
}
