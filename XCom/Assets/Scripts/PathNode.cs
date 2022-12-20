using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{ //This is going to be our GridObject representing a single pathfinding node
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost; //h+g
    private PathNode cameFromPathNode; //Reference to know where a node has come from (parent)
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }
    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }

}
