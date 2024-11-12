using UnityEngine;
using TMPro;

public class DartboardCollision : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject dartPrefab;            // Reference to the dart prefab

    [Header("Score Zones")]
    public float innerRadius = 0.25f;
    public float middleRadius = 1.45f;
    public float outerRadius = 2.25f;

    public int innerScore = 50;
    public int middleScore = 30;
    public int outerScore = 10;

    [Header("Audio Clips")]
    public AudioClip innerHitSound;          // Sound for the inner circle
    public AudioClip middleHitSound;         // Sound for the middle circle
    public AudioClip outerHitSound;          // Sound for the outer circle

    private AudioSource audioSource;         // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component attached to the Dartboard
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Dart"))
        {
            DartFlickThrow dartScript = collision.GetComponent<DartFlickThrow>();
            if (dartScript != null)
            {
                // dartScript.StopDart();  // Stop the dart's motion when it hits the board
            }

            // Calculate score based on distance
            float distance = Vector3.Distance(collision.transform.position, transform.position);
            int score = 0;

            // Play the appropriate sound and calculate the score
            if (distance <= innerRadius)
            {
                score = innerScore;
                PlaySound(innerHitSound);  // Play sound for inner radius hit
            }
            else if (distance <= middleRadius)
            {
                score = middleScore;
                PlaySound(middleHitSound); // Play sound for middle radius hit
            }
            else if (distance <= outerRadius)
            {
                score = outerScore;
                PlaySound(outerHitSound);  // Play sound for outer radius hit
            }

            scoreManager.IncreaseScore(score);

            // Spawn a new dart after a short delay
            // Invoke(nameof(SpawnNewDart), 1f);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        // Play the given audio clip
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Uncomment and modify to spawn a new dart if needed
    // private void SpawnNewDart()
    // {
    //     GameObject newDart = Instantiate(dartPrefab, initialDartPosition.position, initialDartPosition.rotation);
    //     newDart.transform.localScale = initialDartPosition.localScale;
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, innerRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, middleRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}