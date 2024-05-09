using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node> 
{
    public float totalCost;
    public Node fatherNode;
    public float heuristic;
    public Vector3 position;

    public Node(Node fatherNode, float heuristic, Vector3 position)
    {
        this.fatherNode = fatherNode;
        this.heuristic = heuristic;
        this.position = position;
        if (fatherNode == null) totalCost = 1;
        else totalCost = fatherNode.totalCost + heuristic;
    }

    public int CompareTo(Node other)
    {
        if(this.heuristic < other.heuristic) return -1;
        else if(this.heuristic > other.heuristic)return 1;
        else return 0;
    }
}
