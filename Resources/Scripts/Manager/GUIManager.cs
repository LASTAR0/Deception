
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GUIManager : UdonSharpBehaviour
{
    public GameManager manager;
    public DarkMode darkMode;

    public void BlindMurdererPhase() {
        PlayerManager pm = manager.playerManager;
        foreach (Player p in pm.players) {
            // 자리 플레이어만
            if (p.IsJoined && p.id == Networking.LocalPlayer.playerId) {
                if (p.job != (int)JOB.MURDERER && p.job != (int)JOB.ACCOMPLICE && p.job != (int)JOB.SCIENTIST) {
                    darkMode.BlindStart();
                }
            }
        }
    }
    public void BlindWitnessPhase() {
        PlayerManager pm = manager.playerManager;
        foreach (Player p in pm.players) {
            if (p.IsJoined && p.id == Networking.LocalPlayer.playerId) {
                if (p.job != (int)JOB.WITNESS && p.job != (int)JOB.SCIENTIST) {
                    darkMode.BlindStart();
                }
            }
        }
    }
    public void BlindEndPhase() {
        PlayerManager pm = manager.playerManager;
        foreach (Player p in pm.players) {
            if (p.IsJoined && p.id == Networking.LocalPlayer.playerId) {
                darkMode.BlindEnd();
            }
        }
    }
}
