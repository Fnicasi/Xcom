using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    //To calculate distance, vertical movement is 10, and diagonal is 14 
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    private int width;
    private int height;
    private float cellsize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null) //Just in case another Pathfinding was created incorrectly 
        {
            Debug.Log("There's more than one Pathfinding" + transform + " - " + Instance);
            Destroy(Instance);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }


    public List<GridPosition> FindPath(GridPosition startGridPos, GridPosition endGridPos)
    {
        List<PathNode> openList= new List<PathNode>();
        List<PathNode> closedList= new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPos); //Get the object (node) in the start grid position
        PathNode endNode = gridSystem.GetGridObject(endGridPos); //Get the object (node) in the end (target) grid position

        openList.Add(startNode); 
        //Reset all the nodes 
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z <gridSystem.GetHeight(); z++) 
            {
                GridPosition gridPosition = new GridPosition(x,z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition); 

                pathNode.SetGCost(int.MaxValue);   //Set it as the max value by default so the path is not taken until updated correctly
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPos, endGridPos)); //H is the distance between the initial position of the unit in the grid and the target
        startNode.CalculateFCost();
        
        //Now that the startNode has the cost initialized and the grid is reset, we can start the cycle
        while (openList.Count > 0) //While there's still nodes to search
        {
            PathNode currentNode = GetLowestFCostPathnode(openList); //Get the node from the open list with the lowest F cost

            if(currentNode == endNode)
            {
                //Reached the final node
                return CalculatePath(endNode); //return a list containing the path from start to end
            }

            //Since we iterated on the currentNode, remove it from the open list and add it to the close list
            openList.Remove(currentNode); 
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) //For all neighbours to the currentNode...
            {
                if (closedList.Contains(neighbourNode)) //if the neighbour is on the closed list, don't analyze it
                {
                    continue;
                }
                //G Cost is going to be the acumulated G cost + the distance (g) between the current node and the neighbour we are checking
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                
                if(tentativeGCost < neighbourNode.GetGCost())//If so, we found a better path to go into the neighbour and have to update it
                {
                    neighbourNode.SetCameFromPathNode(currentNode); //Set the parent 
                    neighbourNode.SetGCost(tentativeGCost); 
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPos));
                    neighbourNode.CalculateFCost(); 

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

        }

        //No path found
        return null;

    }
    //Used to calculate the distance between 2 grid positions
    public int CalculateDistance(GridPosition gridpositionA, GridPosition gridpositionB)
    {//Diagonal movement is going to be the diference between x and z, e.g. 3x and 1z would be 1 diagonal movement and 2 horizontal ones.
        GridPosition gridPositionDistance = gridpositionA - gridpositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance- zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathnode (List<PathNode> pathNodeList) 
    {
        PathNode lowestFCostPathnode = pathNodeList[0];//Get the first node
        for (int i = 0; i < pathNodeList.Count; i++) 
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathnode.GetFCost()) //if the iterated node has a lower F cost, then...
            {
                lowestFCostPathnode = pathNodeList[i]; //It's the current lowest
            }
        }
        return lowestFCostPathnode;
    }
    //We pass "x" and "z" and receive the GridObject, the Node in this case, and return it
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));   
    }

    //Return all the neighbours to the current one
    private List<PathNode> GetNeighbourList (PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition(); //the grid position of the node we are searching neighbours for

        if (gridPosition.x - 1 >= 0) //To check that we are inbounds
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0)); //Left
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); //Left Down (Diagonal)
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); //Left Up (Diagonal)
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth()) //To check that we are inbounds
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0)); //Right
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); //Right Down
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {

                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); //Right Up
            }
        }
        if (gridPosition.z - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1)); //Down
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1)); //Up
        }

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null) //while not null, there's still a connection
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode()); //Add the parent to the list of the path
            currentNode = currentNode.GetCameFromPathNode(); //Go check the parent now
        }

        pathNodeList.Reverse(); //Reverse it so it's from begining to end and no end to begining

        List<GridPosition> gridPositionList = new List<GridPosition>(); //create a list of grid positions
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
}
