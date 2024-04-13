
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class murderphase : UdonSharpBehaviour
{
    public GUIManager gUIManager;

    public override void Interact() {
        gUIManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "BlindMurdererPhase");
    }
}
