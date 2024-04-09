
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
    DETECTIVE = 5
}

public class JobManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Sprite[] Jobs;
    [UdonSynced] public int[] shuffledJobs;

    public void Shuffle() {
        Setting();
        int i = shuffledJobs.Length;
        while (i > 1) {
            --i;
            int k = Random.Range(0, i + 1);
            int temp = shuffledJobs[i];
            shuffledJobs[i] = shuffledJobs[k];
            shuffledJobs[k] = temp;
        }
        
        RequestSerialization();
    }
    private void Setting() {
        int num = manager.playerManager.cntPlayers;
        shuffledJobs = new int[num];
        if (num < 6) {
            for (int i = 0; i < num; ++i) {
                if (i < 2) { shuffledJobs[i] = i; } else { shuffledJobs[i] = (int)JOB.INVESTIGATOR; }
            }
        } else {
            for (int i = 0; i < num; ++i) {
                if (i < 2) { shuffledJobs[i] = i; } else if (i == 2) { shuffledJobs[i] = (int)JOB.ACCOMPLICE; } else if (i == 3) { shuffledJobs[i] = (int)JOB.WITNESS; } else { shuffledJobs[i] = (int)JOB.INVESTIGATOR; }
            }
        }
    }
}
