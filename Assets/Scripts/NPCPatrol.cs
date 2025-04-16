using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCPatrol : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Persecución")]
    [SerializeField] private Transform player;
    [SerializeField] private float visionRange;
    [SerializeField] private float fieldOfView;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private float chaseTime;

    [Header("Captura")]
    [SerializeField] private float captureDistance;
    [SerializeField] private Image fadeOut;
    [SerializeField] private TextMeshProUGUI dieText;
    [SerializeField] private float durationFade;

    private NavMeshAgent agent;
    private Animator animator;
    private float chaseTimer = 0f;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PatrolToNextPoint();

        if (dieText != null)
        {
            dieText.color = new Color(dieText.color.r, dieText.color.g, dieText.color.b, 0f);
        }
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            isChasing = true;
            chaseTimer = chaseTime;
            agent.destination = player.position;
        }
        else if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
            {
                isChasing = false;
                PatrolToNextPoint();
            }
        }

        if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PatrolToNextPoint();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (isChasing && distanceToPlayer < captureDistance)
        {
            StartCoroutine(FadeOut());
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void PatrolToNextPoint()
    {
        if (waypoints.Length == 0) return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private bool CanSeePlayer()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 directionToPlayer = (player.position - rayOrigin).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.DrawRay(rayOrigin, directionToPlayer * visionRange, Color.green, 1f);

        if (angle < fieldOfView / 2 && distanceToPlayer < visionRange)
        {
            if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, visionRange, visionMask))
            {
                if (hit.transform == player || hit.transform.IsChildOf(player))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator FadeOut()
    {
        float tiempo = 0f;

        Color screen = fadeOut.color;
        Color textColor = dieText.color;

        if (dieText != null)
        {
            dieText.gameObject.SetActive(true);
        }

        while (tiempo < durationFade)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Clamp01(tiempo / durationFade);

            fadeOut.color = new Color(screen.r, screen.g, screen.b, alpha);

            if (dieText != null)
                dieText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 rayOrigin = transform.position + Vector3.up * 1.2f;
        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin, leftLimit * visionRange);
        Gizmos.DrawRay(rayOrigin, rightLimit * visionRange);
    }
}
