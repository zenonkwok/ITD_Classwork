using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementTutorialManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private string playerTag = "Player";

    [Header("Trigger Zones")]
    [SerializeField] private List<TutorialTriggerZone> triggerZones = new List<TutorialTriggerZone>();

    [Header("Teleport Zones")]
    [SerializeField] private List<TutorialTeleportZone> teleportZones = new List<TutorialTeleportZone>();

    [Header("Events")]
    [SerializeField] private UnityEvent onTutorialComplete = new UnityEvent();

    private int currentTriggerIndex;
    private int currentTeleportIndex = -1;
    private bool triggersComplete;
    private bool tutorialComplete;

    private void Awake()
    {
        for (int i = 0; i < triggerZones.Count; i++)
        {
            if (triggerZones[i] != null)
                triggerZones[i].Configure(this, i);
        }

        for (int i = 0; i < teleportZones.Count; i++)
        {
            if (teleportZones[i] != null)
                teleportZones[i].Configure(this, i);
        }
    }

    private void Start()
    {
        currentTriggerIndex = 0;
        triggersComplete = triggerZones.Count == 0;

        UpdateTriggerZoneStates();
        UpdateTeleportZoneStates();
    }

    public void NotifyTriggerZoneEntered(int zoneIndex)
    {
        if (triggersComplete)
            return;

        if (zoneIndex != currentTriggerIndex)
            return;

        if (triggerZones[currentTriggerIndex] != null)
            triggerZones[currentTriggerIndex].SetEnabled(false);

        currentTriggerIndex++;
        if (currentTriggerIndex >= triggerZones.Count)
        {
            triggersComplete = true;
            currentTeleportIndex = teleportZones.Count > 0 ? 0 : -1;
            UpdateTeleportZoneStates();
            return;
        }

        UpdateTriggerZoneStates();
    }

    public void NotifyTeleportZoneEntered(int zoneIndex)
    {
        if (!triggersComplete)
            return;

        if (zoneIndex != currentTeleportIndex)
            return;

        if (currentTeleportIndex >= 0 && currentTeleportIndex < teleportZones.Count)
        {
            if (teleportZones[currentTeleportIndex] != null)
                teleportZones[currentTeleportIndex].SetEnabled(false);
        }

        currentTeleportIndex++;
        UpdateTeleportZoneStates();

        if (currentTeleportIndex >= teleportZones.Count && !tutorialComplete)
        {
            tutorialComplete = true;
            onTutorialComplete.Invoke();
        }
    }

    public UnityEvent OnTutorialComplete => onTutorialComplete;

    public bool IsPlayerCollider(Collider other)
    {
        if (other == null)
            return false;

        if (playerRoot != null)
            return other.transform.IsChildOf(playerRoot);

        return other.CompareTag(playerTag);
    }

    private void UpdateTriggerZoneStates()
    {
        for (int i = 0; i < triggerZones.Count; i++)
        {
            if (triggerZones[i] == null)
                continue;

            bool shouldEnable = !triggersComplete && i == currentTriggerIndex;
            triggerZones[i].SetEnabled(shouldEnable);
        }
    }

    private void UpdateTeleportZoneStates()
    {
        for (int i = 0; i < teleportZones.Count; i++)
        {
            if (teleportZones[i] == null)
                continue;

            bool shouldEnable = triggersComplete && i == currentTeleportIndex;
            teleportZones[i].SetEnabled(shouldEnable);
        }
    }
}
