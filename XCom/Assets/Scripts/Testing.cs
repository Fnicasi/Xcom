using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit; 
    void Start()
    {


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(Mouse.GetPosition());
            GridPosition startGridPosition = new GridPosition(0,0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            for (int i = 0; i < gridPositionList.Count - 1; i++) // -1 because we check with the gridPositionList [i + 1]
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]) + Vector3.up,
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]) + Vector3.up,
                    Color.red,
                    20f
                );
            }
        }
    }
}
