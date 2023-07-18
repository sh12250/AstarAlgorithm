using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RDefine
{
    public const string TERRAIN_PREF_OCEAN = "OceanTile";
    public const string TERRAIN_PREF_PLAIN = "PlainTile";

    public const string OBSTACLE_PREF_PLAIN_CASTLE = "Obstacle_PlainCastle";

    public enum TileStatusColor
    {
        DEFAULT, SELECTED, SEARCH, INACTIVE
    }
}
