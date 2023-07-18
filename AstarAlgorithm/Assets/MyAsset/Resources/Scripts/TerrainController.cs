using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : MonoBehaviour
{
    private const string TILE_FRONT_RENDERER_OBJ_NAME = "FrontRenderer";

    private TerrainType terrainType = TerrainType.NONE;
    private MapBoard mapController = default;

    public bool IsPossible { get; private set; } = false;
    public int TileIdx1D { get; private set; } = -1;
    public Vector2Int TileIdx2D { get; private set; } = default;

    #region 길찾기 알고리즘을 위한 변수
    private SpriteRenderer frontRenderer = default;
    private Color defaultColor = default;
    private Color selectedColor = default;
    private Color searchColor = default;
    private Color inactiveColor = default;
    #endregion // 길찾기 알고리즘을 위한 변수

    private void Awake()
    {
        frontRenderer = gameObject.FindChildComponent<SpriteRenderer>(TILE_FRONT_RENDERER_OBJ_NAME);

        defaultColor = new Color(1f, 1f, 1f, 1f);
        selectedColor = new Color(236f / 255f, 130f / 255f, 20f / 255f, 1f);
        searchColor = new Color(0f, 192f / 255f, 0f, 1f);
        inactiveColor = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
    }       // Awake()

    //! 지형정보를 초기 설정한다
    public void SetupTerrain(MapBoard mapController_, TerrainType type_, int tileIdx1D_)
    {
        terrainType = type_;
        mapController = mapController_;
        TileIdx1D = tileIdx1D_;
        // TileIdx2D = mapController.GetTile
    }
}
