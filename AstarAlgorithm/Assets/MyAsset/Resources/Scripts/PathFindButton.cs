using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindButton : MonoBehaviour
{
    //! Astar find 버튼을 누른 경우
    public void OnClickAstarFindButton()
    {
        PathFinder.Instance.FindPath_Astar();
    }       // OnClickAstarFindButton()
}
