
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

    [UdonSynced]private int OPCstayplayer;

    [UdonSynced]private int x = 0;

    async void start()
    {
        if (OPCstayplayer <= triggercount)
        {
            for (int i = 0;i<showobject.Length;i++)
            {
                if (showobject[i] != null)
                showobject[i].SetActive(true);
            }
            for (int i=0;i<hideobject.Length;i++)
            {
                if (hideobject[i] != null)
                hideobject[i].SetActive(false);  
            }
        }
        else
        {
            for (int i = 0;i<showobject.Length;i++)
            {
                if (showobject[i] != null)
                showobject[i].SetActive(false);
            }
            for (int i=0;i<hideobject.Length;i++)
            {
                if (hideobject[i] != null)
                hideobject[i].SetActive(true);  
            }
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!Networking.IsOwner(gameObject)) return;
        OPCstayplayer++;
        RequestSerialization();
        if (OPCstayplayer <= triggercount) return;

        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "hideobjecttrue");
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "showobjectfalse");
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    { 
        if (!Networking.IsOwner(gameObject)) return;
        OPCstayplayer--;
        RequestSerialization();
        if (OPCstayplayer > triggercount) return;

        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "hideobjectfalse");
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "showobjecttrue");
            }
        }
    }

    public void showobjecttrue()
    {
        showobject[x].SetActive(true);
    }

    public void showobjectfalse()
    {
        showobject[x].SetActive(false);     
    }

    public void hideobjecttrue()
    {
        hideobject[x].SetActive(true);
    }

    public void hideobjectfalse()
    {
        hideobject[x].SetActive(false);
    }
}