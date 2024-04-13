
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
    public GUIManager guiManager;

    public void Play() {
        playerManager.CheckJoinPlayer();
        cardManager.Shuffle();
        jobManager.Shuffle();
        slotManager.Shuffle();


        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetCardImage");
        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetJobImage");

        SendCustomEventDelayedFrames("FrameSkip", 15);
    }
    public void FrameSkip() {
        slotManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InitSetting");
    }
    void Start()
    {
        
    }
}
