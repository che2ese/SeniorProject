using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogToFile : MonoBehaviour
{
    private Player player;
    private WeatherManager wm;
    private LiftManager lm;
    private TimeManager tm;

    private StreamWriter writer;
    private float humidity;
    private bool weatherAlreadyLogged = false;
    private bool weightAlreadyLogged = false;
    void Awake()
    {
        wm = GameObject.FindObjectOfType<WeatherManager>();
        lm = GameObject.FindObjectOfType<LiftManager>();
        tm = GameObject.FindObjectOfType<TimeManager>();
        player = GameObject.FindObjectOfType<Player>();
    }
    void Start()
    {
        string filePath = Application.dataPath + "/log.txt";
        writer = new StreamWriter(filePath, true);
        writer.WriteLine("Logging started at: " + System.DateTime.Now);
        writer.Flush();
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }

    void OnDestroy()
    {
        writer.Close();
    }

    void Update()
    {
        if (wm.isSlipped == false)
        {
            weatherAlreadyLogged = false; // 다시 로깅 가능하도록 초기화
        }

        else if (!weatherAlreadyLogged)
        {
            if (wm.isSlipped == true)
            {
                if (wm.previousWeather == WeatherManager.WeatherState.Rainy && wm.currentWeather != WeatherManager.WeatherState.Rainy)
                {
                    humidity = Mathf.Lerp(65f, 70f, wm.fallProbability);

                    if (wm.currentWeather == WeatherManager.WeatherState.Windy)
                    {
                        humidity *= 1.2f;
                    }
                }
                else
                {
                    humidity = Mathf.Lerp(55f, 60f, wm.fallProbability);
                    if (wm.currentWeather == WeatherManager.WeatherState.Windy)
                    {
                        humidity *= 1.2f;
                    }
                }
                writer.WriteLine(
                tm.elapsedTime >= tm.dayLengthInSeconds ? "오전입니다." :
                tm.elapsedTime >= tm.dayLengthInSeconds / 3f ? "오후입니다." :
                tm.elapsedTime >= (tm.dayLengthInSeconds / 3f) * 2f ? "저녁입니다." : ""
                );
                writer.WriteLine("사고 발생 원인 : 미끄러짐");
                writer.WriteLine("날씨 : " + wm.currentWeather);
                writer.WriteLine("최근 비의 유무 : " + (wm.previousWeather == WeatherManager.WeatherState.Rainy ? "O" : "X"));
                writer.WriteLine("습도 : " + humidity + "%");
                writer.WriteLine("바람의 유무 : " + (wm.previousWeather == WeatherManager.WeatherState.Windy ? "O" : "X"));
                writer.WriteLine();

                weatherAlreadyLogged = true;
            }
        }
        if (player.isHeavy == false)
        {
            weightAlreadyLogged = false;
        }

        else if (!weightAlreadyLogged)
        {
            if (player.isHeavy == true)
            {
                writer.WriteLine(
                tm.elapsedTime >= tm.dayLengthInSeconds ? "오전입니다." :
                tm.elapsedTime >= tm.dayLengthInSeconds / 3f ? "오후입니다." :
                tm.elapsedTime >= (tm.dayLengthInSeconds / 3f) * 2f ? "저녁입니다." : ""
                );
                writer.WriteLine("사고 발생 원인 : 무거운 물체를 들다가");
                writer.WriteLine("무게 : " + (int)lm.weight + "kg");
                writer.WriteLine();

                weightAlreadyLogged = true;
            }
        }
    }
}

