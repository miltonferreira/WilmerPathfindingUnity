using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    //*** Indica em que posição criar nodes (caminho ou parede) <---------------------

    // altura e largura do maze
    public int width = 10;
    public int height = 5;

    /*
    preenche a tela
    width = 18
    height = 10;
    */

    public string resourcePath = "Mapdata";

    // usando texto 0 - 1 para compor maze <-----------------------
    public TextAsset textAsset;
    
    // usando imagem pixel branco/preto para compor maze <---------
    public Texture2D textureMap;

    private void Awake() {

        string levelName = SceneManager.GetActiveScene().name;

        if(textureMap == null){
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }

        if(textAsset == null){
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
    }

    // usando imagem pixel branco/preto para compor maze <---------
    public List<string> GetMapFromTexture(Texture2D texture){
        List<string> lines = new List<string>();

        if(texture != null){
            for(int y = 0; y < texture.height; y++){
                string newLine = "";

                for(int x = 0; x < texture.width; x++){
                    if(texture.GetPixel(x,y) == Color.black){
                        newLine += '1';
                    }else if(texture.GetPixel(x,y) == Color.white){
                        newLine += '0';
                    }else{
                        newLine += ' ';
                    }
                }

                lines.Add(newLine);
            }
        }

        return lines;
    }

    // usando texto 0 - 1 para compor maze <-----------------------
    public List<string> GetMapFromTextFile(TextAsset tAsset){  // lê arquivo txt para formar maze
        List<string> lines = new List<string>();

        if(tAsset != null){
            string textData = tAsset.text;
            string[] delimiters = {"\r\n","\n"};    // windows = "\r\n" - Unix/Mac = "\n"
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        // else{
        //     Debug.LogWarning("MapData GetTextFromFile Error: invalid TextAsset");
        // }

        return lines;
    }

    public List<string> GetMapFromTextFile(){
        return GetMapFromTextFile(textAsset);
    }

    public void SetDimensions(List<string> textLines){  // checa consistencia linha/coluna
        height = textLines.Count;
        foreach(string line in textLines){
            if(line.Length > width){
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {

        List<string> lines = new List<string>();

        if(textureMap != null){
            lines = GetMapFromTexture(textureMap);  // cria maze com imagem
        }else{
            lines = GetMapFromTextFile();           // cria maze com texto
        }

        SetDimensions(lines);

        int[,] map = new int[width, height];

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                if(lines[y].Length > x){    // evita error
                    map[x,y] = (int) Char.GetNumericValue(lines[y][x]); // caminho = 0 - parede = 1
                }
                //map[x,y] = 0; //old
            }
        }

        return map;
    }

    void manualWall(int[,] map){
        // posição que vão está bloqueados/blocos pretos
        map[1,0] = 1;
        map[1,1] = 1;
        map[1,2] = 1;
        map[3,2] = 1;
        map[3,3] = 1;
        map[3,4] = 1;
        map[4,2] = 1;
        map[5,1] = 1;
        map[5,2] = 1;
        map[6,2] = 1;
        map[6,3] = 1;
        map[8,0] = 1;
        map[8,1] = 1;
        map[8,2] = 1;
        map[8,4] = 1;
    }

}
