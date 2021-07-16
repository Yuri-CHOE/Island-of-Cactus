using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job
{
    public enum JobType
    {
        None,
        System,
        Druid,
        Hunter,
        Blacksmith,
        Alchemist,
        Chef,
    }

    public static int GetTypeCount()
    {
        return System.Enum.GetValues(typeof(JobType)).Length;
    }
}
