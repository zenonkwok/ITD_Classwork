using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TutorialTriggerZone : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private GameObject visualsRoot;

    private MovementTutorialManager manager;
    private int zoneIndex;

    public void Configure(MovementTutorialManager tutorialManager, int index)
    {
        manager = tutorialManager;
        zoneIndex = index;
    }

    private void Awake()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider>();

        if (triggerCollider != null)
            triggerCollider.isTrigger = true;
    }

    public void SetEnabled(bool enabled)
    {
        if (triggerCollider != null)
            triggerCollider.enabled = enabled;

        if (visualsRoot != null)
            visualsRoot.SetActive(enabled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null)
            return;

        if (!manager.IsPlayerCollider(other))
            return;

        manager.NotifyTriggerZoneEntered(zoneIndex);
    }
}
