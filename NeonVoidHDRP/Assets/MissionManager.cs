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
    private Dictionary<string, bool> Missions = new Dictionary<string, bool>(); // Dictionary to store mission status
    private int currentMissionIndex = 0; // Index to track the current mission

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Makes it persist across scenes
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

    void UpdateMissionDisplay()
    {
        if (currentMissionIndex < missionOrder.Count)
        {
            string currentMission = missionOrder[currentMissionIndex];
            currentMissionText.text = $"{currentMission}: Incomplete";
            currentMissionText.color = Color.black; // Set text color to black for incomplete missions
        }
        else
        {
            currentMissionText.text = "All missions completed!";
            currentMissionText.color = Color.cyan; // Optional color change to indicate completion
        }
    }

    public void CompleteMission(string missionKey)
    {
        if (Missions.ContainsKey(missionKey) && missionKey == missionOrder[currentMissionIndex]) // Check if the mission exists and is the current mission
        {
            Missions[missionKey] = true;
            currentMissionText.text = $"{missionKey}: Complete";
            currentMissionText.color = Color.green;  // Change text color to green
            Debug.Log($"Mission '{missionKey}' completed!");
            StartCoroutine(WaitAndShowNextMission(5.0f)); // Wait for 5 seconds before showing next mission
        }
    }

    IEnumerator WaitAndShowNextMission(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentMissionIndex++;
        UpdateMissionDisplay(); // Update the display to the next mission
    }
}