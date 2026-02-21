using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

[RequireComponent(typeof(Collider))]
public class TutorialTeleportZone : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private GameObject visualsRoot;
    [SerializeField] private TeleportationAnchor teleportationAnchor;
    [SerializeField] private bool setColliderAsTrigger = false;
    [SerializeField] private Behaviour[] enableComponents;
    [SerializeField] private GameObject[] enableObjects;

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

        if (teleportationAnchor == null)
            teleportationAnchor = GetComponent<TeleportationAnchor>();

        if (triggerCollider != null && setColliderAsTrigger)
            triggerCollider.isTrigger = true;
    }

    private void OnEnable()
    {
        if (teleportationAnchor != null)
            teleportationAnchor.teleporting.AddListener(HandleTeleporting);
    }

    private void OnDisable()
    {
        if (teleportationAnchor != null)
            teleportationAnchor.teleporting.RemoveListener(HandleTeleporting);
    }

    public void SetEnabled(bool enabled)
    {
        if (triggerCollider != null)
            triggerCollider.enabled = enabled;

        if (visualsRoot != null)
            visualsRoot.SetActive(enabled);

        if (enableComponents != null)
        {
            for (int i = 0; i < enableComponents.Length; i++)
            {
                if (enableComponents[i] != null)
                    enableComponents[i].enabled = enabled;
            }
        }

        if (enableObjects != null)
        {
            for (int i = 0; i < enableObjects.Length; i++)
            {
                if (enableObjects[i] != null)
                    enableObjects[i].SetActive(enabled);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null)
            return;

        if (!manager.IsPlayerCollider(other))
            return;

        manager.NotifyTeleportZoneEntered(zoneIndex);
    }

    private void HandleTeleporting(TeleportingEventArgs args)
    {
        if (manager == null)
            return;

        if (teleportationAnchor != null && !teleportationAnchor.enabled)
            return;

        if (triggerCollider != null && !triggerCollider.enabled)
            return;

        Debug.Log($"Tutorial teleport zone reached: {gameObject.name} (index {zoneIndex}).", this);
        manager.NotifyTeleportZoneEntered(zoneIndex);
    }
}
