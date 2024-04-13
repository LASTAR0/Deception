
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SlotManager : UdonSharpBehaviour
{
    public Image location;
    public Image[] hints;

    public Material[] matLocations;
    public Material[] matHints;
    public Material[] matRandHints;

    [UdonSynced] public int curLocation, curHint;
    [UdonSynced] public int[] randHints;
    public int curHintLocal;

    // @ Master : Shuffle
    public void Shuffle() {
        curLocation = 0;
        curHint = 0;

        randHints = new int[matHints.Length];
        for (int i = 0; i < randHints.Length; i++) { randHints[i] = i; }
        int len = randHints.Length;
        while (len > 1) {
            --len;
            int k = Random.Range(0, len + 1);
            int temp = randHints[len];
            randHints[len] = randHints[k];
            randHints[k] = temp;
        }
        RequestSerialization();
    }
    // @ All : After FrameSkip, Set materials
    public void InitSetting() {
        foreach (Image e in hints) {
            e.material = matHints[randHints[curHintLocal++]];
        }
    }
    public void ClickUp() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "LocationUp");
    }
    public void ClickDown() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "LocationDown");
    }
    public void ClickRandom_1() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "RandomToOwner_1");
    }
    public void ClickRandom_2() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "RandomToOwner_2");
    }
    public void ClickRandom_3() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "RandomToOwner_3");
    }
    public void ClickRandom_4() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "RandomToOwner_4");
    }
    public void LocationUp() {
        if (curLocation < matLocations.Length - 1) {
            curLocation++;
        }
        RequestSerialization();
    }
    public void LocationDown() {
        if (curLocation > 0) {
            curLocation--;
        }
        RequestSerialization();
    }

    // @ Master
    public void RandomToOwner_1() {
        curHint = ++curHintLocal;
        RequestSerialization();
        SendCustomEventDelayedFrames("FrameSkip_1", 15);
    }
    public void RandomToOwner_2() {
        curHint = ++curHintLocal;
        RequestSerialization();
        SendCustomEventDelayedFrames("FrameSkip_2", 15);
        
    }
    public void RandomToOwner_3() {
        curHint = ++curHintLocal;
        RequestSerialization();
        SendCustomEventDelayedFrames("FrameSkip_3", 15);
        
    }
    public void RandomToOwner_4() {
        curHint = ++curHintLocal;
        RequestSerialization();
        SendCustomEventDelayedFrames("FrameSkip_4", 15);
    }
    // @ Master, Frame Skip for Sync
    public void FrameSkip_1() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeRandom_1");
    }
    public void FrameSkip_2() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeRandom_2");
    }
    public void FrameSkip_3() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeRandom_3");
    }
    public void FrameSkip_4() {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeRandom_4");
    }
    // @ All
    public void ChangeRandom_1() {
        hints[0].material = matHints[randHints[curHintLocal - 1]];
    }
    public void ChangeRandom_2() {
        hints[1].material = matHints[randHints[curHintLocal - 1]];
    }
    public void ChangeRandom_3() {
        hints[2].material = matHints[randHints[curHintLocal - 1]];
    }
    public void ChangeRandom_4() {
        hints[3].material = matHints[randHints[curHintLocal - 1]];
    }

    public override void OnPreSerialization() {
        location.material = matLocations[curLocation];
        curHintLocal = curHint;
        Debug.Log($"curHintLocal : {curHintLocal}");
    }
    public override void OnDeserialization() {
        location.material = matLocations[curLocation];
        curHintLocal = curHint;
        Debug.Log($"curHintLocal : {curHintLocal}");
    }
}
