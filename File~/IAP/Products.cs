using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Products
{
    public Products(string productId, string productIdForGoogle, string productIdForiOS, int productType)
    {
        this.productId = productId;
        this.productIdForGoogle = productIdForGoogle;
        this.productIdForiOS = productIdForiOS;
        this.productType = productType;
    }

    public string productId { get; }
    public string productIdForGoogle { get; }
    public string productIdForiOS { get; }
    public int productType { get; } // 0消耗品，1非消耗品
}
