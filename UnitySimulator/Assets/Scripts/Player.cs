using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Player : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0; // 현재 웨이포인트 인덱스
    public NavMeshAgent agent; // NavMeshAgent 컴포넌트
    private float rayDistance = 1f; // Ray의 길이

    private CapsuleCollider capsuleCollider;
    private LineRenderer lineRenderer;
    public Animator animator;
    private Rigidbody rb;
    private WeatherManager wm;
    private LiftManager lm;
    private ItemManager im;

    private int pushcount = 0;
    private bool ignoreHit = false; // 충돌을 무시하는지 여부를 나타내는 변수
    private float ignoreDuration = 7f; // 충돌을 무시할 기간 (초)
    public bool isHeavy = false;
    public string startAnimation = "WalkFwdLoop"; // 기본 시작 애니메이션

    public string targetTag = "Worker"; // 버튼 클릭 이벤트를 적용할 프리팹 인스턴스의 태그

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from the player.");
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        wm = GameObject.FindObjectOfType<WeatherManager>();
        lm = GameObject.FindObjectOfType<LiftManager>();
        im = GameObject.FindObjectOfType<ItemManager>();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints array is not set or empty.");
            return;
        }

        // 초기 waypoint로 이동 설정
        SetNextWaypoint();

        lineRenderer.positionCount = 2; // 시작점과 끝점으로 이루어진 선
        lineRenderer.startWidth = 0.05f; // 선의 시작 너비
        lineRenderer.endWidth = 0.05f; // 선의 끝 너비

        if (gameObject.CompareTag("Worker2") || gameObject.CompareTag("Worker3") || gameObject.CompareTag("Worker4"))
        {
            startAnimation = "WalkFwdStop"; 
            animator.Play(startAnimation);
            agent.isStopped = true; // 시작 시 정지
            if (gameObject.CompareTag("Worker4"))
            {
                transform.position = new Vector3(129, 3.3f, 118);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                agent.enabled = false;
                rb.isKinematic = false;
                rb.useGravity = true;
                capsuleCollider.enabled = true;
            }
        }
        if (gameObject.CompareTag("Worker5"))
        {
            string[] animations = { "Sitting", "Sitting1", "Sitting2" };

            startAnimation = animations[Random.Range(0, animations.Length)];
            animator.Play(startAnimation);
            agent.isStopped = true; // 시작 시 정지
            agent.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            capsuleCollider.enabled = true;
            capsuleCollider.height /= 5f;
            capsuleCollider.center /= 2f;
        }
        if (gameObject.CompareTag("Worker6"))
        {
            string[] animations = { "Laying1", "Laying2" };
            startAnimation = animations[Random.Range(0, animations.Length)];
            animator.Play(startAnimation);
            agent.isStopped = true; // 시작 시 정지
            agent.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            capsuleCollider.enabled = true;
            StartCoroutine(RunRollingAnimAfterDelay(3f));
        }
    }

    public void Update()
    {
        if (ignoreHit)
            return;

        if (agent.enabled && agent.remainingDistance < agent.stoppingDistance)
        {
            SetNextWaypoint();
        }
        if (gameObject.CompareTag("Worker4"))
        {
            if (im.item4.transform.position.y < 2.8)
            {
                animator.Play("Death");
            }
        }

        PerformRaycast();
    }

    public void PerformRaycast()
    {
        Vector3 playerCenter = transform.position + transform.up * GetComponent<Collider>().bounds.extents.y;

        if (Physics.Raycast(playerCenter, transform.forward, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Stop"))
            {
                HandleStopHit();
            }
            else if (hit.collider.CompareTag("Lift"))
            {
                HandleLiftHit();
            }
            else if (hit.collider.CompareTag("Break"))
            {
                if (gameObject.CompareTag("Worker2"))
                {
                    animator.Play("Death");
                }
                else if (gameObject.CompareTag("Worker3"))
                {
                    animator.Play("Knockdown_Right");
                }
                StartCoroutine(ChangeColliderSizeAfterDelay(0.5f));
            }
        }
    }

    private void HandleStopHit()
    {
        ignoreHit = true;
        StartCoroutine(ResetIgnoreHit());
        agent.isStopped = true;
        animator.Play("WalkFwdStop");
        StartCoroutine(StopForSeconds(1f));
    }

    private void HandleLiftHit()
    {
        ignoreHit = true;
        StartCoroutine(ResetIgnoreHit());
        agent.isStopped = true;
        if (lm.weight >= 35)
        {
            isHeavy = true;
            rb.isKinematic = false;
            animator.Play("Lifting");
            StartCoroutine(PlayBackSlipAfterDelay(9.3f));
        }
        else
        {
            isHeavy = false;
            animator.Play("Throw");
            Destroy(lm.spawnedObject, 2.5f);
            StartCoroutine(ReWalk(3f));
        }
    }

    public void StartWithWalk()
    {
        if (gameObject.CompareTag(targetTag))
        {
            startAnimation = "WalkFwdLoop";
            animator.Play(startAnimation);
            agent.isStopped = false;
            agent.speed = 1f;
            agent.acceleration = 8;
            agent.stoppingDistance = 1;
            agent.autoBraking = false;
        }
    }

    public void StartWithRun()
    {
        if (gameObject.CompareTag(targetTag))
        {
            startAnimation = "RunFwdLoop";
            animator.Play(startAnimation);
            agent.isStopped = false;
            agent.speed = Random.Range(2f, 4f);
            agent.acceleration = 5;
            agent.stoppingDistance = 2;
            agent.autoBraking = true;
        }
    }

    void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    IEnumerator StopForSeconds(float seconds)
    {
        wm.CheckPlayerFall();
        yield return new WaitForSeconds(seconds);
        if (wm.isSlipped)
        {
            if (pushcount < 1)
            {
                StartCoroutine(PushBack(0.1f));
            }
        }
        else
        {
            agent.isStopped = false;
            animator.Play(startAnimation);
        }
    }

    IEnumerator PushBack(float seconds)
    {
        agent.enabled = false;
        animator.Play("Tripping");

        yield return new WaitForSeconds(seconds);
        StartCoroutine(FallForSeconds(1.5f));
        pushcount++;
    }

    IEnumerator FallForSeconds(float seconds)
    {
        rb.isKinematic = true;

        yield return new WaitForSeconds(seconds);
        animator.Play("FallingLoop_RootMotion");
        agent.enabled = true;

        yield return new WaitForSeconds(2f);
        ResetPlayerPosition();
    }

    public void ResetPlayerPosition()
    {
        // NavMeshAgent가 활성 상태인지 확인
        if (agent != null && agent.isActiveAndEnabled)
        {
            Debug.Log("Resetting player position to specific coordinates.");

            Vector3 resetPosition = waypoints[0].position;
            transform.position = resetPosition;
            agent.Warp(resetPosition);

            currentWaypointIndex = 0;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            Debug.Log("Setting destination to: " + waypoints[currentWaypointIndex].position);

            animator.Play(startAnimation);

            rb.isKinematic = true;
            pushcount = 0;
            agent.isStopped = false;
            ignoreHit = false;
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not active and enabled.");
        }
    }

    IEnumerator ResetIgnoreHit()
    {
        yield return new WaitForSeconds(ignoreDuration);
        ignoreHit = false;
    }

    private IEnumerator PlayBackSlipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("BackSlip");

        yield return new WaitForSeconds(2f);
        isHeavy = false;
        ResetPlayerPosition();
    }

    IEnumerator ReWalk(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        agent.isStopped = false;
        animator.Play(startAnimation);
    }

    IEnumerator ChangeColliderSizeAfterDelay(float delay)
    {
        // delay 초만큼 대기
        yield return new WaitForSeconds(delay);

        capsuleCollider.height /= 5f; // 높이 줄이기
        capsuleCollider.center = new Vector3(0, 0.2f, 0); // 센터 위치 조정

        yield return new WaitForSeconds(1f);
        capsuleCollider.height *= 5f; // 높이 원상복구
        capsuleCollider.center = new Vector3(0, 1f, 0); // 센터 위치 조정
    }
    IEnumerator RunRollingAnimAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(RollingAnim(0f));
    }
    IEnumerator RollingAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(startAnimation == "Laying1")
        {
            animator.SetBool("IsLeft", false);
        }
        else if(startAnimation == "Laying2")
        {
            animator.SetBool("IsRight", false);
        }
    }
}
