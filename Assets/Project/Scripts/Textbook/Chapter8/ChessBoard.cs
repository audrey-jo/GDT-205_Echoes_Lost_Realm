/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    //Chess board is always 8x8
    public static int boardDimensions = 8;
    public float fCellSize = 100.0f;
    public float fHalfCellSize = 50;

    [SerializeField] private GameObject prefabToSpawn = null;
    [SerializeField] private Transform pieceParent = null;
    [SerializeField] private Transform topLeftCorner = null;

    [SerializeField] private Transform moveHighlightParent = null;
    [SerializeField] private GameObject highlightPrefabToSpawn = null;
    [SerializeField] private GameObject highlight = null;

    //This is the actual data - internal for our use.
    public PieceDetails[,] boardLayout;

    //This is an array of references to the pieces so we can visually move them when required.
    public ChessPiece[,] boardVisuals;

    //This is an array of references to the pieces so we can visually move them when required.
    public GameObject[,] moveHighlights;

    //--------------------------------------------------------------------------------------

    void Start()
    {
        CreateBoard();
    }

    //--------------------------------------------------------------------------------------

    void CreateBoard()
    {
        if (prefabToSpawn != null && pieceParent != null && topLeftCorner != null)
        {
            //Chess board is always 8x8
            boardLayout = new PieceDetails[boardDimensions, boardDimensions];
            boardVisuals = new ChessPiece[boardDimensions, boardDimensions];
            moveHighlights = new GameObject[boardDimensions, boardDimensions];

            //Set all cells to be null.
            for (int iRow = 0; iRow < boardDimensions; iRow++)
            {
                for (int iCol = 0; iCol < boardDimensions; iCol++)
                {
                    PieceDetails pieceDetails = GetStartingPieceAt(iRow, iCol);

                    //If there is a piece here, lets spawn that piece and position appropriately.
                    if (pieceDetails.pieceType != PieceType.None)
                    {
                        Vector3 startPosition = topLeftCorner.position + new Vector3((iCol * fCellSize) + fHalfCellSize, (iRow * -fCellSize) - fHalfCellSize, 0.0f);
                        GameObject newPiece = Instantiate(prefabToSpawn, startPosition, Quaternion.identity, pieceParent);

                        ChessPiece chessPiece = newPiece.GetComponent<ChessPiece>();
                        if (chessPiece != null)
                        {
                            chessPiece.SetDetails(pieceDetails);
                            boardVisuals[iRow, iCol] = chessPiece;
                        }
                    }

                    //Internal board data.
                    boardLayout[iRow, iCol] = pieceDetails;

                    //Move highlights.
                    Vector3 moveHighlightPosition = topLeftCorner.position + new Vector3((iCol * fCellSize) + fHalfCellSize, (iRow * -fCellSize) - fHalfCellSize, 0.0f);
                    GameObject newHighlight = Instantiate(highlightPrefabToSpawn, moveHighlightPosition, Quaternion.identity, moveHighlightParent);
                    newHighlight.SetActive(false);
                    if (newHighlight != null)
                    {
                        moveHighlights[iRow, iCol] = newHighlight;
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void CopyInternalBoard(ChessBoard boardToCopy)
    {
        //Chess board is always 8x8
        boardLayout = new PieceDetails[boardDimensions, boardDimensions];

        //Set all cells to be null.
        for (int iRow = 0; iRow < 8; iRow++)
        {
            for (int iCol = 0; iCol < 8; iCol++)
            {
                boardLayout[iRow, iCol] = boardToCopy.boardLayout[iRow, iCol];
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void SwapPieceType(int iRow, int iCol, PieceType toType)
    {
        if (iRow >= 0 && iRow < boardDimensions && iCol >= 0 && iCol < boardDimensions)
        {
            boardLayout[iRow, iCol].pieceType = toType;

            if (boardVisuals != null)
            {
                boardVisuals[iRow, iCol].SetPieceImage();
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void KillPiece(int iRow, int iCol)
    {
        if (iRow >= 0 && iRow < boardDimensions && iCol >= 0 && iCol < boardDimensions)
        {
            boardLayout[iRow, iCol].Clear();

            if (boardVisuals != null)
            {
                if (boardVisuals[iRow, iCol] != null)
                {
                    Destroy(boardVisuals[iRow, iCol].gameObject);
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (fromRow >= 0 && fromRow < boardDimensions && fromCol >= 0 && fromCol < boardDimensions &&
            toRow >= 0 && toRow < boardDimensions && toCol >= 0 && toCol < boardDimensions)
        {
            boardLayout[fromRow, fromCol].bHasMoved = true;
            //boardLayout[fromRow, fromCol].bCanEnPassant. = true;
            boardLayout[toRow, toCol] = boardLayout[fromRow, fromCol];
            boardLayout[fromRow, fromCol].Clear();

            if (boardVisuals != null)
            {
                Vector3 newPosition = topLeftCorner.position + new Vector3((toCol * fCellSize) + fHalfCellSize, (toRow * -fCellSize) - fHalfCellSize, 0.0f);

                boardVisuals[toRow, toCol] = boardVisuals[fromRow, fromCol];
                boardVisuals[fromRow, fromCol] = null;
                boardVisuals[toRow, toCol].transform.position = newPosition;
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public PieceDetails GetStartingPieceAt(int row, int column)
    {
        //Default settings
        PieceDetails piece;
        piece.pieceType = PieceType.None;
        piece.pieceColour = PieceColour.None;
        piece.bCanEnPassant = false;
        piece.bHasMoved = false;

        //Set type and colour:
        if (row == 0)
        {
            piece.pieceColour = PieceColour.White;
            switch (column)
            {
                case 0: piece.pieceType = PieceType.Rook;      break;
                case 1: piece.pieceType = PieceType.Knight;    break;
                case 2: piece.pieceType = PieceType.Bishop;    break;
                case 3: piece.pieceType = PieceType.King;      break;
                case 4: piece.pieceType = PieceType.Queen;     break;
                case 5: piece.pieceType = PieceType.Bishop;    break;
                case 6: piece.pieceType = PieceType.Knight;    break;
                case 7: piece.pieceType = PieceType.Rook;      break;
            }
        }
        else if (row == 1)
        {
            piece.pieceColour = PieceColour.White;
            piece.pieceType = PieceType.Pawn;
        }
        else if (row == 6)
        {
            piece.pieceColour = PieceColour.Black;
            piece.pieceType = PieceType.Pawn;
        }
        else if (row == 7)
        {
            piece.pieceColour = PieceColour.Black;
            switch (column)
            {
                case 0: piece.pieceType = PieceType.Rook;      break;
                case 1: piece.pieceType = PieceType.Knight;    break;
                case 2: piece.pieceType = PieceType.Bishop;    break;
                case 3: piece.pieceType = PieceType.King;      break;
                case 4: piece.pieceType = PieceType.Queen;     break;
                case 5: piece.pieceType = PieceType.Bishop;    break;
                case 6: piece.pieceType = PieceType.Knight;    break;
                case 7: piece.pieceType = PieceType.Rook;      break;
            }
        }

        return piece;
    }

    //--------------------------------------------------------------------------------------

    public GridPosition SelectCell(Vector3 position)
    {
        if(highlight != null && topLeftCorner != null)
        {
            GridPosition selectedGridPosition;
            selectedGridPosition.row = (int)((topLeftCorner.position.y - position.y) / fCellSize);
            selectedGridPosition.column = (int)((position.x - topLeftCorner.position.x) / fCellSize);

            //Ensure the highlight is on the board.
            if (selectedGridPosition.row < boardDimensions && selectedGridPosition.column < boardDimensions)
            {
                Vector3 highlightPosition = topLeftCorner.position + new Vector3((selectedGridPosition.column * fCellSize) + fHalfCellSize, (-selectedGridPosition.row * fCellSize) - fHalfCellSize, 0.0f);
                highlight.transform.position = highlightPosition;
                return selectedGridPosition;
            }
        }

        return new GridPosition();
    }

    //--------------------------------------------------------------------------------------

    public void HideHighlight()
    {
        if (highlight != null)
        {
            highlight.transform.position = new Vector3(-999.0f, -999.0f, 0.0f);
        }
    }

    //--------------------------------------------------------------------------------------

    public void ClearMoveOptions()
    {
        //Clear previous highlights.
        for (int iRow = 0; iRow < 8; iRow++)
        {
            for (int iCol = 0; iCol < 8; iCol++)
            {
                moveHighlights[iRow, iCol].gameObject.SetActive(false);
            }
        }
    }

    //--------------------------------------------------------------------------------------

    public void HighlightMoveOptions(List<GridPosition> moveOptions)
    {
        //Clear previous highlights.
        ClearMoveOptions();

        //Show new highlights.
        for (int i = 0; i < moveOptions.Count; i++)
        {
            int iRow = moveOptions[i].row;
            int iCol = moveOptions[i].column;

            if(iRow >= 0 && iRow < boardDimensions && iCol >= 0 && iCol < boardDimensions)
            {
                moveHighlights[iRow, iCol].gameObject.SetActive(true);
            }
        }
    }

    //--------------------------------------------------------------------------------------
}
*/