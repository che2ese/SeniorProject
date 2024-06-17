using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float dayLengthInSeconds = 120f; // 하루의 길이 (초)
    public float elapsedTime = 0f; // 경과한 시간

    public Light light1; // 조명 1
    public Light light2; // 조명 2

    private bool light1Active = true; // 조명 1 활성화 여부
    private bool light2Active = true; // 조명 2 활성화 여부

    private void Update()
    {
        elapsedTime += Time.deltaTime; // 경과한 시간을 누적

        if (elapsedTime >= dayLengthInSeconds)
        {
            elapsedTime = 0f; // 다시 초기화
            // 조명 초기화
            light1.gameObject.SetActive(true);
            light2.gameObject.SetActive(true);
            light1Active = true;
            light2Active = true;
            Debug.Log("오전입니다.");
        }
        else if (elapsedTime >= dayLengthInSeconds / 3f && light1Active) // 1/3 경과 시
        {
            light1.gameObject.SetActive(false); // 조명 1 비활성화
            light1Active = false;
            Debug.Log("오후입니다.");
        }
        else if (elapsedTime >= (dayLengthInSeconds / 3f) * 2f && light2Active) // 2/3 경과 시
        {
            light2.gameObject.SetActive(false); // 조명 2 비활성화
            light2Active = false;
            Debug.Log("저녁입니다.");
        }
    }
}
