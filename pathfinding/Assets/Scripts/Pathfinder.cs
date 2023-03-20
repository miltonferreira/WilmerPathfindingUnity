using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    //*** Tira NODE da fronteira e adiciona os vizinhos a lista da fronteira, ajustando suas cores <-----------------------

    Node m_startNode;
    Node m_goalNode;
    Graph m_graph;
    GraphView m_graphView;
    Queue<Node> m_frontierNodes;    // fronteira para explorar * rosa
    List<Node> m_exploredNodes;     // ponto de exploração * cinza explorado / verde ponto inicial
    List<Node> m_pathNodes;

    #region cores dos nodes
    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color32(216, 216, 216, 255);
    public Color highlightColor = new Color32(255, 255, 128, 255);
    #endregion

    public bool isComplete = false;
    int m_iterations = 0;


    public void Init(Graph graph, GraphView graphView, Node start, Node goal){
        if(graph == null || graphView == null || start == null || goal == null){
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return;
        }

        if(start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked){
            Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
            return;
        }

        // atribui os parametros
        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;

        ShowColors(graphView, start, goal);

        m_frontierNodes = new Queue<Node>();    // cria lista de fronteira de nodes
        m_frontierNodes.Enqueue(start);         // ponto incial de busca de enfileiramento
        
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        for(int x = 0; x < m_graph.Width; x++){
            for(int y = 0; y < m_graph.Height; y++){
                m_graph.nodes[x,y].Reset();
            }
        }

        isComplete = false;
        m_iterations = 0;

    }

    void ShowColors(){
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    // gerencia as cores do nodes
    void ShowColors(GraphView graphView, Node start, Node goal){

        if(graphView == null || start == null || goal == null){
            return;
        }

        if(m_frontierNodes != null){    // rosa para fronteira
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if(m_exploredNodes != null){    // cinza para explorado
            graphView.ColorNodes(m_exploredNodes.ToList(), exploredColor);
        }

        if(m_pathNodes != null && m_pathNodes.Count > 0){    // azul claro para caminho
            graphView.ColorNodes(m_pathNodes.ToList(), pathColor);
        }

        //Node inicial de busca
        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if(startNodeView != null){
            startNodeView.ColorNode(startColor);    // atribui cor verde
        }

        //Node final de busca
        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if(goalNodeView != null){
            goalNodeView.ColorNode(goalColor);      // atribui cor vermelha
        }
    }

    // Tira NODE da fronteira e adiciona os vizinhos a lista da fronteira
    public IEnumerator SearchRoutine(float timeStep = 0.1f){
        yield return null;

        while(!isComplete){
            if(m_frontierNodes.Count > 0){
                Node currentNode = m_frontierNodes.Dequeue();   //  tira NODE da fila de fronteira
                m_iterations++;
                if(!m_exploredNodes.Contains(currentNode)){
                    m_exploredNodes.Add(currentNode);           // adiciona o NODE aos explorados
                }
                ExpandFrontier(currentNode);                    //  procura vizinhos para expandir fronteira
                
                #region sistema de caminho entre os NODES
                if(m_frontierNodes.Contains(m_goalNode)){
                    m_pathNodes = GetPathNodes(m_goalNode);
                }
                #endregion

                ShowColors();                                   //  ajusta as cores dos NODES

                #region sistema de arrows
                if(m_graphView){
                    m_graphView.ShowNodeArrows(m_frontierNodes.ToList(), arrowColor);
                    if(m_frontierNodes.Contains(m_goalNode)){
                        m_graphView.ShowNodeArrows(m_pathNodes, highlightColor);
                    }
                }
                #endregion

                yield return new WaitForSeconds(timeStep);

            }else{
                isComplete = true;
            }
        }
    }

    // pega vizinhos do NODE e adiciona a fronteira da pesquisa
    void ExpandFrontier(Node node){
        if(node != null){
            for(int i =0; i < node.neighbors.Count; i++){               // pega lista de vizinhos do NODE
                if(!m_exploredNodes.Contains(node.neighbors[i]) 
                    && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    node.neighbors[i].previous = node;              // pega o vizinho e atribui o node como anterior
                    m_frontierNodes.Enqueue(node.neighbors[i]);     // coloca o vizinho na fila de fronteira
                }
            }
        }
    }

    List<Node> GetPathNodes(Node endNode){

        List<Node> path = new List<Node>();

        if(endNode == null){
            return path;
        }

        path.Add(endNode);

        Node currentNode = endNode.previous;

        while(currentNode != null){
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }

}
