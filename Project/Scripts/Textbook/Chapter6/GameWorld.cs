using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public static bool GameBegan     = false;
    public static bool GamePaused    = false;

    public static int BoardRows      = 31;
    public static int BoardColumns   = 28;

    //0 = Obstacle, 1 = Passable, 2 = Ghost only passable.
    public static int[,] Tiles = {
    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0 },
    { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
    { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
    { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
    { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
    { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0 },
    { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0 },
    { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0 },
    { 0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,0,0,0,2,2,0,0,0,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,0,2,2,2,2,2,2,0,1,0,0,1,0,0,0,0,0,0 },
    { 1,1,1,1,1,1,1,1,1,1,0,2,2,2,2,2,2,0,1,1,1,1,1,1,1,1,1,1 },
    { 0,0,0,0,0,0,1,0,0,1,0,2,2,2,2,2,2,0,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0 },
    { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0 },
    { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0 },
    { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
    { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
    { 0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0 },
    { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0 },
    { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0 },
    { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0 },
    { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0 },
    { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0 },
    { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0 },
    { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 } };

    [SerializeField] Transform  pillParent      = null;
    [SerializeField] GameObject pillPrefab      = null;

    //----------------------------------------------------------------------------------------------

    void Start()
    {
        Screen.SetResolution(1024, 768, false);

        Pill.pillCount = 0;
        GeneratePills();

        //Start the game.
        if (!GameWorld.GameBegan && !GameWorld.GamePaused)
        {
            GameWorld.GameBegan = true;
        }
    }

    //----------------------------------------------------------------------------------------------

    void GeneratePills()
    {
        //Place a pill on each passable tile.
        for(int col = 0; col < BoardColumns; col++)
        {
            for(int row = 0; row < BoardRows; row++)
            {
                if(Tiles[row,col] == 1)
                {
                    GameObject newPill = Instantiate(pillPrefab, new Vector3(col, -row, 0.0f), Quaternion.identity, pillParent);

                    //Set the corner pulls to be power pills.
                    if ((col == 1 && row == 1) || (col == 26 && row == 1) || (col == 1 && row == 29) || (col == 26 && row == 29))
                    {
                        newPill.GetComponent<Pill>().SetPillType(PillType.Power);
                    }
                }
            }
        }
    }

    //----------------------------------------------------------------------------------------------

    public static Vector2Int GetBoardPosition(Vector2 worldPosition)
    {
        worldPosition.x += 0.5f;
        worldPosition.x /= 31.0f;
        worldPosition.y /= -28.0f;
        Vector2Int boardPosition = new Vector2Int((int)(worldPosition.x * BoardRows), (int)(worldPosition.y * BoardColumns));
        return boardPosition;
    }

    //----------------------------------------------------------------------------------------------

    public static Vector3 GetWorldPosition(Vector2Int boardPosition)
    {
        Vector3 worldPosition = new Vector3 (boardPosition.x, boardPosition.y, 0.0f);
        return worldPosition;
    }

    //----------------------------------------------------------------------------------------------

    public static bool IsCellAccessible(Vector2Int boardPos)
    {
        //Don't allow off the board checks.
        if (boardPos.x < 0 || boardPos.x >= BoardColumns)//28
        {
            return true;
        }

        //Return the value at this position in the tiles array.
        return Tiles[boardPos.y, boardPos.x] == 1;
    }

    //----------------------------------------------------------------------------------------------

    public static bool IsCellAccessibleToGhost(Vector2Int boardPos)
    {
        //Don't allow off the board checks.
        if (boardPos.x < 0 || boardPos.x >= BoardColumns)//28
        {
            return true;
        }

        //Return the value at this position in the tiles array.
        return Tiles[boardPos.y, boardPos.x] >= 1;
    }

    //----------------------------------------------------------------------------------------------

    public static int ReturnCellValue(Vector2Int boardPos)
    {
        //Don't allow off the board checks.
        if (boardPos.x < 0 || boardPos.x >= BoardColumns)//28
        {
            return 0;
        }

        if (boardPos.y < 0 || boardPos.y >= BoardRows)//31
        {
            return 0;
        }

        return Tiles[boardPos.y, boardPos.x];
    }

    //----------------------------------------------------------------------------------------------
}
