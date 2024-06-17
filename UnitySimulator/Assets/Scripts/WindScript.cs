using UnityEngine;

public class WindEffect : MonoBehaviour
{
    // 바람 효과를 나타내는 오브젝트의 사운드
    public AudioClip windSound;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Start()
    {
        // 바람 소리 재생
        if (windSound != null && audioSource != null)
        {
            audioSource.clip = windSound;
            audioSource.loop = true; // 반복 재생 설정
            audioSource.Play();
        }

        // 배경 어둡게 만들기
        DarkenBackground();
    }

    private void DarkenBackground()
    {
        // 배경 어둡게 만들기 (예시로 화면을 푸르게 만들었습니다)
        Camera.main.backgroundColor = Color.blue * 0.3f; // 어둡게 조절
    }

    private void OnDestroy()
    {
        // 소리 정지
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Camera.main.backgroundColor = Color.black;
    }
}
