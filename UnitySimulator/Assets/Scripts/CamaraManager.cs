using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public RawImage[] rawImages; // 4개의 RawImage 배열
    public GameObject[] uiElements; // UI 요소 배열

    private RectTransform[] originalRects; // 각 RawImage의 원래 RectTransform
    private Vector2[] originalAnchorMin;
    private Vector2[] originalAnchorMax;
    private Vector2[] originalOffsetMin;
    private Vector2[] originalOffsetMax;
    private bool isZoomed = false; // 확대 여부
    private int zoomedIndex = -1; // 확대된 이미지 인덱스

    void Start()
    {
        // 각 RawImage의 원래 RectTransform 값을 저장합니다.
        originalRects = new RectTransform[rawImages.Length];
        originalAnchorMin = new Vector2[rawImages.Length];
        originalAnchorMax = new Vector2[rawImages.Length];
        originalOffsetMin = new Vector2[rawImages.Length];
        originalOffsetMax = new Vector2[rawImages.Length];
        for (int i = 0; i < rawImages.Length; i++)
        {
            originalRects[i] = rawImages[i].GetComponent<RectTransform>();
            originalAnchorMin[i] = originalRects[i].anchorMin;
            originalAnchorMax[i] = originalRects[i].anchorMax;
            originalOffsetMin[i] = originalRects[i].offsetMin;
            originalOffsetMax[i] = originalRects[i].offsetMax;
        }

        // 각 RawImage에 클릭 이벤트를 추가합니다.
        foreach (RawImage rawImage in rawImages)
        {
            AddClickEvent(rawImage);
        }
    }

    void AddClickEvent(RawImage rawImage)
    {
        EventTrigger trigger = rawImage.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { OnImageClick(rawImage); });
        trigger.triggers.Add(entry);
    }

    void OnImageClick(RawImage clickedImage)
    {
        if (isZoomed)
        {
            // 원래 RectTransform으로 돌아갑니다.
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].GetComponent<RectTransform>().anchorMin = originalAnchorMin[i];
                rawImages[i].GetComponent<RectTransform>().anchorMax = originalAnchorMax[i];
                rawImages[i].GetComponent<RectTransform>().offsetMin = originalOffsetMin[i];
                rawImages[i].GetComponent<RectTransform>().offsetMax = originalOffsetMax[i];
            }

            // UI 요소들을 다시 활성화합니다.
            foreach (GameObject uiElement in uiElements)
            {
                uiElement.SetActive(true);
            }

            isZoomed = false;
            zoomedIndex = -1;
        }
        else
        {
            // 모든 RawImage를 숨기고 클릭한 RawImage만 확대합니다.
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                rawImages[i].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                rawImages[i].GetComponent<RectTransform>().offsetMin = Vector2.zero;
                rawImages[i].GetComponent<RectTransform>().offsetMax = Vector2.zero;
            }

            clickedImage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            clickedImage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            clickedImage.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            clickedImage.GetComponent<RectTransform>().offsetMax = Vector2.zero;

            // UI 요소들을 비활성화합니다.
            foreach (GameObject uiElement in uiElements)
            {
                uiElement.SetActive(false);
            }

            isZoomed = true;
            zoomedIndex = System.Array.IndexOf(rawImages, clickedImage);
        }
    }
}
