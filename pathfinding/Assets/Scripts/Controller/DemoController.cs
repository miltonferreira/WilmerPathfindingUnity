using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    //*** Inicia gerenciamento do grid de nodes na tela <-----------------------
    public MapData mapData; // gerador da grid
    public Graph graph;

    void Start() {
        if(mapData != null && graph != null){
            int[,] mapInstance = mapData.MakeMap(); // cria a grid/labirinto/maze
            graph.Init(mapInstance);                // criação do grid

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if(graphView != null){
                graphView.Init(graph);
            }
        }
    }
}
