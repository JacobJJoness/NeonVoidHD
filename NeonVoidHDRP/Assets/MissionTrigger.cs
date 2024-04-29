using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public string missionToComplete = "Join the Portal";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has a tag "Player"
        {
            MissionManager.Instance.CompleteMission(missionToComplete);
        }
    }
}
