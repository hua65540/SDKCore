using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;
using Newtonsoft.Json.Linq;
using I2.Loc.SimpleJSON;

public class IAPListener : Singleton<IAPListener>, IStoreListener
{

    private IStoreController myController;
    private IExtensionProvider myExtensions;
    private string productId;
    private Product product;
    private bool isInit = false;
    private string type;
    private double price;
    private string currencyCode;


    public void init(List<Products> pList)
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var p in pList)
        {
            builder.AddProduct(p.productId, (ProductType)p.productType, new IDs
            {
                {p.productIdForGoogle, GooglePlay.Name},
                {p.productIdForiOS, MacAppStore.Name}
            });
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void buy(string id)
    {
        productId = id;
        if (myController != null)
        {
            myController.InitiatePurchase(id);
        }
        else
        {
            Debug.Log("myController is null");
            IAPManager.Instance.payAction.Invoke(false);
        }
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("初始化成功");
        isInit = true;
        this.myController = controller;
        this.myExtensions = extensions;

        Debug.Log("Fetched successfully!");
        // The additional products are added to the set of
        // previously retrieved products and are browseable
        // and purchasable.
        JArray jsonArray = new JArray();
        foreach (var product in myController.products.all)
        {
            //获取产品ID productId
            string productId = product.definition.storeSpecificId;
            Debug.Log("产品id=" + product.definition.storeSpecificId);
            //获取商品类型 type
            ProductType type = product.definition.type;
            Debug.Log(product.definition.type);
            //获取商品标题 title
            string title = product.metadata.localizedTitle;
            Debug.Log(product.metadata.localizedTitle);
            //获取商品名称
            string name = product.metadata.localizedTitle;
            Debug.Log(product.metadata.localizedPriceString);
            //获取产品描述
            string description = product.metadata.localizedDescription;
            Debug.Log(product.metadata.localizedDescription);
            //获取产品价格
            string price = product.metadata.localizedPriceString;
            Debug.Log(product.metadata.localizedPriceString);
            //获取产品price_amount_micros价格
            string price_amount_micros = product.metadata.localizedPrice.ToString();
            Debug.Log(product.metadata.localizedPrice);
            //获取产品price_currency_code
            string price_currency_code = product.metadata.isoCurrencyCode;
            Debug.Log(product.metadata.isoCurrencyCode);
            //获取产品skuDetailsToken 没有token设置为xxx
            string skuDetailsToken = "xxx";
            Debug.Log("没有token 设置为xxx");
            //把上面获取到的信息放入到一个字典中
            Dictionary<string, string> productInfo = new Dictionary<string, string>();
            productInfo.Add("productId", productId);
            productInfo.Add("type", type.ToString());
            productInfo.Add("title", title);
            productInfo.Add("name", name);
            productInfo.Add("description", description);
            productInfo.Add("price", price);
            productInfo.Add("price_amount_micros", price_amount_micros);
            productInfo.Add("price_currency_code", price_currency_code);
            productInfo.Add("skuDetailsToken", skuDetailsToken);
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(productInfo);
            JObject json = JObject.Parse(jsonStr);
            jsonArray.Add(json);
            Debug.Log("单一商品信息=" + jsonStr);
        }
        Debug.Log("商品信息=" + jsonArray);
        SDKManager.Instance.setProductInfo(jsonArray.ToString());


    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error, string msg)
    {

        Debug.Log("初始化失败,errMsg=" + msg);
        isInit = false;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("初始化失败");
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {

        if (IAPManager.Instance.payAction == null)
        {
            //可以在这里处理恢复购买的操作
            return PurchaseProcessingResult.Complete;
        }

        if (!productId.Equals(e.purchasedProduct.definition.id))
        {
            Debug.Log($"当前商品id{productId}与返回的商品id{e.purchasedProduct.definition.id}不一致,不进行发放");
            return PurchaseProcessingResult.Complete;
        }
        Debug.Log("购买成功invoke执行");
        this.product = e.purchasedProduct;
        string id = e.purchasedProduct.definition.id;
        //获取商品价格
        this.price = (double)e.purchasedProduct.metadata.localizedPrice;
        //获取商品货币代码
        this.currencyCode = e.purchasedProduct.metadata.isoCurrencyCode;
        //获取商品类型
        this.type = e.purchasedProduct.definition.type.ToString();
        seJson(e.purchasedProduct.receipt);
        return PurchaseProcessingResult.Pending;
    }


    public void seJson(string json)
    {
        try
        {
            JObject obj = JObject.Parse(json);
            string Payload = (string)obj["Payload"];
            JObject payObj = JObject.Parse(Payload);
            string receipt = (string)payObj["json"];
            JObject receiptObj = JObject.Parse(receipt);
            // 获取字段值
            string orderId = (string)receiptObj["orderId"];
            string productId = (string)receiptObj["productId"];
            string purchaseToken = (string)receiptObj["purchaseToken"];
            vaildToken(orderId, productId, purchaseToken);
        }
        catch (Exception e)
        {
            Debug.Log("解析json发生异常,err=" + e.Message);
            IAPManager.Instance.payAction.Invoke(false);
        }

    }

    public async void vaildToken(string orderId, string productId, string purchaseToken)
    {
        await HttpRequest.Instance.MakeNetworkRequest(orderId, productId, purchaseToken);
        string bodyString = HttpRequest.Instance.responseBody;
        if (bodyString.Equals("") || bodyString == "")
        {
            IAPManager.Instance.payAction.Invoke(false);
            return;
        }
        JObject obj = JObject.Parse(bodyString);
        int code = (int)obj["code"];
        string msg = (string)obj["message"];
        if (code == 200 && (msg.Equals("success") || msg == "success"))
        {
            if (this.product != null)
            {
                SDKManager.Instance.logEvent("IAP_Success", "OrderId=" + orderId + "-productId=" + productId + "-purchaseToken=" + purchaseToken + "price=" + this.product.metadata.localizedPrice + "-code=" + this.product.metadata.isoCurrencyCode);
                confirmPurchase();
            }
        }
        else
        {
            Debug.Log("校验失败");
            IAPManager.Instance.payAction.Invoke(false);
        }
    }

    public void confirmPurchase()
    {
        SDKManager.Instance.logIapRevenue(productId, productId, type, price, currencyCode);
        IAPManager.Instance.payAction.Invoke(true);
        myController.ConfirmPendingPurchase(this.product);
        this.product = null;
    }



    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("购买失败invoke执行");
        // string id = i.definition.id; //获取商品id
        IAPManager.Instance.payAction.Invoke(false);
    }





}