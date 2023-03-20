using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    //*** Mostra NODE individual no grid da tela
    //*** Atribui posicao no grid e nome do NODE <-----------------------
    public GameObject tile;
    public GameObject arrow;
    Node m_node;

    [Range(0,0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node){
        if(tile != null){
            gameObject.name = "Node (" + node.xIndex + "," + node.yIndex + ")"; // atribui o nome o node
            gameObject.transform.position = node.position;  // posiciona o node no grid
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
            m_node = node;
            EnableObject(arrow, false);
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

    #region Apontamento do Arrow para NODE anterior
    private void EnableObject(GameObject go, bool state) {
        if(go != null){
            go.SetActive(state);
        }
    }

    // mostra arrow apontando para o node anterior
    public void ShowArrow(Color color){
        if(m_node != null && arrow != null && m_node.previous != null){

            EnableObject(arrow, true);

            Vector3 dirToPrevious = (m_node.previous.position - m_node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(dirToPrevious);

            //ShowArrow2D(m_node.previous.position, m_node.position, arrow);

            Renderer arrowRenderer = arrow.GetComponent<Renderer>();
            if(arrowRenderer != null){
                arrowRenderer.material.color = color;
            }
        }
    }

    void ShowArrow2D(Vector3 previous, Vector3 node, GameObject arrow){
        Vector3 direction = previous - node;
        float angulo = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion rotation = Quaternion.AngleAxis(angulo, Vector3.forward);

        arrow.transform.eulerAngles = Vector3.forward * angulo;

    }
    #endregion

}
