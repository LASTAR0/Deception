
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Player : UdonSharpBehaviour
{
    public PlayerManager playerManager;
    public Image[] CT, E;
    public Image imgJob;
    public GameObject joinButton;
    public GameObject cards;

    [UdonSynced,HideInInspector] public bool IsJoined = false;
    [UdonSynced] public int id = -1;
    [UdonSynced] public JOB job = JOB.NOTHING;

    public void Join() {
        OnTakeOwnership();
        IsJoined = true;
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowBoard");
        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "addPlayer");
        id = Networking.LocalPlayer.playerId;
        RequestSerialization();
    }
    public void ShowBoard() {
        joinButton.SetActive(false);
        cards.SetActive(true);
    }
    public void HideBoard() {
        joinButton.SetActive(false);
    }
    public void SetCT() {
        foreach(Image ct in CT) {
            ct.material = playerManager.manager.cardManager.getCT();
        }
    }
    public void SetE() {
        foreach (Image e in E) {
            e.material = playerManager.manager.cardManager.getE();
        }
    }
    // @ Local
    public void SetJob() {
        if (Networking.LocalPlayer == Networking.GetOwner(gameObject)) {
            JobManager jm = playerManager.manager.jobManager;
            imgJob.sprite = jm.Jobs[jm.shuffledJobs[Networking.LocalPlayer.playerId]];
        }
    }
    public void OnTakeOwnership() {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }
}
