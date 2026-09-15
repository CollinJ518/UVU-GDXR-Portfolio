using UnityEngine;

// Attach this to a small CHILD object positioned at the TOP of the player
// (e.g. Player > HeadZone), with its own Collider2D set to "Is Trigger".
// This way, only objects falling onto the player's head deal damage —
// side or bottom contact with the main player collider does nothing.
public class PlayerHeadHitbox : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        // Looks for PlayerController on this object or any parent
        playerController = GetComponentInParent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogWarning("PlayerHeadHitbox: No PlayerController found in parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("FallingObject")) return;
        if (playerController == null) return;
        if (playerController.IsInvincible()) return;

        playerController.TakeHit();
        Destroy(other.gameObject);
    }
}
