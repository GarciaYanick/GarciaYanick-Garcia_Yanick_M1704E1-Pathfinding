using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3, token4;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];
    private List<Node> openList = new List<Node>();
    private List<Node> closedList = new List<Node>();

    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();
        Node inicial = new Node(null, Calculator.CheckDistanceToObj(startPos, objectivePos), new Vector2(startPos[0], startPos[1]));
        //Lo primero será colocar el primer nodo que tengamos com el inicial de ambas listas.
        openList.Add(inicial);
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }

    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }

    
    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {
        if(!EvaluateWin())
        {
            //En caso de aún no ganar se ejecuta una nueva comprobación de los caminos y se crean nuevos nodos.
            openList.Sort();
            openList.AddRange(CreateNodesAround(openList[0]));
            closedList.Add(openList[0]);
            openList.Remove(openList[0]);
        }
        else
        {
            //Avance y creación de posibles caminos
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                CreatePaths(closedList[closedList.Count - 1]);
            }
        }
    }
    private bool EvaluateWin()
    {
        if (closedList.Count == 0)
        {
            return false;
        }
        if (closedList[closedList.Count - 1].position == new Vector3(objectivePos[0], objectivePos[1]))
            return true;
        return false;
    }

    public static int CompareTotalCost(Node node_1, Node node_2)
    {
        return node_1.heuristic.CompareTo(node_2.heuristic);
    }

    public List<Node> CreateNodesAround(Node currentNode)
    {
        List<Node> nodes = new List<Node>();
        for(int i = 0; i<4; i++)
        {
            nodes.Add(CreateNodes(currentNode, i));
        }
        return nodes;
    }

    public Node CreateNodes(Node currentNode, int currentDir)
    {
        Vector2 position = new Vector2();
        int[] nodePos = {0,0};
        switch (currentDir)
        {
            case 0:
                position = new Vector2(currentNode.position.x + 1, currentNode.position.y);
                break;
            case 1:
                position = new Vector2(currentNode.position.x, currentNode.position.y - 1);
                break;
            case 2:
                position = new Vector2(currentNode.position.x - 1, currentNode.position.y);
                break;
            case 3:
                position = new Vector2(currentNode.position.x, currentNode.position.y + 1);
                break;
        }

        nodePos[0] = System.Convert.ToInt32(position[0]);
        nodePos[1] = System.Convert.ToInt32(position[1]);

        int[] positionCal = { (int)position[0], (int)position[1] };
        InstantiateToken(token3, positionCal);
        return new Node(currentNode, Calculator.CheckDistanceToObj(nodePos, objectivePos), position);
    }

    public void CreatePaths(Node nextNode)
    {
        if(nextNode.fatherNode != null)
        {
            int[] position = { (int)nextNode.fatherNode.position.x, (int)nextNode.fatherNode.position.y};
            InstantiateToken(token4, position);
            CreatePaths(nextNode.fatherNode);
        }
    }
}
