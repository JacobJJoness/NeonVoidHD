using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mission
{
    public string MissionName;
    public bool IsCompleted;

    public Mission(string name)
    {
        MissionName = name;
        IsCompleted = false;
    }
}

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;
    public TMP_Text currentMissionText;  // Direct reference to the TextMeshPro UI element

    private List<string> missionOrder = new List<string>(); // List to keep missions in order
    internal Dictionary<string, bool> Missions = new Dictionary<string, bool>(); // Dictionary to store mission status, accessible within the same assembly
    private int currentMissionIndex = 0; // Index to track the current mission

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes it persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        missionOrder.Add("Jump");
        missionOrder.Add("Dash");
        missionOrder.Add("Move");
        missionOrder.Add("Punch a Robot to Death");
        InitializeMissions();
        UpdateMissionDisplay(); // Ensure the first mission is displayed on start
    }

    void InitializeMissions()
    {
        foreach (var missionName in missionOrder)
        {
            Missions.Add(missionName, false);
        }
    }

    public void AddMission(string missionName)
    {
        if (!Missions.ContainsKey(missionName))
        {
            Missions.Add(missionName, false);
            missionOrder.Add(missionName);
            UpdateMissionDisplay();
        }
        else
        {
            Debug.Log("Mission already exists.");
        }
    }

    public bool AreAllMissionsComplete()
    {
        foreach (var status in Missions.Values)
        {
            if (!status) return false;
        }
        return true;
    }



    void UpdateMissionDisplay()
    {
        if (currentMissionIndex < missionOrder.Count)
        {
            string currentMission = missionOrder[currentMissionIndex];
            currentMissionText.text = $"{currentMission}: Incomplete";
            currentMissionText.color = Color.black; // Set text color to black for incomplete missions
            Debug.Log("Displaying Mission: " + currentMission);
        }
        else
        {
            currentMissionText.text = "All missions completed!";
            currentMissionText.color = Color.cyan; // Optional color change to indicate completion
            Debug.Log("No more missions to display.");
        }
    }

    public void CompleteMission(string missionKey)
    {
        Debug.Log("Attempting to complete mission: " + missionKey);
        if (Missions.ContainsKey(missionKey) && !Missions[missionKey])
        {
            Missions[missionKey] = true;
            currentMissionText.text = $"{missionKey}: Complete";
            currentMissionText.color = Color.green;  // Change text color to green
            Debug.Log($"Mission '{missionKey}' completed!");
            StartCoroutine(WaitAndShowNextMission(5.0f)); // Wait for 5 seconds before showing next mission
        }
        else
        {
            Debug.Log("Mission completion failed. Either not found or already completed.");
        }
    }

    IEnumerator WaitAndShowNextMission(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Find the next incomplete mission
        while (currentMissionIndex < missionOrder.Count && Missions[missionOrder[currentMissionIndex]])
        {
            currentMissionIndex++;
        }
        UpdateMissionDisplay(); // Update the display to the next mission
    }
}
