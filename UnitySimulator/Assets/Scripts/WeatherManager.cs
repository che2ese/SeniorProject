using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour
{
    public enum WeatherState { None, Rainy, Windy }
    public WeatherState currentWeather;
    public WeatherState previousWeather;
    public bool isSlipped;
    public float fallProbability;
    public List<Player> players = new List<Player>();
    public List<GameObject> workers = new List<GameObject>();

    // 비와 바람 프리팹
    public GameObject rainPrefab;
    public GameObject windPrefab;
    public GameObject nonePrefab;
    public GameObject worker;

    private Dictionary<WeatherState, float> fallProbabilities = new Dictionary<WeatherState, float>();

    private void Start()
    {
        // Player 스크립트 참조 가져오기
        Player[] foundPlayers = GameObject.FindObjectsOfType<Player>();
        foreach (Player p in foundPlayers)
        {
            players.Add(p);
        }

        // 초기에는 날씨 효과 없음
        SetWeather(WeatherState.None);

        InitializeFallProbabilities();
    }

    public void ChangeWeatherToNone()
    {
        SetWeather(WeatherState.None);
    }

    public void ChangeWeatherToRainy()
    {
        SetWeather(WeatherState.Rainy);
    }

    public void ChangeWeatherToWindy()
    {
        SetWeather(WeatherState.Windy);
    }

    private void SetWeather(WeatherState weatherState)
    {
        // 비와 바람 효과 비활성화
        rainPrefab.SetActive(false);
        windPrefab.SetActive(false);
        nonePrefab.SetActive(false);

        previousWeather = currentWeather;
        currentWeather = weatherState;

        // 날씨 상태에 따라 적절한 효과 활성화
        switch (currentWeather)
        {
            case WeatherState.None:
                foreach (var worker in workers)
                {
                    worker.SetActive(true);
                }
                nonePrefab.SetActive(true);
                foreach (Player player in players)
                {
                    player.agent.speed = 1.5f;
                    player.agent.acceleration = 5;
                    player.agent.stoppingDistance = 2;
                    player.agent.autoBraking = true;
                }
                break;
            case WeatherState.Rainy:
                foreach (var worker in workers)
                {
                    worker.SetActive(false);
                }
                rainPrefab.SetActive(true);
                break;
            case WeatherState.Windy:
                foreach (var worker in workers)
                {
                    worker.SetActive(true);
                }
                windPrefab.SetActive(true);
                foreach (Player player in players)
                {
                    player.agent.speed = 1f;
                    player.agent.acceleration = 8;
                    player.agent.stoppingDistance = 1;
                    player.agent.autoBraking = false;
                }
                break;
            default:
                break;
        }

        Debug.Log("현재 날씨: " + currentWeather); // 디버깅용 로그 추가

        // 이전 날씨가 Rainy이었을 때, 비가 그친 후에 모든 기능을 1.5배로 증가
        if (previousWeather == WeatherState.Rainy && currentWeather != WeatherState.Rainy)
        {
            IncreaseFunctionality();
            GameManager.Instance.Restart();
        }
    }

    private void IncreaseFunctionality()
    {
        // 이전 날씨가 Rainy였을 때 비가 그친 후 모든 기능을 1.5배로 증가시킴
        foreach (Player player in players)
        {
            player.agent.acceleration *= 1.5f;
            player.agent.stoppingDistance /= 2;
            player.agent.autoBraking = false;
        }
        fallProbabilities[WeatherState.None] *= 1.5f;
        fallProbabilities[WeatherState.Windy] *= 1.5f;
    }

    private void InitializeFallProbabilities()
    {
        // 각 날씨별 확률을 딕셔너리에 저장
        fallProbabilities.Add(WeatherState.None, Random.Range(10f, 60f));
        fallProbabilities.Add(WeatherState.Windy, Random.Range(30f, 80f));
    }

    public void CheckPlayerFall()
    {
        fallProbability = fallProbabilities[currentWeather];

        foreach (Player player in players)
        {
            if (player.startAnimation == "RunFwdLoop")
            {
                fallProbability *= 1.2f;
                break; 
            }
        }

        isSlipped = fallProbability > 50;
    }
}
