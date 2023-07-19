using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : GSingleton<PathFinder>
{
    #region ���� Ž���� ���� ����
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion      // ���� Ž���� ���� ����

    #region Astar �˰������� �ִܰŸ��� ã�� ���� ����
    private List<AstarNode> aStarResultPath = default;
    private List<AstarNode> aStarOpenPath = default;
    private List<AstarNode> aStarClosePath = default;
    #endregion      // Astar �˰������� �ִܰŸ��� ã�� ���� ����

    //! ������� ������ ������ ���� ã�� �Լ�
    public void FindPath_Astar()
    {
        StartCoroutine(DelayFindPath_Astar(1.0f));
    }       // FindPath_Astar()

    //! Ž�� �˰��� �����̸� �Ǵ�
    private IEnumerator DelayFindPath_Astar(float delay_)
    {
        // Astar �˰����� ����ϱ� ���ؼ� �н� ����Ʈ�� �ʱ�ȭ�Ѵ�
        aStarOpenPath = new List<AstarNode>();
        aStarClosePath = new List<AstarNode>();
        aStarResultPath = new List<AstarNode>();

        TerrainController targetTerrain = default;

        // ������� �ε����� ���ؼ�, ����� ��带 ã�ƿ´�
        string[] sourceObjNameParts = sourceObj.name.Split('_');
        int sourceIdx1D = -1;
        int.TryParse(sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIdx1D);
        targetTerrain = mapBoard.GetTerrain(sourceIdx1D);
        // ã�ƿ� ����� ��带 open list�� �߰�
        AstarNode targetNode = new AstarNode(targetTerrain, destinationObj);
        Add_AstarOpenList(targetNode);

        int loopIdx = 0;
        bool isFoundDestination = false;
        bool isNowayToGo = false;

        while (loopIdx < 10)
        {
            // { open list�� ��ȸ�ؼ� ���� �ڽ�Ʈ�� ���� ��带 �����Ѵ�
            AstarNode minCostNode = default;
            foreach (var terrainNode in aStarOpenPath)
            {
                if (minCostNode == default)
                {
                    minCostNode = terrainNode;
                }       // if: ���� ���� �ڽ�Ʈ�� ��尡 ��� �ִ� ���
                else
                {
                    // terrainNode �� �� ���� �ڽ�Ʈ�� ������ ���
                    // minCostNode �� ������Ʈ�Ѵ�
                    if (terrainNode.AstarF < minCostNode.AstarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else { continue; }
                }       // else: ���� ���� �ڽ�Ʈ�� ��尡 ĳ�̵Ǿ� �ִ� ���
            }       // loop: ���� �ڽ�Ʈ�� ���� ��带 ã�� ����
            // } open list�� ��ȸ�ؼ� ���� �ڽ�Ʈ�� ���� ��带 �����Ѵ�

            minCostNode.ShowCost_Astar();
            minCostNode.Terrain.SetTileActiveColor(RDefine.TileStatusColor.SEARCH);

            // ������ ��尡 �������� �����ߴ��� Ȯ���Ѵ�
            bool isArriveDest = mapBoard.GetDistance2D(minCostNode.Terrain.gameObject, destinationObj).Equals(Vector2Int.zero);

            if (isArriveDest)
            {
                // { �������� �����ߴٸ� aStarResultPath ����Ʈ�� �����Ѵ�
                AstarNode resultNode = minCostNode;
                bool isSet_aStarResultPath = false;
                while (isSet_aStarResultPath == false)
                {
                    aStarResultPath.Add(resultNode);
                    if (resultNode.AstarPrevNode == default || resultNode.AstarPrevNode == null)
                    {
                        isSet_aStarResultPath = true;
                        break;
                    }
                    else { /* Do Nothing */ }

                    resultNode = resultNode.AstarPrevNode;
                }       // loop: ���� ��带 ã�� ���� ������ ��ȸ�ϴ� ����
                // } �������� �����ߴٸ� aStarResultPath ����Ʈ�� �����Ѵ�

                // open list�� close list�� ����
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }       // if: ������ ��尡 �������� ������ ���
            else
            {
                // { �������� �ʾҴٸ� ���� Ÿ���� �������� 4������ ��带 ã�ƿ´�
                List<int> nextSearchIdx1Ds = mapBoard.GetTileIdx2D_Around4Ways(minCostNode.Terrain.TileIdx2D);

                // ã�ƿ� ��� �߿��� �̵� ������ ���� open list�� �߰��Ѵ�
                AstarNode nextNode = default;
                foreach (var nextIdx1D in nextSearchIdx1Ds)
                {
                    nextNode = new AstarNode(mapBoard.GetTerrain(nextIdx1D), destinationObj);

                    if (nextNode.Terrain.IsPassable == false) { continue; }

                    Add_AstarOpenList(nextNode, minCostNode);
                }       // loop: �̵� ������ ��带 open list�� �߰��ϴ� ����
                // } �������� �ʾҴٸ� ���� Ÿ���� �������� 4������ ��带 ã�ƿ´�

                // Ž���� ���� ���� close list�� �߰��ϰ�, open list���� �����Ѵ�
                // �� ��, open list�� ��� �ִٸ� �� �̻� Ž���� �� �ִ� ���� 
                // �������� �ʴ� ���̴�
                aStarClosePath.Add(minCostNode);
                aStarOpenPath.Remove(minCostNode);
                if (aStarOpenPath.IsValid() == false)
                {
                    GFunc.LogWarning("[Warning] There are no more tiles to explore");
                    isNowayToGo = true;
                }       // if: �������� �������� ���ߴµ�, �� �̻� Ž���� �� �ִ� ���� ���� ���

                foreach (var tempNode in aStarOpenPath)
                {
                    GFunc.Log($"Idx: {tempNode.Terrain.TileIdx1D}, " + $"cost: {tempNode.AstarF}");
                }

            }       // else: ������ ��尡 �������� �������� ���� ���

            loopIdx += 1;
            yield return new WaitForSeconds(delay_);
        }
        yield return new WaitForSeconds(delay_);
    }       // DelayFindPath_Astar()

    //! ����� ������ ��带 Open ����Ʈ�� �߰��ϴ� �Լ�
    private void Add_AstarOpenList(AstarNode targetTerrain_, AstarNode prevNode_ = default)
    {
        // Open ����Ʈ�� �߰��ϱ� ���� �˰��� ����� �����Ѵ�
        Update_AstarCostToTerrain(targetTerrain_, prevNode_);

        AstarNode closeNode = aStarClosePath.FindNode(targetTerrain_);
        if (closeNode != default && closeNode != null)
        {
            // �̹� Ž���� ���� ��ǥ�� ��尡 �����ϴ� ��쿡��
            // Open List�� �߰����� �ʴ´�
            /* Do Nothing */
        }       // if: close list �� �̹� Ž���� ���� ��ǥ�� ��尡 �����ϴ� ���
        else
        {
            AstarNode openedNode = aStarOpenPath.FindNode(targetTerrain_);
            if (openedNode != default && openedNode != null)
            {
                // Ÿ�� ����� �ڽ�Ʈ�� �� ���� ��쿡�� open list�� ���� ��ü�Ѵ�
                // Ÿ�� ����� �ڽ�Ʈ�� �� ū ��쿡�� open list�� �߰����� �ʴ´�
                if (targetTerrain_.AstarF < openedNode.AstarF)
                {
                    aStarOpenPath.Remove(openedNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                else { /* Do Nothing */ }
            }       // if: open list �� ���� �߰��� ���� ���� ��ǥ�� ��尡 �����ϴ� ���
            else
            {
                aStarOpenPath.Add(targetTerrain_);
            }       // else: open list�� ���� �߰��� ���� ���� ��ǥ�� ��尡 ���� ���
        }       // else: ���� Ž���� ������ ���� ����� ���
    }       // Add_AstarOpenList()

    //! Target ���� ������ Destination ���� ������ Distance �� Heuristic �� �����ϴ� �Լ�
    private void Update_AstarCostToTerrain(AstarNode targetNode, AstarNode prevNode)
    {
        // { Target �������� Destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����
        Vector2Int distance2D = mapBoard.GetDistance2D(targetNode.Terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        // Heuristic�� �����Ÿ��� ����
        Vector2 localDistance = destinationObj.transform.localPosition - targetNode.Terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);
        // } Target �������� Destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����

        // { ���� ��尡 �����ϴ� ���, ���� ����� �ڽ�Ʈ�� �߰��ؼ� ����
        if (prevNode == default || prevNode == null) { /* Do Nothing */ }
        else
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }
        targetNode.UpdateCost_Astar(totalDistance2D, heuristic, prevNode);
        // } ���� ��尡 �����ϴ� ���, ���� ����� �ڽ�Ʈ�� �߰��ؼ� ����
    }       // Update_AstarCostToTerrain()
}       // class PathFinder
