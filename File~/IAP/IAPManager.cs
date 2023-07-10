using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IAPManager : Singleton<IAPManager>
{

    private IAPListener iapListener;

    public Action<bool> payAction;

    public void initIAP()
    {
        IAPListener.Instance.init(getProducts());
    }

    public void pay(string productId, Action<bool> payAction)
    {
        Debug.Log("productId=" + productId);
        this.payAction = payAction;
        IAPListener.Instance.buy(productId);
    }


    /*
        设置商品
        参数1：商品id
        参数2：Google商品id
        参数3：ios商品id
        参数4：商品类型 -- 0 消耗品 1 非消耗品
    */

    public List<Products> getProducts()
    {
        List<Products> pList = new List<Products>();
        Products products1 = new Products("diamond_60", "diamond_60", "diamond_60", 0);
        Products products2 = new Products("diamond_240", "diamond_240", "diamond_240", 0);
        Products products3 = new Products("diamond_600", "diamond_600", "diamond_600", 0);
        Products products4 = new Products("diamond_1300", "diamond_1300", "diamond_1300", 0);
        Products products5 = new Products("diamond_3200", "diamond_3200", "diamond_3200", 0);
        Products products6 = new Products("diamond_7000", "diamond_7000", "diamond_7000", 0);
        Products products7 = new Products("role_scientist", "role_scientist", "role_scientist", 1);
        pList.Add(products1);
        pList.Add(products2);
        pList.Add(products3);
        pList.Add(products4);
        pList.Add(products5);
        pList.Add(products6);
        pList.Add(products7);
        return pList;
    }


}
