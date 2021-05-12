using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeScript : MonoBehaviour
{
    public bool active;
    public int vertexARow, vertexACol, vertexBRow, vertexBCol;
    public int weight, a, b, graphNo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive(bool a)
    {
        active = a;
    }

    public bool isActive()
    {
        return active;
    }

    public void setVertices(int ar, int ac, int br, int bc)
    {
        vertexARow = ar;
        vertexACol = ac;
        vertexBRow = br;
        vertexBCol = bc;
    }

    public void setA(int name)
    {
        a = name;
    }

    public int getA()
    {
        return a;
    }

    public void setB(int name)
    {
        b = name;
    }

    public int getB()
    {
        return b;
    }
    public int getAR()
    {
        return vertexARow;
    }

    public int getAC()
    {
        return vertexACol;
    }

    public int getBR()
    {
        return vertexBRow;
    }

    public int getBC()
    {
        return vertexBCol;
    }

    public string printStatus()
    {
        return graphNo + " - [" + vertexARow + "," + vertexACol + "]-[" + vertexBRow + "," + vertexBCol + "]";
    }

    public bool isTheSame(edgeScript e)
    {
        bool exists = false;

        if(getAR() == e.getAR())
        {
            if(getAC() == e.getAC())
            {
                if(getBR() == e.getBR())
                {
                    if(getBC() == e.getBC())
                    {
                        exists = true;
                    }
                }
            }
        }
        return exists;
    }

    public bool isMirroredEdge(edgeScript e)
    {
        bool exists = false;

        if(getAR() == e.getBR())
        {
            if(getAC() == e.getBC())
            {
                if(getBR() == e.getAR())
                {
                    if(getBC() == e.getAC())
                    {
                        exists = true;
                    }
                }
            }
        }
        return exists;
    }

    public void setWeight(int w)
    {
        weight = w;
    }

    public int getWeight()
    {
        return weight;
    }

    public void setGraphNo(int num)
    {
        graphNo = num;
    }

    public int getGraphNo()
    {
        return graphNo;
    }
}
