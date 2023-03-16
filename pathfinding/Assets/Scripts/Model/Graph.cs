using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //*** Posiciona os node no grid e nodes pegam posições dos vizinhos <---------------------

    public Node[,] nodes;
    public List<Node> walls = new List<Node>(); // lista de parede/bloqueio caminho

    int[,] m_mapData;
    int m_width;
    int m_height;

    // direção dos vizinhos do Node/ponto
    public static readonly Vector2[] allDirections = {
        new Vector2(0f,1f),
        new Vector2(1f,1f),
        new Vector2(1f,0f),
        new Vector2(1f,-1f),
        new Vector2(0f,-1f),
        new Vector2(-1f,-1f),
        new Vector2(-1f,0f),
        new Vector2(-1f,1f)
    };

    public void Init(int[,] mapData){   // posiciona os node e pega vizinhos
        m_mapData = mapData;

        m_width = mapData.GetLength(0);         // pega a largura
        m_height = mapData.GetLength(1);        // pega a altura

        nodes = new Node[m_width, m_height];    // cria nova coluna/linha

        // posiciona os nodes/quadrados em suas posições
        for(int y = 0; y < m_height; y++){
            for(int x = 0; x < m_width; x++){
                NodeType type = (NodeType)mapData[x,y];
                
                Node newNode = new Node(x,y,type);  // cria novo node
                nodes[x,y] = newNode;               // atribui a linha/coluna

                // posiciona na grid o node
                newNode.position = new Vector3((-4.5f+x),(-4.5f+y),0);

                if(type == NodeType.Blocked){
                    walls.Add((newNode));
                }
            }
        }

        //Cada NODE pega referencias dos vizinhos
        for(int y = 0; y < m_height; y++){
            for(int x = 0; x < m_width; x++){
                if(nodes[x,y].nodeType != NodeType.Blocked){
                    nodes[x,y].neighbors = GetNeighbors(x,y);
                }
            }
        }

    }

    
    public bool IsWithinBounds(int x, int y){   // limite de altura e largura mapData
        return (x >=0 && x < m_width && y>= 0 && y < m_height);
    }

    // faz ligação de um NODE com seus vizinhos
    List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions){
        List<Node> neighborsNodes = new List<Node>();

        foreach(Vector2 dir in directions){
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            if(IsWithinBounds(newX,newY) && nodeArray[newX,newY] != null && 
            nodeArray[newX,newY].nodeType != NodeType.Blocked){
                neighborsNodes.Add(nodeArray[newX,newY]);
            }
        }

        return neighborsNodes;

    }

    // *simplificado* faz ligação de um NODE com seus vizinhos
    List<Node> GetNeighbors(int x, int y){
        return GetNeighbors(x,y, nodes, allDirections);
    }
}
