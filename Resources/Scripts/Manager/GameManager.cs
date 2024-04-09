
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GameManager : UdonSharpBehaviour
{
    public PlayerManager playerManager;
    public CardManager cardManager;
    public SlotManager slotManager;
    public JobManager jobManager;

    public void Play() {
        playerManager.CheckJoinPlayer();
        cardManager.Shuffle();
        jobManager.Shuffle();
        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetCardImage");
        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetJobImage");
    }
    void Start()
    {
        
    }
}
