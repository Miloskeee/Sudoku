using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private int row;
    [SerializeField]
    private int column;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int rowIndex
    {
        get { return row; }
        set { row = value; }
    }

    public int columnIndex
    {
        get { return column; }
        set { column = value; }
    }


}
