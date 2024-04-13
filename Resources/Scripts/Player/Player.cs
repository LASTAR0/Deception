
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class Player : UdonSharpBehaviour
{
    public PlayerManager playerManager;
    public int index = 0;
    public Image[] CT, E;
    public Image imgJob;
    public GameObject joinButton, cards, confirm, refuse, accept;

    [UdonSynced,HideInInspector] public bool IsJoined = false;
    [UdonSynced] public int id = -1;
    [UdonSynced] public int job = (int)JOB.NOTHING;

    public void Join() {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowBoard");
        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "addPlayer");
        IsJoined = true;
        id = Networking.LocalPlayer.playerId;
        RequestSerialization();
    }
    public void Sync() {
        Debug.Log($"id : {id} , job : {job}");
        Debug.Log($"{gameObject.name} Owner : {Networking.GetOwner(gameObject).playerId}");
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
        if (Networking.LocalPlayer.playerId != id)
            return;
        JobManager jm = playerManager.manager.jobManager;
        job = jm.shuffledJobs[index];
        imgJob.sprite = jm.Jobs[jm.shuffledJobs[index]];
        if (job == (int)JOB.SCIENTIST) {
            ShowScientistSelect();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowAllScientist");
        }
        Debug.Log($"your job is {job}");
        RequestSerialization();
    }

    // @ Local : Confirm Change Scientist
    public void ConfirmJob() {
        if (Networking.LocalPlayer.playerId != id)
            return;
        JobManager jm = playerManager.manager.jobManager;
        imgJob.sprite = jm.Jobs[job];
        if (job == (int)JOB.SCIENTIST) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShowAllScientist");
            // Show All Players Jobs (To Scientist)
            Debug.Log("Show All Players Jobs");
            SendCustomEventDelayedSeconds("ConfirmShowAllJobsToScientist", 1.0f);
        } else {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideJob");
        }
        Debug.Log($"Confirm : your job is {job}");
    }
    public void ConfirmShowAllJobsToScientist() {
        foreach (Player p in playerManager.players) {
            if (p.IsJoined && p.job != (int)JOB.SCIENTIST) {
                p.imgJob.sprite = playerManager.manager.jobManager.Jobs[p.job];
            }
        }
    }

    // @ All : Show Scientist To All
    public void ShowAllScientist() {
        JobManager jm = playerManager.manager.jobManager;
        imgJob.sprite = jm.Jobs[(int)JOB.SCIENTIST];
    }
    // @ All : Hide Scientist To All
    public void HideJob() {
        if (id == Networking.LocalPlayer.playerId)
            return;
        JobManager jm = playerManager.manager.jobManager;
        imgJob.sprite = jm.Jobs[(int)JOB.SECRET];
    }

    // 법의학자 확정 -> 다른 모든플레이어 직업이 보임
    public void ConfirmScientist() {
        JobManager jm = playerManager.manager.jobManager;
        foreach (Player p in playerManager.players) {
            if (p.IsJoined) {
                p.imgJob.sprite = jm.Jobs[jm.shuffledJobs[p.index]];
            }
        }
        HideScientistSelect();
    }
    // 법의학자 거절 -> 다른 모든 플레이어에게 법의학자 받겠다는 버튼이 뜨게 만듬
    public void RefuseScientist() {
        Debug.Log("Refuse Select");
        foreach(Player p in playerManager.players) {
            Debug.Log($"{p.name} : {p.id}");
            if (p.IsJoined && p.id != Networking.LocalPlayer.playerId) {
                p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowScientistAccept");
            }
        }
        HideScientistSelect();
    }
    public void ShowScientistAccept() {
        Debug.Log("Show Accept Select");
        accept.SetActive(true);
    }
    public void AcceptScientist() {
        foreach(Player p in playerManager.players) {
            if (p.IsJoined) {
                p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideAccept");
            }
        }
        // 법의학자 받고 job 교체 후 이미지 재조정
        foreach(Player p in playerManager.players) {
            if (p.IsJoined && p.job == (int)JOB.SCIENTIST) {
                // 법의학자 플레이어 찾기
                int temp = job;
                job = (int)JOB.SCIENTIST;
                // switch 로 각각의 직업에 해당하는걸 owner 에게 해당직업으로 변환 시도.
                switch (temp) {
                    case (int)JOB.MURDERER:
                        p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ChangeJobToMurderer");
                        break;
                    case (int)JOB.INVESTIGATOR:
                        p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ChangeJobToInvestigator");
                        break;
                    case (int)JOB.ACCOMPLICE:
                        p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ChangeJobToAccomplice");
                        break;
                    case (int)JOB.WITNESS:
                        p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ChangeJobToWitness");
                        break;
                    case (int)JOB.DETECTIVE:
                        p.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ChangeJobToDetective");
                        break;
                }
                // Sync 
                RequestSerialization();
            }
        }

        playerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ConfirmJob");
    }
    public void ChangeJobToMurderer() {
        job = (int)JOB.MURDERER;
        RequestSerialization();
        Debug.Log($"Now your job is {job}");
    }
    public void ChangeJobToInvestigator() {
        job = (int)JOB.INVESTIGATOR;
        RequestSerialization();
        Debug.Log($"Now your job is {job}");
    }
    public void ChangeJobToAccomplice() {
        job = (int)JOB.ACCOMPLICE;
        RequestSerialization();
        Debug.Log($"Now your job is {job}");
    }
    public void ChangeJobToWitness() {
        job = (int)JOB.WITNESS;
        RequestSerialization();
        Debug.Log($"Now your job is {job}");
    }
    public void ChangeJobToDetective() {
        job = (int)JOB.DETECTIVE;
        RequestSerialization();
        Debug.Log($"Now your job is {job}");
    }
    public void HideAccept() {
        accept.SetActive(false);
    }
    private void ShowScientistSelect() {
        confirm.SetActive(true);
        refuse.SetActive(true);
    }
    private void HideScientistSelect() {
        confirm.SetActive(false);
        refuse.SetActive(false);
    }

    public override void OnPreSerialization() {
    }
    public override void OnDeserialization() {
    }
}
