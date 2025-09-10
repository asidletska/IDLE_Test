using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    private int currentIndex = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (waypoints.Length > 0)
            StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Transform target = waypoints[currentIndex];

            // �������� �� �����
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;

                // ̳����� �������� ������� NPC (��� 2D ��� ����������)
                if (dir.x != 0)
                    transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

                // ������� ������� ������
                animator?.SetBool("walk", true);

                yield return null;
            }

            // ������� � �����
            animator?.SetBool("walk", false);
            animator?.SetBool("idle", true);
            yield return new WaitForSeconds(waitTime);

            // ���������� �� �������� �����
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }
}
