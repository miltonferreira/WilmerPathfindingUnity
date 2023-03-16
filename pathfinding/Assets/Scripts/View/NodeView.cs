using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    //*** Atribui posicao no grid e nome do NODE <-----------------------
    public GameObject tile;

    [Range(0,0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node){
        if(tile != null){
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")"; // atribui o nome o node
            gameObject.transform.position = node.position;  // posiciona o node no grid
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
        }
    }

    void ColorNode(Color color, GameObject go){ // atribui a cor do node
        if(go != null){
            Renderer goRenderer = go.GetComponent<Renderer>();

            if(goRenderer != null){
                goRenderer.material.color = color;
            }
        }
    }

    public void ColorNode(Color color){ // atribui a cor do node simplificado
        ColorNode(color, tile);
    }

}
