using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private WeatherManager wm;
    public GameObject warning;
    private float timer = 0f; // 경과 시간을 추적하는 변수
    private RawImage warningRawImage;

    public Scrollbar scrollbar; // UI Scrollbar
    public Text scaleText;
    private GameObject[] workers; // Worker 오브젝트 배열
    public GameObject worker3;

    void Awake()
    {
        warningRawImage = warning.GetComponent<RawImage>();
        wm = GameObject.FindObjectOfType<WeatherManager>();

        if (wm == null)
        {
            Debug.LogError("WeatherManager not found in the scene!");
        }
    }

    void Start()
    {
        // Worker 태그를 가진 모든 오브젝트를 찾습니다.
        workers = GameObject.FindGameObjectsWithTag("Worker");
        if (workers == null || workers.Length == 0)
        {
            Debug.LogWarning("No Workers found with tag 'Worker'!");
        }

        if (scrollbar != null)
        {
            // 스크롤바 값 변경 시 호출될 콜백 함수 설정
            scrollbar.onValueChanged.AddListener(OnScrollBarValueChanged);
            // 초기 값 설정
            scrollbar.value = 0.5f; // 중간 값 (0.5에 해당하는 y축 스케일은 0.9)
            OnScrollBarValueChanged(scrollbar.value);
        }
        else
        {
            Debug.LogError("Scrollbar is not assigned!");
        }
    }

    void Update()
    {
        if (wm != null && wm.currentWeather == WeatherManager.WeatherState.Rainy)
        {
            Blink();
        }
        else if (warning != null)
        {
            warning.SetActive(false);
        }
    }

    void Blink()
    {
        if (warning != null)
        {
            warning.SetActive(true);

            // 경과 시간을 누적
            timer += Time.deltaTime;

            // 투명도 값을 계산 (0.2에서 1까지 왕복)
            float alpha = Mathf.PingPong(timer, 1.0f) * 0.8f + 0.2f;

            if (warningRawImage != null)
            {
                // warning 오브젝트의 RawImage 컴포넌트의 투명도를 설정
                Color color = warningRawImage.color;
                color.a = alpha;
                warningRawImage.color = color;
            }

            // 일정 간격마다 투명도를 변경합니다.
            if (timer >= 1.0f)
            {
                timer = 0f; // 타이머를 재설정
            }
        }
    }

    public void OnScrollBarValueChanged(float value)
    {
        // y축 스케일 범위 0.8에서 1.0 사이로 맵핑
        float newYScale = Mathf.Lerp(0.8f, 1.0f, value);

        if (workers != null)
        {
            // 모든 Worker 오브젝트의 y축 스케일을 업데이트
            foreach (GameObject worker in workers)
            {
                if (worker != null)
                {
                    Vector3 newScale = worker.transform.localScale;
                    newScale.y = newYScale;
                    worker.transform.localScale = newScale;
                }
            }
        }
        // 스케일 크기에 180을 곱한 값을 텍스트로 표시
        float scaleValue = (newYScale - 0.9f) * 100 + 170;

        Debug.Log("Scale Value: " + scaleValue); // Debug 로그 추가

        if (scaleText != null)
        {
            scaleText.fontSize = 40;
            scaleText.color = Color.white;
            scaleText.text = "Height : " + scaleValue.ToString("F2") + " cm";
        }
        else
        {
            Debug.LogError("scaleText is not assigned!");
        }
    }
    public void IncreasePeople()
    {
        worker3.SetActive(true);
    }
    public void DecreasePeople()
    {
        worker3.SetActive(false);
    }
}
