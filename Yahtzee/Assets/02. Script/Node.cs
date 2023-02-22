
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Straight,
    Left,
    Right,
    Fork
}

public class Node : MonoBehaviour
{
    public Direction direction;
    public List<Node> nodeList;
    public Dictionary<Direction, Node> nextNode;

    private void Start()
    {
        //nextNode = new Dictionary<Direction, Node>();

        //if (nodeList.Count <= 2)
        //{
        //    foreach (Node item in nodeList)
        //        nextNode.Add(item.direction, item);
        //}




    }
}
