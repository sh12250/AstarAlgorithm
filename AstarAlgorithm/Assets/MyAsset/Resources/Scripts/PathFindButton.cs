using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindButton : MonoBehaviour
{
    //! Astar find ��ư�� ���� ���
    public void OnClickAstarFindButton()
    {
        PathFinder.Instance.FindPath_Astar();
    }       // OnClickAstarFindButton()
}
