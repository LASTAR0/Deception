
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


/*
 * 0 - 법의학자 
 * 1 - 살인자
 * 2 - 수사관
 * 3 - 공범
 * 4 - 목격자
 * 5 - 사립탐정
 */
public enum JOB {
    NOTHING = -1,
    SCIENTIST = 0,
    MURDERER = 1,
    INVESTIGATOR = 2,
    ACCOMPLICE = 3,
    WITNESS = 4,
    DETECTIVE = 5,
    SECRET = 6
}

public class JobManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Sprite[] Jobs;
    [UdonSynced] public int[] shuffledJobs, tempJobs;

    public void Shuffle() {
        PlayerManager pm = manager.playerManager;
        int num = pm.cntPlayers;
        shuffledJobs = new int[pm.players.Length];
        tempJobs = new int[num];
        for (int i = 0; i < shuffledJobs.Length; ++i) {
            if (pm.players[i].IsJoined) { shuffledJobs[i] = 0; }
            else { shuffledJobs[i] = -1; }
        }
        // init setting from player count
        if (num < 6) {
            for (int i = 0; i < num; ++i) {
                if (i < 2) { tempJobs[i] = i; }
                else { tempJobs[i] = (int)JOB.INVESTIGATOR; }
            }
        } else {
            for (int i = 0; i < num; ++i) {
                if (i < 2) { tempJobs[i] = i; }
                else if (i == 2) { tempJobs[i] = (int)JOB.ACCOMPLICE; }
                else if (i == 3) { tempJobs[i] = (int)JOB.WITNESS; }
                else { tempJobs[i] = (int)JOB.INVESTIGATOR; }
            }
        }
        // shuffle jobs
        int jobsize = tempJobs.Length;
        while (jobsize > 1) {
            --jobsize;
            int k = Random.Range(0, jobsize + 1);
            int temp = tempJobs[jobsize];
            tempJobs[jobsize] = tempJobs[k];
            tempJobs[k] = temp;
        }

        int cur = 0;
        for (int i = 0; i < shuffledJobs.Length; ++i) {
            if (shuffledJobs[i] == 0) { shuffledJobs[i] = tempJobs[cur++]; }
        }

        RequestSerialization();
        //SendCustomEventDelayedFrames("FrameSkip", 15);        
    }
    public void FrameSkip() {
        PlayerManager pm = manager.playerManager;
        for (int i = 0; i < shuffledJobs.Length; ++i) {
            if (pm.players[i].IsJoined) {
                pm.players[i].SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetJob");
            }
        }
    }
}
