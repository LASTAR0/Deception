
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Player[] players;
    [UdonSynced] public int[] joinedPlayerId;

    [UdonSynced, HideInInspector] public int cntPlayers;

    public void addPlayer() {
        cntPlayers++;
        RequestSerialization();
    }
    public void subPlayer() {
        cntPlayers--;
        RequestSerialization();
    }
    // @ Master
    public void CheckJoinPlayer() {
        int[] temp = new int[players.Length];
        int max = 0;
        foreach (Player p in players) {
            if (!p.IsJoined) {
                p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideBoard");
            } else {
                temp[max++] = p.id;
            }
        }
        joinedPlayerId = new int[max];
        System.Array.Copy(temp, joinedPlayerId, max);
    }
    // @ All
    public void SetCardImage() {
        foreach(Player p in players) {
            // if joined
            if (!p.IsJoined) continue;
            p.SetCT();
            p.SetE();
        }
    }
    public void SetJobImage() {
        foreach(Player p in players) {
            if (!p.IsJoined || p.id != Networking.LocalPlayer.playerId)
                continue;
            p.SetJob();
        }
    }
    // @ All : Joined && p.id same
    public void ConfirmJob() {
        foreach (Player p in players) {
            if (!p.IsJoined || p.id != Networking.LocalPlayer.playerId)
                continue;
            p.ConfirmJob();
        }
    }
}
