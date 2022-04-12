
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OnPlayerCheck : UdonSharpBehaviour
{
    [Header("トリガー人数")]
    [SerializeField, Range(0, 100)] private int triggercount;

    [Header("トリガー人数より多いとき表示するオブジェクト")]
    [SerializeField] private GameObject[] hideobject;

    [Header("トリガー人数以下のとき表示するオブジェクト")]
    [SerializeField] private GameObject[] showobject;

    [UdonSynced]private int OPCstayplayer;
    
    [UdonSynced]private bool active;

    void start()
    {
        active = true;
    }

    void OnPlayerJoin(VRCPlayerApi player){
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OPCchange");
    }

    void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);
        OPCstayplayer++;
        if (OPCstayplayer <= triggercount) return;
        active = false;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OPCchange");
    }

    void OnPlayerTriggerExit(VRCPlayerApi player)
    { 
        if (!Networking.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);
        OPCstayplayer--;
        if (OPCstayplayer > triggercount) return;
        active = true;
        RequestSerialization();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OPCchange");
    }
    
    public void OPCchange()
    {
        for (int i = 0;i < showobject.Length;i++)
        {
            if (showobject[i] != null)
            {
                showobject[i].SetActive(active);
            }
        }
        for (int i=0;i<hideobject.Length;i++)
        {
            if (hideobject[i] != null)
            {
                hideobject[i].SetActive(!active);
            }
        }
    }
}