using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Heatmap
{
    public static int Rows;
    public static int Columns;

    public static int HomeCol;
    public static int HomeRow;

    //3 locations for food.
    public static int FoodCol1;
    public static int FoodRow1;

    public static int FoodCol2;
    public static int FoodRow2;

    public static int FoodCol3;
    public static int FoodRow3;

    public static GameObject[,] grid;
}
