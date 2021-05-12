using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGraphScript : MonoBehaviour
{
    public int numOfBaseGraphs;
    public GameObject baseGraphGO;
    public GameObject[] baseGraphs;
    public GameObject edgeGO;
    public int size;
    // Start is called before the first frame update
    void Start()
    {
        baseGraphs = new GameObject[numOfBaseGraphs];
        GameObject bg;

        if(numOfBaseGraphs == 1)
        {
            baseGraphs[0] = baseGraphGO;
            bg = Instantiate(baseGraphs[0], transform.position, Quaternion.identity);
            bg.name = "Base Graph " + 1;
            bg.GetComponent<baseGraphScript2>().setGraphNo(1);
            bg.GetComponent<baseGraphScript2>().size = size;
            bg.transform.parent = gameObject.transform;
        }

        if(numOfBaseGraphs == 2)
        {
            for(int i = 0; i < numOfBaseGraphs; i++)
            {
                baseGraphs[i] = baseGraphGO;
            }

            bg = Instantiate(baseGraphs[0], transform.position, Quaternion.identity);
            bg.name = "Base Graph " + 1;
            bg.GetComponent<baseGraphScript2>().setGraphNo(1);
            bg.GetComponent<baseGraphScript2>().size = size;
            bg.transform.parent = gameObject.transform;
            bg.transform.Rotate(0,0,-90f,Space.Self);

            bg = Instantiate(baseGraphs[1], transform.position + Vector3.right*size, Quaternion.identity);
            bg.name = "Base Graph " + 2;
            bg.GetComponent<baseGraphScript2>().setGraphNo(2);
            bg.GetComponent<baseGraphScript2>().size = size;
            bg.transform.parent = gameObject.transform;

        }

        positionCamera();

        connectBaseGraphs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void positionCamera()
    {
        Camera.main.transform.position = new Vector3((size-1f), -(size-1f)/2f, Camera.main.transform.position.z);
        Camera.main.orthographicSize *= (size/Camera.main.orthographicSize);
    }

    void connectBaseGraphs()
    {
        if(numOfBaseGraphs == 2)
        {
            Instantiate(edgeGO, new Vector3(size-0.5f, 0, 0), Quaternion.identity);
            Instantiate(edgeGO, new Vector3(size-0.5f, -size+1, 0), Quaternion.identity);
        }
    }
}
