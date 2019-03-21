using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static float p1Money { get; set; }
    public static float p2Money { get; set; }
    public static float p1BodyCount { get; set; }
    public static float p2BodyCount { get; set; }
    public static float p1SizeRatio { get; set; }
    public static float p2SizeRatio { get; set; }
    public static bool bgm { get; set; }
    public static bool scream { get; set; }
    public static bool collecting { get; set; }
    public static bool playerEaten { get; set; }
    
    public static void InitializeP1Size()
    {
        p1SizeRatio = 1.0f;
    }
    public static void InitializeP2Size()
    {
        p2SizeRatio = 1.0f;
    }
    public static void InitializeP1Money()
    {
        p1Money = 0f;
    }
    public static void InitializeP2Money()
    {
        p2Money = 0f;
    }
    public static void InitializeP1Body()
    {
        p1BodyCount = 0f;
    }
    public static void InitializeP2Body()
    {
        p2BodyCount = 0f;
    }
    public static void InitializeBgm()
    {
        bgm = true;
    }


}
