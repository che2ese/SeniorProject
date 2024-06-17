using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    // 초기 transform 정보를 저장할 배열
    public Transform[] objectsToReset;
    public Transform[] objectsToReset1;

    // 초기 transform 정보를 저장할 변수
    private Vector3[] initialPositions;
    private Vector3[] initialPositions1;
    private Quaternion[] initialRotations;
    private Quaternion[] initialRotations1;

    private Vector3 initialPrefabPosition;
    private Vector3 initialPrefabPosition1;
    private Quaternion initialPrefabRotation;
    private Quaternion initialPrefabRotation1;

    void Start()
    {
        // 초기 transform 정보 저장
        SaveInitialTransforms();
        SaveInitialTransforms1();
    }

    void SaveInitialTransforms()
    {
        // 배열 크기만큼 초기 transform 정보 저장
        int objectCount = objectsToReset.Length;
        initialPositions = new Vector3[objectCount];
        initialRotations = new Quaternion[objectCount];

        for (int i = 0; i < objectCount; i++)
        {
            initialPositions[i] = objectsToReset[i].position;
            initialRotations[i] = objectsToReset[i].rotation;
        }
    }
    void SaveInitialTransforms1()
    {
        // 배열 크기만큼 초기 transform 정보 저장
        int objectCount1 = objectsToReset1.Length;
        initialPositions1 = new Vector3[objectCount1];
        initialRotations1 = new Quaternion[objectCount1];

        for (int i = 0; i < objectCount1; i++)
        {
            initialPositions1[i] = objectsToReset1[i].position;
            initialRotations1[i] = objectsToReset1[i].rotation;
        }
    }

    public void ResetObjectsToInitialTransforms()
    {
        // 저장된 초기 transform 정보를 사용하여 모든 오브젝트의 transform을 재설정
        int objectCount = objectsToReset.Length;
        for (int i = 0; i < objectCount; i++)
        {
            objectsToReset[i].position = initialPositions[i];
            objectsToReset[i].rotation = initialRotations[i];
        }
    }
    public void ResetObjectsToInitialTransforms1()
    {
        // 저장된 초기 transform 정보를 사용하여 모든 오브젝트의 transform을 재설정
        int objectCount1 = objectsToReset1.Length;
        for (int i = 0; i < objectCount1; i++)
        {
            objectsToReset1[i].position = initialPositions1[i];
            objectsToReset1[i].rotation = initialRotations1[i];
        }
    }
}
