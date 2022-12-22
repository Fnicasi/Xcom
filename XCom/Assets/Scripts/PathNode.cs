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

    public void SetGCost(int gCost) 
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    { 
        this.hCost = hCost;
    }
    public void CalculateFCost()
    {
        fCost = hCost + gCost;
    }
    public void ResetCameFromPathNode() //remove parent node
    {
        cameFromPathNode = null;
    }
    public void SetCameFromPathNode(PathNode pathNode) //set the parent node
    {
        cameFromPathNode = pathNode;
    }public PathNode GetCameFromPathNode() //get the parent node
    {
        return cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    
}
