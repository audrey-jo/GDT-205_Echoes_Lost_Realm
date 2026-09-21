using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------------------------------------------------------------

public enum AlgorithmChoice
{
    AStar,
    Dijkstra,
    DFS,
    BFS
};

//-------------------------------------------------------------------------------------------------

public class AlgorithmOptions : MonoBehaviour
{
    public static AlgorithmChoice algorithmChoice = AlgorithmChoice.AStar;

    [SerializeField] private Image AStar_ButtonImage;
    [SerializeField] private Image Dijkstra_ButtonImage;
    [SerializeField] private Image DFS_ButtonImage;
    [SerializeField] private Image BFS_ButtonImage;

    [SerializeField] private AStarAlgorithm aStar;
    [SerializeField] private DijkstraAlgorithm dijkstra;
    [SerializeField] private DFSAlgorithm dfs;
    [SerializeField] private BFSAlgorithm bfs;

    private BaseAlgorithm currentAlgorithm;

    //-------------------------------------------------------------------------------------------------

    void OnEnable()
    {
        Choose_Astar();
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_Astar()
    {
        algorithmChoice  = AlgorithmChoice.AStar;
        currentAlgorithm = aStar;
        if (currentAlgorithm != null)
            currentAlgorithm.ClearVisuals();

        if (AStar_ButtonImage != null)
        {
            AStar_ButtonImage.color = Color.blue;
        }

        if (Dijkstra_ButtonImage != null)
        {
            Dijkstra_ButtonImage.color = Color.black;
        }

        if (DFS_ButtonImage != null)
        {
            DFS_ButtonImage.color = Color.black;
        }

        if (BFS_ButtonImage != null)
        {
            BFS_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_Dijkstra()
    {
        algorithmChoice  = AlgorithmChoice.Dijkstra;
        currentAlgorithm = dijkstra;
        if (currentAlgorithm != null)
            currentAlgorithm.ClearVisuals();

        if (AStar_ButtonImage != null)
        {
            AStar_ButtonImage.color = Color.black;
        }

        if (Dijkstra_ButtonImage != null)
        {
            Dijkstra_ButtonImage.color = Color.blue;
        }

        if (DFS_ButtonImage != null)
        {
            DFS_ButtonImage.color = Color.black;
        }

        if (BFS_ButtonImage != null)
        {
            BFS_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_BFS()
    {
        algorithmChoice  = AlgorithmChoice.BFS;
        currentAlgorithm = bfs;
        if(currentAlgorithm != null)
            currentAlgorithm.ClearVisuals();

        if (AStar_ButtonImage != null)
        {
            AStar_ButtonImage.color = Color.black;
        }

        if (Dijkstra_ButtonImage != null)
        {
            Dijkstra_ButtonImage.color = Color.black;
        }

        if (DFS_ButtonImage != null)
        {
            DFS_ButtonImage.color = Color.black;
        }

        if (BFS_ButtonImage != null)
        {
            BFS_ButtonImage.color = Color.blue;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Choose_DFS()
    {
        algorithmChoice  = AlgorithmChoice.DFS;
        currentAlgorithm = dfs;
        if (currentAlgorithm != null)
            currentAlgorithm.ClearVisuals();

        if (AStar_ButtonImage != null)
        {
            AStar_ButtonImage.color = Color.black;
        }

        if (Dijkstra_ButtonImage != null)
        {
            Dijkstra_ButtonImage.color = Color.black;
        }

        if (DFS_ButtonImage != null)
        {
            DFS_ButtonImage.color = Color.blue;
        }

        if (BFS_ButtonImage != null)
        {
            BFS_ButtonImage.color = Color.black;
        }
    }

    //-------------------------------------------------------------------------------------------------

    public void Search()
    {
        currentAlgorithm.Search();
    }
    
    //-------------------------------------------------------------------------------------------------

    public void SteppedSearch()
    {
        currentAlgorithm.SteppedSearch();
    }

    //-------------------------------------------------------------------------------------------------
}
