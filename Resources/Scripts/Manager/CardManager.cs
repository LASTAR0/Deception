
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CardManager : UdonSharpBehaviour
{
    public Material[] matCT, matE;
    public int curCT, curE;

    [UdonSynced, HideInInspector] public int[] randCT;
    [UdonSynced, HideInInspector] public int[] randE;

    // @Master
    public void Shuffle() {
        int i = randCT.Length;
        while (i > 1) {
            --i;
            int k = Random.Range(0, i + 1);
            int temp = randCT[i];
            randCT[i] = randCT[k];
            randCT[k] = temp;
        }

        i = randE.Length;
        while (i > 1) {
            --i;
            int k = Random.Range(0, i + 1);
            int temp = randE[i];
            randE[i] = randE[k];
            randE[k] = temp;
        }
        // Sync
        RequestSerialization();
    }
    
    // @Local
    public Material getCT() {
        return matCT[randCT[curCT++]];
    }
    public Material getE() {
        return matE[randE[curE++]];
    }

    public void Reset() {
        randCT = new int[matCT.Length];
        randE = new int[matE.Length];
        for (int i = 0; i < randCT.Length; ++i) { randCT[i] = i; }
        for (int i = 0; i < randE.Length; ++i) { randE[i] = i; }
    }
    private void Start() {
        Reset();
    }
}
