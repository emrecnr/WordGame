using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
}
[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
}
public class ShopManager : MonoBehaviour, IDetailedStoreListener
{
    IStoreController m_storeController;

    [Header(" ## Coin Shop Elements ##")]
    public ConsumableItem consumableItem100;
    public ConsumableItem consumableItem250;
    public ConsumableItem consumableItem500;
    public ConsumableItem consumableItem1000;

    public NonConsumableItem nonConsumableItem;

    [Header(" ## Hint Shop Elements ##")]
    [SerializeField] GameObject coinPanel;
    [SerializeField] GameObject hintPanel;

    [SerializeField] Button coinButton;
    [SerializeField] Button hintButton;

    [SerializeField] private GameObject removeAdsContainer;

    [Header(" ## Color ## ")]
    [SerializeField] private Color disableColor;
    private void Start()
    {
        SetupBuilder();
        coinButton.onClick.AddListener(()=>{
            coinButton.image.color = Color.white;
            hintButton.image.color = disableColor;
            hintPanel.SetActive(false);
            coinPanel.SetActive(true);
        });

        hintButton.onClick.AddListener(()=>{
            coinButton.image.color = disableColor;
            hintButton.image.color = Color.white;
            coinPanel.SetActive(false);
            hintPanel.SetActive(true);
        });
    }
    #region Hint Shop Extras
    public void HintKeyboardBuyCallback()
    {
        if(DataManager.Instance.Coin < 50)
        {
            // TODO: Message
            Debug.Log("Not enough coin!");
            return;
        }
        DataManager.Instance.RemoveCoins(50);
        DataManager.Instance.IncreaseHintKeyboardCount();
    }
    
    public void HintWordBuyCallback()
    {
        if (DataManager.Instance.Coin < 50)
        {
            // TODO: Message
            Debug.Log("Not enough coin!");
            return;
        }
        DataManager.Instance.RemoveCoins(50);
        DataManager.Instance.IncreaseHintWordCount();
    }

    #endregion
    #region Coin Shop Extras
    private void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(consumableItem100.Id,ProductType.Consumable);
        builder.AddProduct(consumableItem250.Id,ProductType.Consumable);
        builder.AddProduct(consumableItem500.Id,ProductType.Consumable);
        builder.AddProduct(consumableItem1000.Id,ProductType.Consumable);
        builder.AddProduct(nonConsumableItem.Id,ProductType.NonConsumable);

        UnityPurchasing.Initialize(this,builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Success");
        m_storeController = controller;
        StartCoroutine(CheckNonConsumable(nonConsumableItem.Id));
    }

    public void ConsumableBtnCallback(string productID)
    {
        m_storeController.InitiatePurchase(productID);
    }

    public void NonConsumableBtnCallback()
    {
        m_storeController.InitiatePurchase(nonConsumableItem.Id);
    }

    IEnumerator  CheckNonConsumable(string id)
    {
        if(m_storeController != null)
        {
            yield return new WaitUntil(()=> DataManager.Instance.IsDataLoaded);
            Debug.Log(DataManager.Instance.IsDataLoaded);
            var product = m_storeController.products.WithID(id);
            if(product != null)
            {
                if(product.hasReceipt) // purchased
                {
                    // Remove Ads
                    PlayerPrefs.SetInt("RemoveAds",1);
                    DataManager.Instance.RemoveAds = 1;
                    removeAdsContainer.SetActive(false);
                }
                else
                {
                    // Show Ads()
                    PlayerPrefs.SetInt("RemoveAds", 0);
                    DataManager.Instance.RemoveAds = 0;
                    removeAdsContainer.SetActive(true);
                }

            }
        }


    }
    // processing purchase
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        // Retrive the purchased product
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("Purchase Complete" + product.definition.id);

        if(product.definition.id == consumableItem100.Id) // consumable item is pressed
        {
            // Add coin
            DataManager.Instance.AddCoins(100);
        }
        else if(product.definition.id == consumableItem250.Id)
        {
            DataManager.Instance.AddCoins(250);

        }
        else if (product.definition.id == consumableItem500.Id)
        {
            DataManager.Instance.AddCoins(500);
        }
        else if (product.definition.id == consumableItem1000.Id)
        {
            DataManager.Instance.AddCoins(1000);
        }
        else if (product.definition.id == nonConsumableItem.Id)
        {
            //RemoveAds
            PlayerPrefs.SetInt("RemoveAds", 1);
            DataManager.Instance.RemoveAds = 1;
            removeAdsContainer.SetActive(false);            
        }

        return PurchaseProcessingResult.Complete;   
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Initialize failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Initialize failed" + error);

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase failed");

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log("Purchase failed");
    }
    #endregion
}
