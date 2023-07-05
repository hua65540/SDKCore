using System;
using System.Collections;
using System.Collections.Generic;
using io.adjoe.sdk;
using UnityEngine;

public class ADJoeManager : MonoBehaviour
{

    private string sdk_hash = "16dc72677d0e4ef3bd45b6b75243f786";
    void Start()
    {
        Debug.Log("ssssssssssssssss");
        // AdjoeOptions options = new AdjoeOptions().SetUserId(Adjoe.GetUserId());
        //
        // if (!Adjoe.IsInitialized()) {
        //     Adjoe.Init("16dc72677d0e4ef3bd45b6b75243f786",options,AdjoeInitialisationSuccess,AdjoeInitialisationError);
        //     Adjoe.SetOfferwallListener(OfferwallOpened,OfferwallClosed);
        // }
        
        // Adjoe.DoPayout(AdjoePayoutSuccessResponse,AdjoePayoutSuccessResponse);
    }


    public void RequestUserRewards()
    {
        Adjoe.RequestRewards(AdjoeRewardResponseSuccess,AdjoeRewardResponseFail);
    }

    public void RequestPremission()
    {
        Adjoe.RequestUsagePermission(RequestUsagePermissionSuccess,RequestUsagePermissionFail);
    }

    public bool CheckTOSTerm()
    {
        return Adjoe.HasAcceptedTOS();
    }

    public bool HasAcceptedUsagePermission()
    {
        return Adjoe.HasAcceptedUsagePermission();
    }

    public void JoeClick()
    {
        showWall();
    }

    public void showWall()
    {
        // if (!CheckTOSTerm())
        // {
        //     Debug.Log("没有接受服务条款");
        //     return;
        // }

        // if (!HasAcceptedUsagePermission())
        // {
        //     RequestPremission();
        //     return;
        // }
        if (Adjoe.CanShowOfferwall()) {
            // Show the teaser button.
            Debug.Log("展示积分墙");
            Adjoe.SendUserEvent(Adjoe.EVENT_TEASER_SHOWN, null);
            Adjoe.ShowOfferwall();
        }
        
    }



    //---------------
    //listener
    //---------------

    public void RequestUsagePermissionSuccess()
    {
        showWall();
    }

    public void RequestUsagePermissionFail(Exception exception)
    {
        Debug.Log("请求授权失败,msg="+exception.Message);
    }

    public void AdjoeRewardResponseSuccess(AdjoeRewardResponse adjoeRewardResponse)
    {
        
    }
    
    public void AdjoeRewardResponseFail(AdjoeRewardResponseError adjoeRewardResponse)
    {
        
    }

    public void OfferwallOpened(String param)
    {
    }

    public void OfferwallClosed(String param)
    {
        
    }

    public void AdjoePayoutSuccessResponse(AdjoePayoutResponse adjoePayoutResponse)
    {
        
    }

    public void AdjoePayoutSuccessResponse(AdjoePayoutError adjoePayoutError)
    {
        
    }

    public void AdjoeInitialisationSuccess()
    {
        Debug.Log("初始化成功");
    }
    
    public void AdjoeInitialisationError(Exception exception)
    {
        Debug.Log("初始化失败,msg="+exception.Message);
    }


}
