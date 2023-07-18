using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : TileMapController
{
    private const string OBSTACLE_TILEMAP_OBJ_NAME = "ObstacleTilemap";
    private GameObject[] volcanoObjs = default;
    // ��ã�� �˰����� �׽�Ʈ�� ������� �������� ĳ���� ������Ʈ �迭

    //! Awake Ÿ�ӿ� �ʱ�ȭ�� ������ ������
    public override void InitAwake(MapBoard mapController_)
    {
        this.tileMapObjName = OBSTACLE_TILEMAP_OBJ_NAME;
        base.InitAwake(mapController_);
    }       // InitAwawe()

    private void Start()
    {
        StartCoroutine(DelayStart(0f));
    }

    private IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoStart();
    }       // DelayStart()

    private void DoStart()
    {
        // { ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�
        volcanoObjs = new GameObject[2];
        TerrainController[] passableTerrains = new TerrainController[2];

        List<TerrainController> searchTerrains = default;
        int searchIdx = 0;
        TerrainController foundTile = default;

        // ������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�
        searchIdx = 0;
        foundTile = default;
        while (foundTile == null || foundTile == default)
        {
            // ������ �Ʒ��� ��ġ�Ѵ�
            searchTerrains = mapController.GetTerrains_Colum(searchIdx, true);
            foreach(var searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }
                else { /* Do Nothing */ }
            }

            if (foundTile != null || foundTile != default) { break; }
            if(mapController.MapCellSize.x - 1 <= searchIdx) { break; }
            searchIdx += 1;
        }       // loop: ������� ã�� ����

        passableTerrains[0] = foundTile;

        // �������� �������� �������� y���� ��ġ�ؼ� �� ������ �޾ƿ´�
        searchIdx = mapController.MapCellSize.x - 1;
        foundTile = default;
        while (foundTile != null || foundTile == default)
        {
            // �Ʒ����� ���� ��ġ�Ѵ�
            searchTerrains = mapController.GetTerrains_Colum(searchIdx);
            foreach(var searchTerrain in searchTerrains)
            {
                if (searchTerrain.IsPassable)
                {
                    foundTile = searchTerrain;
                    break;
                }
                else { /* Do Nothing */ }
            }

            if(foundTile != null || foundTile != default) { break; }
            if(searchIdx <= 0) { break; }
            searchIdx -= 1;
        }       // �������� ã�� ����

        passableTerrains[1] = foundTile;

        // } ������� �������� �����ؼ� Ÿ���� ��ġ�Ѵ�

        // TODO
        // { ������� �������� ������ �߰��Ѵ�
        // GameObject changeTilePrefab = null;
        // } ������� �������� ������ �߰��Ѵ�
    }       // DoStart()

    //! ������ �߰��Ѵ�
    public void Add_Obstacle(GameObject obstacle_)
    {
        allTileObjs.Add(obstacle_);
    }       // Add_Obstacle()
}
