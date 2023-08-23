using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelper
{
    public static string ExtractName(GameObject enemyObject)
    {
        string[] names = enemyObject.name.Split("-");
        return names.Length>0? names[0] : null;
    }
}
