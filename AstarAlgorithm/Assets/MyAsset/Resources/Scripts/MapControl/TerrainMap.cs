using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainMap : TerrainController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTilemap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;

    private List<TerrainController> allTerrains = default;

    //! Awake 타임에 초기화 할 내용을 재정의한다
    public override void InitAwake(MapBoard mapController_)
    {
        tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);

        allTerrains = new List<TerrainController>();

        // { 타일의 x축 갯수와 전체 타일의 수로 맵의 가로, 세로 사이즈를 연산한다
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;
        for (int i = 0; i < allTerrains.Count; i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y) == false)
            {
                mapCellSize.x = i;
                break;
            }       // if: 첫번째 타일의 y 좌표와 달라지는 지점 전까지가 맵의 가로 셀 크기이다
        }

        // 전체 타일의 수를 맵의 가로 셀 크기로 나눈 값이 맵의 세로 셀 크기이다
        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);

        // } 타일의 x축 갯수와 전체 타일의 수로 맵의 가로, 세로 사이즈를 연산한다

        // { x축 상의 두 타일과, y축 상의 두 타일 사이의 로컬 포지션으로 타일 갭을 계산한다
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;
        // } x축 상의 두 타일과, y축 상의 두 타일 사이의 로컬 포지션으로 타일 갭을 계산한다
    }       // InitAwake()
    private void Start()
    {
        // { 타일맵의 일부를 일정 확률로 다른 타일로 교체하는 로직
        // GameObject chageTilePrefab
        // } 타일맵의 일부를 일정 확률로 다른 타일로 교체하는 로직
    }

    //! 초기화된 타일의 정보로 연산한 맵의 가로, 세로 크기를 리턴하는 함수
    public Vector2Int GetCellSize()
    {
        return mapCellSize;
    }

    //! 초기화된 타일의 정보로 연산된 타일 사이의 갭을 리턴한다
    public Vector2 GetCellGap()
    {
        return mapCellGap;
    }

    //! 인덱스에 해당하는 타일을 리턴하는 함수
    public TerrainController GetTile(int tileIdx1D)
    {
        if(allTerrains.IsValid(tileIdx1D))
        {
            return allTerrains[tileIdx1D];
        }

        return default;
    }       // GetTile()

}
