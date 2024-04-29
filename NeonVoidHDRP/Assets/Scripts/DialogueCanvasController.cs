using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvasController : MonoBehaviour
{
    public delegate void MissionAcceptedHandler(bool isAccepted);
    public event MissionAcceptedHandler OnMissionAccepted;

    public GameObject canvas;
    public TMP_Text missionDescriptionText;
    public Button acceptButton;
    public Button declineButton;

    private Mission currentMission;

    void Start()
    {
        canvas.SetActive(false);
        acceptButton.onClick.AddListener(AcceptMission);
        declineButton.onClick.AddListener(DeclineMission);
    }

    public void ShowMission(Mission mission)
    {
        currentMission = mission;
        missionDescriptionText.text = mission.MissionName;
        canvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void AcceptMission()
    {
        if (currentMission != null)
        {
            MissionManager.Instance.AddMission(currentMission.MissionName);
            OnMissionAccepted?.Invoke(true);
        }
        CloseDialogue();
    }

    private void DeclineMission()
    {
        OnMissionAccepted?.Invoke(false);
        CloseDialogue();
    }

    private void CloseDialogue()
    {
        canvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
