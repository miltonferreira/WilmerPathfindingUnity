using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    //*** Inicia gerenciamento do grid de nodes na tela <-----------------------
    public MapData mapData; // gerador da grid
    public Graph graph;

    #region vars pathfinder
    public Pathfinder pathfinder;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 15;
    public int goalY = 1;

    public float timeStep = 0.1f;
    #endregion

    void Start() {
        if(mapData != null && graph != null){
            int[,] mapInstance = mapData.MakeMap(); // cria a grid/labirinto/maze
            graph.Init(mapInstance);                // criação do grid

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if(graphView != null){
                graphView.Init(graph);
            }

            #region pathfinder system
            // indica ponto inicial e ponto final para o PATHFINDER
            if(graph.IsWithinBounds(startX,startY) && graph.IsWithinBounds(goalX, goalY)
            && pathfinder != null){
                Node startNode = graph.nodes[startX,startY];
                Node goalNode = graph.nodes[goalX,goalY];

                // pega os grids ponto inicial e final
                pathfinder.Init(graph, graphView, startNode, goalNode);
                
                // faz pesquisa para achar ponto final
                StartCoroutine(pathfinder.SearchRoutine(timeStep));
            }
            #endregion
        }
    }
}
