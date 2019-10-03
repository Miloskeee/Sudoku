using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SudokuControler : MonoBehaviour
{
    private int[][] matrix;
    private int[][] filledMatrix;

    [SerializeField]
    private int N = 9;//broj redova i kolona
    [SerializeField]
    private int SQRT; //koren iz N
    [SerializeField]
    private int K; // Koliko cifara 

    private GameObject[][] table;

    [SerializeField]
    GameObject[] sudokuButtons;

    [SerializeField]
    private GameControler game;

    [SerializeField]
    private List<GameObject> missingFields = new List<GameObject>();

    
    private void Start()
    {
        InitializeTable();

        double SQ = Math.Sqrt(N);
        SQRT = Convert.ToInt32(SQ);

        //Inicijalizujemo polja
        sudokuButtons = GameObject.FindGameObjectsWithTag("SudokuButton");
        int i = 0;
        int j = 0;

        foreach(GameObject sb in sudokuButtons)
        {
            Debug.Log(sb);

            if((j%9)== 0 && j != 0)
            {
                i++;
                j = 0;
                table[i][j] = sb;
                sb.GetComponent<Field>().rowIndex = i;
                sb.GetComponent<Field>().columnIndex = j;
                j++;

            }
            else
            {
                table[i][j] = sb;
                sb.GetComponent<Field>().rowIndex = i;
                sb.GetComponent<Field>().columnIndex = j;
                j++;

            }

        }

        fillValues();

    }

    public void ResetValues()
    {
        fillValues();
    }

    private void Initialize()
    {

        matrix = new int[N][];
        table = new GameObject[N][];

        for (int i = 0; i < N; i++)
        {
            matrix[i] = new int[N];
            table[i] = new GameObject[N];
        }
    }



    public void InitializeMatrix()
    {

        matrix = new int[N][];
        for (int i = 0; i < N; i++)
        {
            matrix[i] = new int[N];

        }
    }


    private void InitializeTable()
    {
        table = new GameObject[N][];

        for (int i = 0; i < N; i++)
        {

            table[i] = new GameObject[N];
        }
    }

    //Upis brojeva u tableu
    public void SetNewNumber(int row, int col, int number)
    {
        matrix[row][col] = 0;

        if (number == 0)
        {
            table[row][col].GetComponentInChildren<Text>().text = "";
            table[row][col].GetComponentInChildren<Text>().color = Color.white;
            if (!missingFields.Contains(table[row][col]))
            {
                missingFields.Add(table[row][col]);
            }
            return;

        }

        //Debug.Log("Setting new number");
        if (game.isEasy())
        {
            Debug.Log("easy mod");
            if (isAllowed(row, col, number))
            {
                //upisuje broj belom bojom
                matrix[row][col] = number;
                table[row][col].GetComponentInChildren<Text>().text = number.ToString();
                table[row][col].GetComponentInChildren<Text>().color = Color.white;

                missingFields.Remove(table[row][col]);
            }


            else
            {
                matrix[row][col] = number;
                //upisi broj crvenom bojom
                table[row][col].GetComponentInChildren<Text>().text = number.ToString();
                table[row][col].GetComponentInChildren<Text>().color = Color.red;

                if (!missingFields.Contains(table[row][row]))
                {
                    missingFields.Add(table[row][row]);
                }
            }
        }
        else
        {
            Debug.Log("hard mod");

            //upisujemo broj belom 
            matrix[row][col] = number;
            table[row][col].GetComponentInChildren<Text>().text = number.ToString();
            table[row][col].GetComponentInChildren<Text>().color = Color.white;

            if (isAllowed(row, col, number))
                missingFields.Remove(table[row][col]);
            else
            {
                if (!missingFields.Contains(table[row][col]))
                    missingFields.Add(table[row][col]);
            }

        }
    }

    private bool CheckFullBoard()
    {

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (!isAllowed(i, j, matrix[i][j]))
                    return false;
            }
        }

        return true;
    }

    private bool ContainsInRow(int row, int number)
    {
        for (int i = 0; i < 9; i++)
        {
            if (matrix[row][i] == number)
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainsInCol(int col, int number)
    {
        for (int i = 0; i < 9; i++)
        {
            if (matrix[i][col] == number)
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainsInBox(int row, int col, int number)
    {
        int r = row - row % 3;
        int c = col - col % 3;
        for (int i = r; i < r + 3; i++)
        {
            for (int j = c; j < c + 3; j++)
            {
                if (matrix[i][j] == number)
                {
                    return true;
                }
            }

        }
        return false;
    }

    public bool isAllowed(int row, int col, int number)
    {
        if (number == 0)
        {
            return true;
        }

        return !(ContainsInRow(row, number) || ContainsInCol(col, number) || ContainsInBox(row, col, number));
    }

    public bool isItSafe(int row, int col, int number)
    {
        //provera celom duzinom pre broja
        for (int i = 0; i < row; i++)
        {

            if (matrix[i][col] == number)
            {
                return false;
            }
        }
        //provera celom duzinom posle broja
        for (int i = row + 1; i < N; i++)
        {

            if (matrix[i][col] == number)
            {
                return false;
            }
        }


        //provera celom sirinom pre broja
        for (int j = 0; j < col; j++)
        {
            if (matrix[row][j] == number)
            {
                return false;
            }
        }

        //provera celom sirinom posle broja
        for (int j = col + 1; j < N; j++)
        {
            if (matrix[row][j] == number)
            {
                return false;
            }
        }

        //proveri u okolini
        int i1 = row, i2 = row, j1 = col, j2 = col;

        while (i1 % 3 != 0)
            i1--;
        while (j1 % 3 != 0)
            j1--;
        while (i2 % 3 != 2)
            i2++;
        while (j2 % 3 != 2)
            j2++;
        for (int k = i1; k <= i2; k++)
        {
            for (int l = j1; l <= j2; l++)
            {
                if (k == row && l == col)
                    continue;
                if (matrix[k][l] == number)
                    return false;
            }
        }

        return true;
    }


    

    public void fillTable()
    {
        for (int i = 0; i < table.Length; i++)
        {
            for (int j = 0; j < table[i].Length; j++)
            {
                if (matrix[i][j] != 0)
                {
                    table[i][j].GetComponentInChildren<Text>().text = matrix[i][j].ToString();
                    table[i][j].GetComponent<Button>().interactable = false;
                    table[i][j].GetComponentInChildren<Text>().color = Color.gray;

                }
                else
                {
                    table[i][j].GetComponentInChildren<Text>().text = "";
                    table[i][j].GetComponent<Button>().interactable = true;
                    table[i][j].GetComponentInChildren<Text>().color = Color.white;

                }
            }
        }
    }

    //Generisemo igru
    public void fillValues()
    {
        InitializeMatrix();
        missingFields = new List<GameObject>();
        //Popunjavamo diagonalu 3x3 matrice
        fillDiagonal();

        //Popunjava ostala polja
        fillRemaining(0, SQRT);
        filledMatrix = matrix;

        //Brise random K brojeve 
        removeRandomKDigits();

        fillTable();





    }

    public void fillDiagonal()
    {
        for (int i = 0; i < N; i = i + SQRT)
        {
            fillBox(i, i);
        }

    }

    //Proverava matricu 3x3 i vraca false ako number postoji
    public bool unUsedInBox(int row, int col, int number)
    {
        for (int i = 0; i < SQRT; i++)
        {
            for (int j = 0; j < SQRT; j++)
            {
                if (matrix[row + i][col + j] == number)
                {
                    return false;
                }

            }
        }

        return true;
    }

    //Popunjava matricu 3x3
    public void fillBox(int row, int col)
    {
        int number;
        for (int i = 0; i < SQRT; i++)
        {
            for (int j = 0; j < SQRT; j++)
            {
                do
                {
                    number = randomGenerator(N);
                } while (!unUsedInBox(row, col, number));

                matrix[row + i][col + j] = number;

            }
        }
    }

    public int randomGenerator(int number)
    {
        System.Random rand = new System.Random();
        return (int)Math.Floor((rand.NextDouble() * number + 1));

    }


    //Provera da li je bezbedno da se upise podatak
    public bool CheckIfSafe(int i, int j, int number)
    {
        return (unUsedInRow(i, number) && unUsedInCol(j, number) && unUsedInBox(i - i % SQRT, j - j % SQRT, number));
    }


    //Provera da li se broj nalazi u istom redu
    public bool unUsedInRow(int i, int number)
    {
        for (int j = 0; j < N; j++)
            if (matrix[i][j] == number)
                return false;
        return true;
    }
    //Provera da li se broj nalazi u istoj koloni
    public bool unUsedInCol(int j, int number)
    {
        for (int i = 0; i < N; i++)
            if (matrix[i][j] == number)
                return false;
        return true;
    }




    //Rekurzivna funk. za popunjavanje ostatka matrice
    public bool fillRemaining(int i, int j)
    {
        if (j >= N && i < N - 1)
        {
            i = i + 1;
            j = 0;

        }
        if (i >= N && j >= N)
        {
            return true;
        }

        if (i < SQRT)
        {
            if (j < SQRT)
            {
                j = SQRT;
            }
        }
        else if (i < N - SQRT)
        {
            if (j == (int)(i / SQRT) * SQRT)
            {
                j = j + SQRT;
            }
        }
        else
        {
            if (j == N - SQRT)
            {
                i = i + 1;
                j = 0;
                if (i >= N)
                {
                    return true;
                }
            }
        }


        for (int k = 1; k <= N; k++)
        {
            if (CheckIfSafe(i, j, k))
            {
                matrix[i][j] = k;
                if (fillRemaining(i, j + 1))
                {
                    return true;

                    
                }
                matrix[i][j] = 0;
            }
        }
        return false;
    }


    //Brisemo K brojeva iz kompletne igre
    public void removeRandomKDigits()
    {
        int count = K;

        while(count != 0)
        {
            int cellID = randomGenerator(N * N) - 1;
            // Debug.Log(cellID);
            int i = (cellID / N);
            int j = cellID % 9;

            if(i >= N)
            {
                i = N - 1;
            }

            //Debug.Log(i + "" + j);
            if(matrix[i][j] != 0)
            {
                count--;
                matrix[i][j] = 0;

                missingFields.Add(table[i][j]);
            }
        }
    }


    //Print Sudoku
    public void PrintSudoku()
    {
        string line = "";

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                line += (matrix[i][j] + " ");

            }

            Debug.Log(line);
            line = "";
        }
    }

    public void SetAllButtonsClickable()
    {
        for (int i = 0; i < table.Length; i++)
        {
            for (int j = 0; j < table[i].Length; j++)
            {
                if (matrix[i][j] != 0)
                    table[i][j].GetComponent<Button>().interactable = true;
            }
        }
    }


    public int missingFieldsCount()
    {
        return missingFields.Count;
    }

    public int getK()
    {
        return K;
    }

    public void setK(int newK)
    {
        K = newK;
    }


}











