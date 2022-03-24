
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class OnPlayerCheck : UdonSharpBehaviour
{
    [Header("トリガー人数")]
    [SerializeField, Range(0, 100)] private int triggercount;

    [Header("トリガー人数より多いとき表示するオブジェクト")]
    [SerializeField] private GameObject[] hideobject;

    [Header("トリガー人数以下のとき表示するオブジェクト")]
    [SerializeField] private GameObject[] showobject;

    [UdonSynced(UdonSyncMode.None)]private int OPCstayplayer;
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        OPCstayplayer++;
        if (OPCstayplayer <= triggercount)
        {
            return;
        }
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                hideobject[i].SetActive(true);
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                showobject[i].SetActive(false);
            }
        }
        RequestSerialization(); 
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        OPCstayplayer--;
        if (OPCstayplayer > triggercount)
        {
            return;
        }
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                hideobject[i].SetActive(false);
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                showobject[i].SetActive(true);
            }
        }
        RequestSerialization(); 
    }

    void OnPlayerJoin(VRCPlayerApi player)
    {
        RequestSerialization(); 
    }
}