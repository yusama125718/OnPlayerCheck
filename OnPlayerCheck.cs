
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class OnPlayerCheck : UdonSharpBehaviour
{
    [Header("トリガー人数")]
    [SerializeField, Range(0, 100)] private int triggercount;

    [Header("トリガー人数より多いとき表示オブジェクト")]
    [SerializeField] private GameObject[] hideobject;

    [Header("トリガー人数以下のとき表示するオブジェクト")]
    [SerializeField] private GameObject[] showobject;

    int stayplayer;
    void OnPlayerTriggerJoin(VRCPlayerApi player)
    {
        stayplayer++;
        if (stayplayer <= triggercount)
        {
            return;
        }
        for (int i = 0;i < hideobject.Length;i++)
        {
            if (!Networking.IsOwner(hideobject[i]))
            {
                Networking.SetOwner(Networking.LocalPlayer, hideobject);
            }
        }
        for (int i = 0;i < showobject.Length;i++)
        {
            if (!Networking.IsOwner(showobject[i]))
            {
                Networking.SetOwner(Networking.LocalPlayer, showobject);
            }
        }
        for (int i=0;i>hideobject.Length;i++)
        {
            if (hideobject[i] = null)
            {
                return;
            }
            hideobject[i].SetActive(true);
        }
        for (int i=0;i>showobject.Length;i++)
        {
            if (showobject[i] = null)
            {
                return;
            }
            showobject[i].SetActive(false);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        stayplayer--;
        if (stayplayer >= triggercount)
        {
            return;
        }
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        for (int i=0;i>hideobject.Length;i++)
        {
            if (hideobject[i] = null)
            {
                return;
            }
            hideobject[i].SetActive(false);
        }
        for (int i=0;i>showobject.Length;i++)
        {
            if (showobject[i] = null)
            {
                return;
            }
            showobject[i].SetActive(true);
        }
    }
    public override void OnPlayerJoin(VRCPlayerApi, player)
    {
        //ここの処理は帰ってからやる(joinしたときの同期処理)
    }
}
