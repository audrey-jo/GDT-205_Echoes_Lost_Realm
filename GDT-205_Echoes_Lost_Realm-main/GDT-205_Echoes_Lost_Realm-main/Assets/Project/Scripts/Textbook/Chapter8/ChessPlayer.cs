/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessPlayer : MonoBehaviour
{
	public static int kTotalNumberOfStartingPieces = 16;
	[SerializeField] protected Chess_GameOverOptions gameOverOptions = null;

	[SerializeField] protected ChessBoard chessBoard;
	public ChessPlayer opponentPlayer;

	[SerializeField] protected Image turnIndicatorBox;
	[SerializeField] protected Text turnIndicatorText;
	[SerializeField] protected GameObject InCheckWarning;

	public bool bMyTurn = true;
	public void SetTurn(bool bturn) { bMyTurn = bturn; }

	public PieceColour colour = PieceColour.White;

	protected int iNumberOfLivingPieces = kTotalNumberOfStartingPieces;
	protected bool bInCheck = false;

	protected MoveType eCurrentMoveType = MoveType.SelectAPiece;
	protected List<GridPosition> moveOptions = new List<GridPosition>();

	protected GridPosition selectedPiecePosition;	//Position to move piece to.

	//--------------------------------------------------------------------------------------------

	void Update()
    {
        if (chessBoard != null && chessBoard.boardLayout != null)
        {
			if (bMyTurn)
			{
				GameState gameState = PreTurn();
				if (gameState == GameState.Normal || gameState == GameState.Check)
				{
					Vector3 selectedPosition;
					if (CheckForInput(out selectedPosition))
					{
						GridPosition selectedGridPosition = chessBoard.SelectCell(selectedPosition);
						GetNumberOfLivingPieces();
						if (MakeAMove(selectedGridPosition))
						{
							bMyTurn = false;
							opponentPlayer.SetTurn(true);

							//Indicators to help the human player.
							ShowCheckWarning();
							SetTurnIndicator();
						}
					}
				}
				else if(gameState == GameState.Checkmate || gameState == GameState.Stalemate)
                {
					//We have an end to the game, so let the game over canvas show the player.
					if(gameOverOptions != null)
                    {
						PieceColour opponentColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
						gameOverOptions.SetWinner(gameState, opponentColour);
						gameOverOptions.ShowGameOverCanvas();
					}
				}
			}
        }
    }

    //--------------------------------------------------------------------------------------------

    public bool CheckForInput(out Vector3 inputPosition)
    {
        //Was Left mouse btton clicked?
        if (Input.GetMouseButtonDown(0))
        {
            inputPosition = Input.mousePosition;
            return true;
        }

        inputPosition = Vector3.zero;
        return false;
    }

	//--------------------------------------------------------------------------------------------


	public GameState PreTurn()
	{
		//Check whether this player is in CHECK, CHECKMATE or STALEMATE.
		bInCheck = CheckForCheck(chessBoard, colour);

		//Remove any highlight position as we have yet to select a piece.
		chessBoard.HideHighlight();

		//If we are in CHECK, can we actually make a move to get us out of it?
		if (bInCheck)
		{
			if (CheckForCheckmate(chessBoard, colour))
				return GameState.Checkmate;
			else
				return GameState.Check;
		}
		else
		{
			if (CheckForStalemate(chessBoard, colour))
				return GameState.Stalemate;
			else
				return GameState.Normal;
		}

		//Return normal if none of the above conditions have been met.
		return GameState.Normal;
	}

	//--------------------------------------------------------------------------------------------------

	public bool MakeAMove(GridPosition gridPosition)
	{
		//Ensure the position passed in is within the board dimensions.
		if (gridPosition.row <= ChessBoard.boardDimensions && gridPosition.column <= ChessBoard.boardDimensions)
		{
			if (eCurrentMoveType == MoveType.SelectAPiece)
			{
				//If the piece selected is of the same colour as the current player - Deal with it.
				if (chessBoard.boardLayout[gridPosition.row, gridPosition.column].pieceColour == colour)
				{
					//Valid position so store it and get possible moves.
					selectedPiecePosition.row = gridPosition.row;
					selectedPiecePosition.column = gridPosition.column;
					PieceDetails boardPiece = chessBoard.boardLayout[gridPosition.row, gridPosition.column];

					//Clear all highlighted move options before repopulating in GetMoveOptions() function.
					moveOptions.Clear();

					List<Move> moves = new List<Move>();
					GetMoveOptions(selectedPiecePosition, boardPiece, chessBoard, ref moves);

					//Is this a valid move?
					for (int currentMove = 0; currentMove < moves.Count; currentMove++)
					{
						ChessBoard boardToTest = new ChessBoard();
						boardToTest.CopyInternalBoard(chessBoard);
						boardToTest.MovePiece(moves[currentMove].from_Row, moves[currentMove].from_Col, moves[currentMove].to_Row, moves[currentMove].to_Col);

						if (!CheckForCheck(boardToTest, colour))
						{
							moveOptions.Add(new GridPosition(moves[currentMove].to_Row, moves[currentMove].to_Col));
						}
					}

					chessBoard.HighlightMoveOptions(moveOptions);

					//Change move type.
					eCurrentMoveType = MoveType.SelectAPosition;
				}
                else
                {
					//Clear highlights.
					chessBoard.ClearMoveOptions();

					//Clear move options.
					moveOptions.Clear();

				}
			}
			else if (eCurrentMoveType == MoveType.SelectAPosition)
			{
				//Check if this position is actually another piece of the current players colour.
				//If so, switch the turn type and recall this function.
				if (chessBoard.boardLayout[gridPosition.row, gridPosition.column].pieceColour == colour)
				{
					eCurrentMoveType = MoveType.SelectAPiece;
					MakeAMove(gridPosition);
				}
				else
				{
					bool validPosition = false;

					//Check if this is a position within the highlighted options.
					for (int i = 0; i < moveOptions.Count; i++)
					{
						GridPosition highlightPos = moveOptions[i];
						if (gridPosition.row == (int)highlightPos.row && gridPosition.column == (int)highlightPos.column)
						{
							validPosition = true;
							break;
						}
					}

					//If we selected a valid position from the highlighted options, then move the piece.
					if (validPosition == true)
					{
						//If this was an en'passant move the taken piece will not be in the square we moved to.
						if (chessBoard.boardLayout[selectedPiecePosition.row, selectedPiecePosition.column].pieceType == PieceType.Pawn)
						{
							//If the pawn is on its start position and it double jumps, then en'passant may be available for opponent.
							if ((selectedPiecePosition.row == 1 && gridPosition.row == 3) ||
								(selectedPiecePosition.row == 6 && gridPosition.row == 4))
							{
								chessBoard.boardLayout[selectedPiecePosition.row, selectedPiecePosition.column].bCanEnPassant = true;
							}
						}

						//En'Passant removal of enemy pawn.
						//If our pawn moved into an empty position to the left or right, then must be En'Passant.
						if (chessBoard.boardLayout[selectedPiecePosition.row, selectedPiecePosition.column].pieceType == PieceType.Pawn &&
							chessBoard.boardLayout[gridPosition.row, gridPosition.column].pieceType == PieceType.None)
						{
							int pawnDirectionOpposite = colour == PieceColour.White ? -1 : 1;

							//If we end up on a different column, but there was no piece in the target cell - Must be En'Passant.
							if (gridPosition.column != selectedPiecePosition.column)
							{
								chessBoard.KillPiece(gridPosition.row + pawnDirectionOpposite, gridPosition.column);
							}
						}

						//CASTLING - Move the rook.
						if (chessBoard.boardLayout[selectedPiecePosition.row, selectedPiecePosition.column].pieceType == PieceType.King)
						{
							//Are we moving 2 spaces??? This indicates CASTLING.
							if (gridPosition.column - selectedPiecePosition.column == 2)
							{
								//Moving 2 spaces to the right - Move the ROOK on the right into its new position.
								chessBoard.MovePiece(selectedPiecePosition.row, selectedPiecePosition.column + 4, selectedPiecePosition.row, selectedPiecePosition.column + 1);
							}
							else if (gridPosition.column - selectedPiecePosition.column == -2)
							{
								//Moving 2 spaces to the left - Move the ROOK on the left into its new position.
								chessBoard.MovePiece(selectedPiecePosition.row, selectedPiecePosition.column - 3, selectedPiecePosition.row, selectedPiecePosition.column - 1);
							}
						}

						//Kill any piece in this new position.
						chessBoard.KillPiece(gridPosition.row, gridPosition.column);

						//Move the piece into new position.
						chessBoard.MovePiece(selectedPiecePosition.row, selectedPiecePosition.column, gridPosition.row, gridPosition.column);

						//Check if we need to promote a pawn.
						if (chessBoard.boardLayout[gridPosition.row, gridPosition.column].pieceType == PieceType.Pawn &&
							(gridPosition.row == 0 || gridPosition.row == 7))
						{
							//Time to promote.
							eCurrentMoveType = MoveType.PawnPromotion;
						}
						else
						{
							//Clear highlights.
							chessBoard.ClearMoveOptions();

							//Clear move options.
							moveOptions.Clear();

							//Turn finished.
							return true;
						}

					}
					else
					{
						eCurrentMoveType = MoveType.SelectAPiece;

						//Clear highlights.
						chessBoard.ClearMoveOptions();

						//Clear move options.
						moveOptions.Clear();
					}
				}
			}
			else if (eCurrentMoveType == MoveType.PawnPromotion)
			{
				//Change the PAWN into the selected piece - Queen for this example
				chessBoard.SwapPieceType(selectedPiecePosition.row, selectedPiecePosition.column, PieceType.Queen);

				//Turn finished.
				return true;
			}
		}

		//Not finished turn yet.
		return false;
	}

	//--------------------------------------------------------------------------------------------------

	public void GetNumberOfLivingPieces()
	{
		iNumberOfLivingPieces = 0;

		for (int iRow = 0; iRow < ChessBoard.boardDimensions; iRow++)
		{
			for (int iCol = 0; iCol < ChessBoard.boardDimensions; iCol++)
			{
				//Check for pieces.
				PieceDetails currentPiece = chessBoard.boardLayout[iRow, iCol];
				if (currentPiece.pieceColour == colour && currentPiece.pieceType != PieceType.None)
				{
					iNumberOfLivingPieces++;
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	protected void GetAllMoveOptions(ChessBoard boardToTest, PieceColour teamColour, ref List<Move> moves)
	{
		int numberOfPiecesFound = 0;

		//Remove any previously stored move options.
		moves.Clear();

		//Go through the board and get the moves for all pieces of our colour.
		for (int iRow = 0; iRow < ChessBoard.boardDimensions; iRow++)
		{
			for (int iCol = 0; iCol < ChessBoard.boardDimensions; iCol++)
			{
				//Check for pieces.
				PieceDetails currentPiece = boardToTest.boardLayout[iRow, iCol];
				if (currentPiece.pieceColour == teamColour && currentPiece.pieceType != PieceType.None)
				{
					numberOfPiecesFound++;

					switch (currentPiece.pieceType)
					{
						case PieceType.Pawn:
							GetPawnMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.Knight:
							GetKnightMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.Bishop:
							GetDiagonalMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.Rook:
							GetHorizontalAndVerticalMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.Queen:
							GetHorizontalAndVerticalMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							GetDiagonalMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.King:
							GetKingMoveOptions(new GridPosition(iRow, iCol), currentPiece, boardToTest, ref moves);
							break;

						case PieceType.None:
							break;

						default:
							break;
					}

					//Early exit - No point searching when we have already found all our pieces.
					if (numberOfPiecesFound == iNumberOfLivingPieces)
					{
						return;
					}
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	void GetMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		//All pieces move differently.
		switch (boardPiece.pieceType)
		{
			case PieceType.Pawn:
				GetPawnMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.Knight:
				GetKnightMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.Bishop:
				GetDiagonalMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.Rook:
				GetHorizontalAndVerticalMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.Queen:
				GetHorizontalAndVerticalMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				GetDiagonalMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.King:
				GetKingMoveOptions(gridPosition, boardPiece, boardToTest, ref moves);
				break;

			case PieceType.None:
				break;

			default:
				break;
		}
	}

	//--------------------------------------------------------------------------------------------------

	bool CheckMoveOptionValidityAndStoreMove(Move moveToCheck, PieceColour pieceColour, ChessBoard boardToTest, ref List<Move> moves)
	{
		ChessBoard tempBoard = new ChessBoard();
		tempBoard.CopyInternalBoard(boardToTest);

		if (moveToCheck.to_Col >= 0 && moveToCheck.to_Col < ChessBoard.boardDimensions && moveToCheck.to_Row >= 0 && moveToCheck.to_Row < ChessBoard.boardDimensions)
		{
			//We check with colour passed in to enable the same functions to construct attacked spaces
			//as well as constructing the positions we can move to.
			if (tempBoard.boardLayout[moveToCheck.to_Row, moveToCheck.to_Col].pieceType == PieceType.None)
			{
				//Will this leave us in check?
				tempBoard.MovePiece(moveToCheck.from_Row, moveToCheck.from_Col, moveToCheck.to_Row, moveToCheck.to_Col);

				if (!CheckForCheck(tempBoard, pieceColour))
				{
					moves.Add(moveToCheck);
				}

			}
			else
			{
				//A piece so no more moves after this, but can we take it?
				if (tempBoard.boardLayout[moveToCheck.to_Row, moveToCheck.to_Col].pieceColour != pieceColour)
				{
					//Will this leave us in check?
					tempBoard.MovePiece(moveToCheck.from_Row, moveToCheck.from_Col, moveToCheck.to_Row, moveToCheck.to_Col);

					if (!CheckForCheck(tempBoard, pieceColour))
					{
						moves.Add(moveToCheck);
					}
				}

				//Hit a piece, so no more moves in this direction.
				return false;
			}

			return true;
		}

		return false;
	}

	//--------------------------------------------------------------------------------------------------

	void ClearEnPassant()
	{
		for (int col = 0; col < ChessBoard.boardDimensions; col++)
		{
			for (int row = 0; row < ChessBoard.boardDimensions; row++)
			{
				//Clear opponents en'Passant, not ours. Ours needs to be available for the opponents turn.
				if (chessBoard.boardLayout[row, col].pieceType == PieceType.Pawn && chessBoard.boardLayout[row,col].pieceColour != colour)
				{
					chessBoard.boardLayout[row, col].bCanEnPassant = false;
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	void GetPawnMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		ChessBoard tempBoard = new ChessBoard();
		tempBoard.CopyInternalBoard(boardToTest);
		int pawnDirection = boardPiece.pieceColour == PieceColour.White ? 1 : -1;

		//Single step FORWARD.

		int col = gridPosition.column;
		int row = gridPosition.row + pawnDirection;
		if (row >= 0 && row < ChessBoard.boardDimensions && tempBoard.boardLayout[row,col].pieceType == PieceType.None)
		{
			//Will this leave us in check? Only interested in check if its one of our moves.
			tempBoard.MovePiece(gridPosition.row, gridPosition.column, row, col);

			if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
			{
				moves.Add(new Move(gridPosition.row, gridPosition.column, row, col));
			}
		}

		//Double step FORWARD.
		if (gridPosition.row == 1 || gridPosition.row == 6)
		{
			//Reset the board.
			tempBoard.CopyInternalBoard(boardToTest);

			int row2 = gridPosition.row + pawnDirection * 2;
			if (row2 >= 0 && row2 < ChessBoard.boardDimensions && tempBoard.boardLayout[row2, col].pieceType == PieceType.None && tempBoard.boardLayout[row2, col].pieceType == PieceType.None)
			{
				//Will this leave us in check? Only interested in check if its one of our moves.
				tempBoard.MovePiece(gridPosition.row, gridPosition.column, row2, col);

				if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
				{
					moves.Add(new Move(gridPosition.row, gridPosition.column, row2, col));
				}
			}
		}

		//En'Passant move.
		if ((gridPosition.row == 4 && boardPiece.pieceColour == PieceColour.White) ||
			(gridPosition.row == 3 && boardPiece.pieceColour == PieceColour.Black))
		{
			//Reset the board.
			tempBoard.CopyInternalBoard(boardToTest);

			//Pawn beside us, can we en'passant.
			col = gridPosition.column - 1;
			row = gridPosition.row;
			if (col >= 0)
			{
				PieceDetails leftPiece = tempBoard.boardLayout[row, col];
				if (leftPiece.pieceType == PieceType.Pawn && leftPiece.bCanEnPassant == true)
				{
					//Will this leave us in check? Only interested in check if its one of our moves.
					tempBoard.KillPiece(row, col);
					tempBoard.MovePiece(gridPosition.row, gridPosition.column, row + pawnDirection, col);

					if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
					{
						moves.Add(new Move(gridPosition.row, gridPosition.column, row + pawnDirection, col));
					}
				}
			}

			//Reset the board.
			tempBoard.CopyInternalBoard(boardToTest);

			col = gridPosition.column + 1;
			if (col < ChessBoard.boardDimensions)
			{
				PieceDetails rightPiece = tempBoard.boardLayout[row, col];
				if (rightPiece.pieceType == PieceType.Pawn && rightPiece.bCanEnPassant == true)
				{
					//Will this leave us in check? Only interested in check if its one of our moves.
					tempBoard.KillPiece(row, col);
					tempBoard.MovePiece(gridPosition.row, gridPosition.column, row + pawnDirection, col);

					if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
					{
						moves.Add(new Move(gridPosition.row, gridPosition.column, row + pawnDirection, col));
					}
				}
			}
		}

		//Take a piece move.
		if (gridPosition.row > 0 && gridPosition.row < ChessBoard.boardDimensions - 1)
		{
			//Ahead of selected pawn to the LEFT.
			if (gridPosition.column > 0)
			{
				//Reset the board.
				tempBoard.CopyInternalBoard(boardToTest);

				col = gridPosition.column - 1;
				row = gridPosition.row + pawnDirection;
				PieceDetails aheadLeftPiece = tempBoard.boardLayout[row, col];
				if (aheadLeftPiece.pieceType != PieceType.None && aheadLeftPiece.pieceColour != boardPiece.pieceColour)
				{
					//Will this leave us in check? Only interested in check if its one of our moves.
					tempBoard.MovePiece(gridPosition.row, gridPosition.column, row, col);

					if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
					{
						moves.Add(new Move(gridPosition.row, gridPosition.column, row, col));
					}
				}
			}

			//Ahead of selected pawn to the RIGHT.
			if (gridPosition.column < ChessBoard.boardDimensions - 1)
			{
				//Reset the board.
				tempBoard.CopyInternalBoard(boardToTest);

				col = gridPosition.column + 1;
				row = gridPosition.row + pawnDirection;
				if (col >= 0 && col < ChessBoard.boardDimensions && row >= 0 && row < ChessBoard.boardDimensions)
				{
					PieceDetails aheadRightPiece = tempBoard.boardLayout[row, col];
					if (aheadRightPiece.pieceType != PieceType.None && aheadRightPiece.pieceColour != boardPiece.pieceColour)
					{
						//Will this leave us in check? Only interested in check if its one of our moves.
						tempBoard.MovePiece(gridPosition.row, gridPosition.column, row, col);

						if (!CheckForCheck(tempBoard, boardPiece.pieceColour))
						{
							moves.Add(new Move(gridPosition.row, gridPosition.column, row, col));
						}
					}
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	protected void GetHorizontalAndVerticalMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		Move move;

		//Vertical DOWN the board.
		for (int row = gridPosition.row + 1; row < ChessBoard.boardDimensions; row++)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, gridPosition.column);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//Vertical UP the board.
		for (int row = gridPosition.row - 1; row >= 0; row--)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, gridPosition.column);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//Horizontal LEFT of the board.
		for (int col = gridPosition.column - 1; col >= 0; col--)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, gridPosition.row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//Horizontal RIGHT of the board.
		for (int col = gridPosition.column + 1; col < ChessBoard.boardDimensions; col++)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, gridPosition.row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	protected void GetDiagonalMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		Move move;

		//ABOVE & LEFT
		for (int row = gridPosition.row - 1, col = gridPosition.column - 1; row >= 0 && col >= 0; row--, col--)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//ABOVE & RIGHT
		for (int row = gridPosition.row - 1, col = gridPosition.column + 1; row >= 0 && col < ChessBoard.boardDimensions; row--, col++)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//BELOW & LEFT
		for (int row = gridPosition.row + 1, col = gridPosition.column - 1; row < ChessBoard.boardDimensions && col >= 0; row++, col--)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}

		//BELOW & RIGHT
		for (int row = gridPosition.row + 1, col = gridPosition.column + 1; row < ChessBoard.boardDimensions && col < ChessBoard.boardDimensions; row++, col++)
		{
			//Keep checking moves until one is invalid.
			move = new Move(gridPosition.row, gridPosition.column, row, col);
			if (CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves) == false)
			{
				break;
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	protected void GetKnightMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		Move move;

		//Moves to the RIGHT.
		move = new Move(gridPosition.row, gridPosition.column, gridPosition.row+1, gridPosition.column+2);
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		move.to_Row = gridPosition.row - 1;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		//Moves to the LEFT.
		move.to_Col = gridPosition.column - 2;
		move.to_Row = gridPosition.row + 1;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		move.to_Row = gridPosition.row - 1;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		//Moves ABOVE.
		move.to_Col = gridPosition.column + 1;
		move.to_Row = gridPosition.row - 2;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		move.to_Col = gridPosition.column - 1;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		//Moves BELOW.
		move.to_Col = gridPosition.column + 1;
		move.to_Row = gridPosition.row + 2;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);

		move.to_Col = gridPosition.column - 1;
		CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);
	}

	//--------------------------------------------------------------------------------------------------

	protected void GetKingMoveOptions(GridPosition gridPosition, PieceDetails boardPiece, ChessBoard boardToTest, ref List<Move> moves)
	{
		Move move;

		//Start at position top left of king and move across and down.
		for (int row = gridPosition.row - 1; row <= gridPosition.row + 1; row++)
		{
			for (int col = gridPosition.column - 1; col <= gridPosition.column + 1; col++)
			{
				if ((row >= 0 && row < ChessBoard.boardDimensions) && (col >= 0 && col < ChessBoard.boardDimensions))
				{
					//Check if move is valid and store it. We dont care about the return value as we are only
					// checking one move in each direction.
					move = new Move(gridPosition.row, gridPosition.column, row, col);
					CheckMoveOptionValidityAndStoreMove(move, boardPiece.pieceColour, boardToTest, ref moves);
				}
			}
		}

		if (boardPiece.pieceColour == colour)
		{
			PieceColour opponentColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

			//Compile all the moves available to our opponent.
			List<Move> allOpponentMoves = new List<Move>();
			GetAllMoveOptions(boardToTest, opponentColour, ref allOpponentMoves);

			//Can CASTLE if not in CHECK.
			if (!bInCheck)
			{
				//CASTLE to the right.
				PieceDetails king = boardToTest.boardLayout[gridPosition.row, gridPosition.column];
				if (!king.bHasMoved)
				{
					if (gridPosition.column + 4 < ChessBoard.boardDimensions)
					{
						PieceDetails rightRook = boardToTest.boardLayout[gridPosition.row, gridPosition.column + 4];

						if (rightRook.pieceType == PieceType.Rook && !rightRook.bHasMoved)
						{
							if (boardToTest.boardLayout[gridPosition.row, gridPosition.column + 1].pieceType == PieceType.None &&
								boardToTest.boardLayout[gridPosition.row, gridPosition.column + 2].pieceType == PieceType.None &&
								boardToTest.boardLayout[gridPosition.row, gridPosition.column + 3].pieceType == PieceType.None)
							{
								//Cannot CASTLE through a CHECK position.
								bool canCastle = true;
								for (int i = 0; i < allOpponentMoves.Count; i++)
								{
									if ((allOpponentMoves[i].to_Col == gridPosition.column + 2 && allOpponentMoves[i].to_Row == gridPosition.row) ||
										(allOpponentMoves[i].to_Col == gridPosition.column + 1 && allOpponentMoves[i].to_Row == gridPosition.row))
									{
										canCastle = false;
									}
								}

								//Check if the final position is valid.
								if (canCastle)
								{
									CheckMoveOptionValidityAndStoreMove(new Move(gridPosition.row, gridPosition.column, gridPosition.row, gridPosition.column + 2), boardPiece.pieceColour, boardToTest, ref moves);
								}
							}
						}
					}

					//CASTLE to the left.
					PieceDetails leftRook = boardToTest.boardLayout[gridPosition.row, gridPosition.column - 3];

					if (leftRook.pieceType == PieceType.Rook && !leftRook.bHasMoved)
					{
						if (gridPosition.column - 3 >= 0)
						{
							if (boardToTest.boardLayout[gridPosition.row, gridPosition.column - 1].pieceType == PieceType.None &&
							boardToTest.boardLayout[gridPosition.row, gridPosition.column - 2].pieceType == PieceType.None)
							{
								//Cannot CASTLE through a CHECK position.
								bool canCastle = true;
								for (int i = 0; i < allOpponentMoves.Count; i++)
								{
									if ((allOpponentMoves[i].to_Col == gridPosition.column - 1 && allOpponentMoves[i].to_Row == gridPosition.row) ||
										(allOpponentMoves[i].to_Col == gridPosition.column - 2 && allOpponentMoves[i].to_Row == gridPosition.row) ||
										(allOpponentMoves[i].to_Col == gridPosition.column - 3 && allOpponentMoves[i].to_Row == gridPosition.row))
									{
										canCastle = false;
									}
								}

								//Check if the final position is valid.
								if (canCastle)
								{
									CheckMoveOptionValidityAndStoreMove(new Move(gridPosition.row, gridPosition.column, gridPosition.row, gridPosition.column - 2), boardPiece.pieceColour, boardToTest, ref moves);
								}
							}
						}
					}
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	protected bool CheckForCheck(ChessBoard boardToTest, PieceColour teamColour)
	{
		GridPosition ourKingPosition = new GridPosition();
		//COLOUR opponentColour = teamColour == COLOUR_WHITE ? COLOUR_BLACK : COLOUR_WHITE;

		//Go through the board and find our KING's position.
		for (int iRow = 0; iRow < ChessBoard.boardDimensions; iRow++)
		{
			for (int iCol = 0; iCol < ChessBoard.boardDimensions; iCol++)
			{
				PieceDetails currentPiece = boardToTest.boardLayout[iRow, iCol];
				if (currentPiece.pieceColour == teamColour && currentPiece.pieceType == PieceType.King)
				{
					//Store our KING's position whilst we go through the board.
					ourKingPosition.row = iRow;
					ourKingPosition.column = iCol;

					//Force double loop exit.
					iRow = ChessBoard.boardDimensions;
					iCol = ChessBoard.boardDimensions;
				}
			}
		}

		//Now we have our kings position lets check if it is under attack from anywhere.
		//Horizontal - Right
		int col = ourKingPosition.column;
		while (++col < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[ourKingPosition.row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Rook)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Horizontal - Left
		col = ourKingPosition.column;
		while (--col >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[ourKingPosition.row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Rook)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Veritcal - Up
		int row = ourKingPosition.row;
		while (--row >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, ourKingPosition.column];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Rook)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Veritcal - Down
		row = ourKingPosition.row;
		while (++row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, ourKingPosition.column];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Rook)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Diagonal - Right Down
		row = ourKingPosition.row;
		col = ourKingPosition.column;
		while (++col < ChessBoard.boardDimensions && ++row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Bishop)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Diagonal - Right Up
		row = ourKingPosition.row;
		col = ourKingPosition.column;
		while (--row >= 0 && ++col < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Bishop)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Diagonal - Left Down
		row = ourKingPosition.row;
		col = ourKingPosition.column;
		while (++row < ChessBoard.boardDimensions && --col >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Bishop)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Diagonal - Left Up
		row = ourKingPosition.row;
		col = ourKingPosition.column;
		while (--row >= 0 && --col >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceType != PieceType.None)
			{
				if (currentPiece.pieceColour == teamColour)
				{
					break;
				}
				else if (currentPiece.pieceType == PieceType.Queen || currentPiece.pieceType == PieceType.Bishop)
				{
					return true;
				}
				else
				{
					break;
				}
			}
		}

		//Awkward Knight moves
		row = ourKingPosition.row + 1;
		col = ourKingPosition.column + 2;
		if (col < ChessBoard.boardDimensions && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row - 1;
		col = ourKingPosition.column + 2;
		if (col < ChessBoard.boardDimensions && row >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row + 2;
		col = ourKingPosition.column + 1;
		if (col < ChessBoard.boardDimensions && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row + 2;
		col = ourKingPosition.column - 1;
		if (col >= 0 && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row + 1;
		col = ourKingPosition.column - 2;
		if (col >= 0 && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row - 1;
		col = ourKingPosition.column - 2;
		if (col >= 0 && row >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row - 2;
		col = ourKingPosition.column - 1;
		if (col >= 0 && row >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		row = ourKingPosition.row - 2;
		col = ourKingPosition.column + 1;
		if (col < ChessBoard.boardDimensions && row >= 0)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Knight)
			{
				return true;
			}
		}

		//Opponent King positions
		for (row = (int)ourKingPosition.row - 1; row < (int)ourKingPosition.row + 2; row++)
		{
			for (col = (int)ourKingPosition.column - 1; col < (int)ourKingPosition.column + 2; col++)
			{
				if ((col >= 0 && col < ChessBoard.boardDimensions) && (row >= 0 && row < ChessBoard.boardDimensions))
				{
					PieceDetails currentPiece = boardToTest.boardLayout[row, col];
					//Must be the opponents king, as w will pass over our own in this embedded loop.
					if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.King)
					{
						return true;
					}
				}
			}
		}

		//Opponent Pawns
		int opponentPawnDirection = teamColour == PieceColour.White ? 1 : -1;
		row = ourKingPosition.row + opponentPawnDirection;
		col = ourKingPosition.column + 1;
		if (col < ChessBoard.boardDimensions && row >= 0 && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row, col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Pawn)
			{
				return true;
			}
		}

		row = ourKingPosition.row + opponentPawnDirection;
		col = ourKingPosition.column - 1;
		if (col >= 0 && row >= 0 && row < ChessBoard.boardDimensions)
		{
			PieceDetails currentPiece = boardToTest.boardLayout[row,col];
			if (currentPiece.pieceColour != teamColour && currentPiece.pieceType == PieceType.Pawn)
			{
				return true;
			}
		}

		return false;
	}

	//--------------------------------------------------------------------------------------------------

	protected bool CheckForCheckmate(ChessBoard boardToCheck, PieceColour teamColour)
	{
		//If we are in CHECK, can we actually make a move to get us out of it?
		if (bInCheck)
		{
			List<Move> moves = new List<Move>();
			GetAllMoveOptions(boardToCheck, teamColour, ref moves);

			//If there are no valid moves then this unfortunately is CHECKMATE.
			if (moves.Count == 0)
			{
				return true;
			}
		}

		return false;
	}

	//--------------------------------------------------------------------------------------------------

	protected bool CheckForStalemate(ChessBoard boardToCheck, PieceColour teamColour)
	{
		//If we are not in CHECK, can we actually make a move? If not then we are in a STALEMATE.
		if (!bInCheck)
		{
			List<Move> moves = new List<Move>();
			GetAllMoveOptions(boardToCheck, teamColour, ref moves);

			//If there are no valid moves then this unfortunately is CHECKMATE.
			if (moves.Count == 0)
			{
				return true;
			}
		}

		return false;
	}

	//--------------------------------------------------------------------------------------------------

	void EndTurn()
	{
		//Change move type.
		eCurrentMoveType = MoveType.SelectAPiece;

		//Remove highlights.
		moveOptions.Clear();

		//Make all current player's pawns unavailable for en'passant. Opponent had their chance.
		ClearEnPassant();
	}

	//--------------------------------------------------------------------------------------------------
	
	protected void SetTurnIndicator()
    {
		if (turnIndicatorBox != null && turnIndicatorText != null)
		{
			if (bMyTurn)
			{
				//Use own colours if our turn.
				if (colour == PieceColour.White)
				{
					turnIndicatorBox.color = Color.white;
					turnIndicatorText.color = Color.black;
					turnIndicatorText.text = "WHITE\nTurn";
				}
				else
				{
					turnIndicatorBox.color = Color.black;
					turnIndicatorText.color = Color.white;
					turnIndicatorText.text = "BLACK\nTurn";
				}
			}
			else
            {
				//Use opposite colours for opponenets turn.
				if (colour == PieceColour.White)
				{
					turnIndicatorBox.color = Color.black;
					turnIndicatorText.color = Color.white;
					turnIndicatorText.text = "BLACK\nTurn"; 
				}
				else
				{
					turnIndicatorBox.color = Color.white;
					turnIndicatorText.color = Color.black;
					turnIndicatorText.text = "WHITE\nTurn";
				}
			}
		}
    }
	
	//--------------------------------------------------------------------------------------------------

	protected void ShowCheckWarning()
    {
		if (InCheckWarning != null)
		{
			if (bMyTurn)
			{
				if (bInCheck)
				{
					//Show CHECK text.
					InCheckWarning.SetActive(true);
				}
				else
				{
					//Remove CHECK text.
					InCheckWarning.SetActive(false);
				}
			}
			else
            {
				if (opponentPlayer.bInCheck)
				{
					//Show CHECK text.
					InCheckWarning.SetActive(true);
				}
				else
				{
					//Remove CHECK text.
					InCheckWarning.SetActive(false);
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

}
*/