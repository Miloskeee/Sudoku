using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickControler : MonoBehaviour
{

    [SerializeField]
    private GameObject selectedField;
    [SerializeField]
    private GameObject selectedNumber;
    [SerializeField]
    private SudokuControler sudoku;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ButtonClicked()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        if(selectedObject != null)
        {
            //Debug.Log("Clicked on : " + selectedObject.name);

            if (selectedObject.tag == "SudokuButton")
            {
                selectedField = selectedObject;
            }
            else if (selectedObject.tag == "NumericButton")
            {
                selectedNumber = selectedObject;

                if(selectedField != null)
                {
                    int number;

                    Field field = selectedField.GetComponent<Field>();

                    bool tryParse = int.TryParse(selectedNumber.name, out number);

                    if (tryParse)
                    {
                        sudoku.SetNewNumber(field.rowIndex, field.columnIndex, number);
                    }
                }
            }
        }
    }
}
