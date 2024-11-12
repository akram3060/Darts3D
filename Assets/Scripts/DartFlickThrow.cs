using UnityEngine;
using TMPro;

public class DartFlickThrow : MonoBehaviour
{
    private Vector2 startInputPos;
    private Vector2 endInputPos;
    private float startTime;
    private float endTime;
    private Rigidbody rb;

    [SerializeField]
    private float forceMultiplier = 0.002f;   // Controls throw strength
    [SerializeField]
    private TMP_Text scoreText;               // Reference to score UI text

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private bool isThrown = false;

    [SerializeField]
    private float maxDistance = 15f;          // Max distance allowed before dart resets

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    private void Update()
    {
        // Check for touch input on mobile
        if (Input.touchCount > 0 && !isThrown)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startInputPos = touch.position;
                startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endInputPos = touch.position;
                endTime = Time.time;
                ThrowDart();
            }
        }
        // Check for mouse input on PC
        else if (Input.GetMouseButtonDown(0) && !isThrown)
        {
            startInputPos = Input.mousePosition;
            startTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0) && !isThrown)
        {
            endInputPos = Input.mousePosition;
            endTime = Time.time;
            ThrowDart();
        }
    }

    private void ThrowDart()
    {
        Vector2 flickDirection = (endInputPos - startInputPos).normalized;
        float flickSpeed = (endInputPos - startInputPos).magnitude / (endTime - startTime);

        // Convert Vector2 direction to a 3D direction for force application
        Vector3 forceDirection = new Vector3(flickDirection.x, flickDirection.y, 1).normalized;
        float appliedForce = flickSpeed * forceMultiplier;

        rb.useGravity = true;
        rb.AddForce(forceDirection * appliedForce, ForceMode.Impulse);

        isThrown = true;
    }

    private void LateUpdate()
    {
        // Keep the dart from rotating after it’s thrown
        transform.rotation = initialRotation;

        // Reset dart if it exceeds max distance
        if (isThrown && Vector3.Distance(transform.position, initialPosition) > maxDistance)
        {
            ResetDart();
        }
    }

    private void ResetDart()
    {
        // Reset dart to its initial position and state
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = false;
        isThrown = false;
    }
}