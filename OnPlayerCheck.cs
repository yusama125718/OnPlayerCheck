
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
    
    [UdonSynced]private bool[] active = new bool[2]

    async void start()
    {
        active[0] = true;
        active[1] = false;
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
            active[0] = false;
            active[1] = true;
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
        if (OPCstayplayer <= triggercount) return;
        active[0] = false;
        active[1] = true;
        RequestSerialization();
        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "hideini");
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "showini");
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    { 
        if (!Networking.IsOwner(gameObject)) return;
        OPCstayplayer--;
        if (OPCstayplayer > triggercount) return;
        active[0] = true;
        active[1] = false;
        RequestSerialization();
        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "hideini");
            }
        }
        for (int i=0;i<showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                x = i;
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "showini");
            }
        }
    }
    
    public void showini()
    {
        showobject[x].SetActive(active[0]);
    }
    
    public void hideini()
    {
        hideobject[x].SetActive(active[1]);
    }
}
