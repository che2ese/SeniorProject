using UnityEngine;

public class LiftManager : MonoBehaviour
{
    public GameObject[] prefabList; // 프리팹 리스트
    public Transform spawnPoint; // 소환 지점
    private float spawnInterval = 30f; // 다시 소환되는 시간 간격

    private float timer = 0f; // 경과 시간을 나타내는 변수
    public float weight;

    // 생성된 프리팹에 대한 참조를 저장하기 위한 변수
    public GameObject spawnedObject { get; private set; }

    private void Start()
    {
        // 최초 시작 시 프리팹 소환
        SpawnRandomPrefab();
    }

    private void Update()
    {
        // 일정 시간 간격마다 새로운 물체 생성
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }
            SpawnRandomPrefab();
        }
    }


    private void SpawnRandomPrefab()
    {
        // 랜덤한 프리팹을 선택하여 소환
        int randomIndex = Random.Range(0, prefabList.Length);
        GameObject prefabToSpawn = prefabList[randomIndex];

        // 프리팹의 크기를 랜덤하게 설정합니다. 각 축별로 0.5에서 0.8 사이의 값을 랜덤하게 선택합니다.
        Vector3 scale = new Vector3(
            Random.Range(0.7f, 0.8f),
            Random.Range(0.7f, 0.8f),
            Random.Range(0.7f, 0.8f)
        );
        float combined = (scale.x + scale.y + scale.z) / 3;

        // 프리팹을 생성하고 크기를 적용합니다.
        spawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.Euler(0f, 90f, 0f));
        spawnedObject.transform.localScale = scale;

        // 생성된 프리팹에 Rigidbody 컴포넌트를 추가하고 중력 옵션을 활성화
        Rigidbody rigidbody = spawnedObject.GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = spawnedObject.AddComponent<Rigidbody>();
        }
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = true;
        spawnedObject.tag = "Lift";

        if (randomIndex == 0)
        {
            spawnedObject.transform.position += new Vector3(-0.3f, 0f, -0.45f);
            weight = 70 * combined;

            BoxCollider collider = spawnedObject.GetComponent<BoxCollider>();
            if (collider != null)
            {
                Vector3 newSize = collider.size;
                newSize.y *= 1.5f;
                collider.size = newSize;
                Vector3 newCenter = collider.center;
                newCenter.y = 1f;
                collider.center = newCenter;
            }
        }
        else if(randomIndex == 1)
        {
            spawnedObject.transform.position += new Vector3(0f, 0f, 0.25f);
            weight = 50 * combined;
            
            CapsuleCollider collider = spawnedObject.GetComponent<CapsuleCollider>();
            if (collider != null)
            {
                collider.height *= 1.5f;
                Vector3 newCenter = collider.center;
                newCenter.y = 1f;
                collider.center = newCenter;
            }
        }
        else if (randomIndex == 2)
        {
            spawnedObject.transform.position += new Vector3(0.1f, 0f, -0.25f);
            weight = 30 * combined;

            BoxCollider collider = spawnedObject.GetComponent<BoxCollider>();
            if (collider != null)
            {
                Vector3 newSize = collider.size;
                newSize.y *= 2f;
                collider.size = newSize;
                Vector3 newCenter = collider.center;
                newCenter.y = 1f;
                collider.center = newCenter;
            }
        }
    }
}
