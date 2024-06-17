using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject item1; // 첫 번째 아이템
    public GameObject item2; // 두 번째 아이템
    public GameObject item3; // 세 번째 아이템
    public GameObject item4;

    private float rotationSpeed1 = -1f; // 첫 번째 아이템의 회전 속도
    private float rotationSpeed2 = 2f;  // 두 번째 아이템의 회전 속도
    private float rotationSpeed3 = 1f;  // 세 번째 아이템의 회전 속도

    void Update()
    {
        // 아이템을 X 축으로 회전시키기
        RotateItem(item1, rotationSpeed1);
        RotateItem(item2, rotationSpeed2);
        RotateItem(item3, rotationSpeed3);
    }

    // 아이템을 주어진 속도로 회전시키는 함수
    void RotateItem(GameObject item, float rotationSpeed)
    {
        if (item != null)
        {
            item.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }
}
