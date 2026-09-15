using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayerAI : ChessPlayer
{
	//--------------------------------------------------------------------------------------------------
	private const int MaxInt = 99999;

	private const int kPawnScore        = 2;
    private const int kKnightScore      = 10;
    private const int kBishopScore      = 10;
    private const int kRookScore        = 25;
    private const int kQueenScore       = 50;
    private const int kKingScore        = 100;

    private const int kCheckScore       = 20;
    private const int kCheckmateScore   = 1000;
    private const int kStalemateScore   = 25; //Tricky one because sometimes you want this, sometimes you don't.

    private const int kPieceWeight      = 4; //Scores as above.
    private const int kMoveWeight       = 2; //Number of moves available to pieces.
    private const int kPositionalWeight = 1; //Whether in CHECK, CHECKMATE or STALEMATE.

	//--------------------------------------------------------------------------------------------------

	public int searchDepth = 3;

	ChessOpenings chessOpenings = null;

    //--------------------------------------------------------------------------------------------------

    private void Start()
    {
		chessOpenings = GetComponent<ChessOpenings>();
    }

    //--------------------------------------------------------------------------------------------------

    void Update()
	{
		if (chessBoard != null && chessBoard.boardLayout != null)
		{
			if (bMyTurn)
			{
				GameState gameState = PreTurn();
				if (gameState == GameState.Normal || gameState == GameState.Check)
				{
					TakeATurn();

					//Indicators to help the human player.
					ShowCheckWarning();
					SetTurnIndicator();
				}
				else if (gameState == GameState.Checkmate || gameState == GameState.Stalemate)
				{
					//We have an end to the game, so let the game over canvas show the player.
					if (gameOverOptions != null)
					{
						PieceColour opponentColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
						gameOverOptions.SetWinner(gameState, opponentColour);
						gameOverOptions.ShowGameOverCanvas();
					}
				}
			}
		}
	}

	//--------------------------------------------------------------------------------------------------

	void MakeAMove(Move move)
	{
		//If this was an en'passant move the taken piece will not be in the square we moved to.
		if (chessBoard.boardLayout[move.from_Row, move.from_Col].pieceType == PieceType.Pawn)
		{
			//If the pawn is on its start position and it double jumps, then en'passant may be available for opponent.
			if ((move.from_Row == 1 && move.to_Row == 3) ||
				(move.from_Row == 6 && move.to_Row == 4))
			{
				chessBoard.boardLayout[move.from_Row, move.from_Col].bCanEnPassant = true;
			}
		}

		//En'Passant removal of enemy pawn.
		//If our pawn moved into an empty position to the left or right, then must be En'Passant.
		if (chessBoard.boardLayout[move.from_Row, move.from_Col].pieceType == PieceType.Pawn &&
			chessBoard.boardLayout[move.to_Row, move.to_Col].pieceType == PieceType.None)
		{
			int pawnDirectionOpposite = colour == PieceColour.White ? -1 : 1;

			if ((move.from_Col < move.to_Col) ||
				(move.from_Col > move.to_Col))
			{
				chessBoard.KillPiece(move.to_Row + pawnDirectionOpposite, move.to_Col);
			}
		}

		//CASTLING - Move the rook.
		if (chessBoard.boardLayout[move.from_Row, move.from_Col].pieceType == PieceType.King)
		{
			//Are we moving 2 spaces??? This indicates CASTLING.
			if (move.to_Col - move.from_Col == 2)
			{
				//Moving 2 spaces to the right - Move the ROOK on the right into its new position.
				chessBoard.MovePiece(move.from_Row, move.from_Col + 4, move.from_Row, move.from_Col + 1);
			}
			else if (move.to_Col - move.from_Col == -2)
			{
				//Moving 2 spaces to the left - Move the ROOK on the left into its new position.
				chessBoard.MovePiece(move.from_Row, move.from_Col - 3, move.from_Row, move.from_Col - 1);
			}
		}

		//Kill any piece in this new position.
		chessBoard.KillPiece(move.to_Row, move.to_Col);

		//Move the piece into new position.
		chessBoard.MovePiece(move.from_Row, move.from_Col, move.to_Row, move.to_Col);


		//Check if we need to promote a pawn.
		if (chessBoard.boardLayout[move.to_Row, move.to_Col].pieceType == PieceType.Pawn && (move.to_Row == 0 || move.to_Row == 7))
		{
			chessBoard.SwapPieceType(move.to_Row, move.to_Col, PieceType.Queen);
		}
	}

	//--------------------------------------------------------------------------------------------------

	void TakeATurn()
	{
		//Play opening book, or calculate best move.
		int score = 0;
		Move bestMove = new Move();
		if (chessOpenings != null && chessOpenings.IsAnotherMoveAvailable())
		{
			//Play opening.
			bestMove = chessOpenings.GetNextMove();
		}
		else
		{
			//Do the MiniMax Algorithm.
			GetNumberOfLivingPieces();
			score = MiniMax(chessBoard, ref bestMove);
		}

		//No we have the best move, do it.
		MakeAMove(bestMove);

		//Allow opponent to take their turn.
		bMyTurn = false;
		opponentPlayer.SetTurn(true);
	}

	//--------------------------------------------------------------------------------------------------

	int MiniMax(ChessBoard boardToTest, ref Move bestMove)
	{
		return Maximise(boardToTest, searchDepth, ref bestMove, MaxInt);
	}

	//--------------------------------------------------------------------------------------------------

	int Maximise(ChessBoard boardToTest, int currentSearchDepth, ref Move bestMove, int parentLow)
	{
		//Todo: Add code here.
		return 0;
	}

	//--------------------------------------------------------------------------------------------------

	int Minimise(ChessBoard boardToTest, int currentSearchDepth, ref Move bestMove, int parentHigh)
	{
		//Todo: Add code here.
		return 0;
	}

	//--------------------------------------------------------------------------------------------------

	int ScoreTheBoard(ChessBoard boardToScore)
	{
		int pieceScore = 0;
		int moveScore = 0;
		int positionalScore = 0;
		List<Move> availableMoves = new List<Move>();

		//------------------------------------------------------------------------------------------------------------------------
		//Individual piece scoring
		//------------------------------------------------------------------------------------------------------------------------
		for (int iCol = 0; iCol < ChessBoard.boardDimensions; iCol++)
		{
			for (int iRow = 0; iRow < ChessBoard.boardDimensions; iRow++)
			{
				PieceDetails currentPiece = boardToScore.boardLayout[iRow, iCol];

				//All pieces score differently.
				GridPosition gridPosition = new GridPosition(iRow, iCol);

				switch (currentPiece.pieceType)
				{
					case PieceType.Pawn:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kPawnScore;

							//Extra points for getting close to promotion.
							if (colour == PieceColour.White && gridPosition.row >= 4)
							{
								moveScore += 8 - (8 - gridPosition.row);
							}
							else if (colour == PieceColour.Black && gridPosition.row <= 3)
							{
								moveScore += 8 - gridPosition.row;
							}
						}
						else
						{
							pieceScore -= kPawnScore;
						}
						break;

					case PieceType.Knight:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kKnightScore;

							//Extra points for the more move options available.
							GetKnightMoveOptions(gridPosition, currentPiece, boardToScore, ref availableMoves);
							moveScore += availableMoves.Count;
						}
						else
							pieceScore -= kKnightScore;
						break;

					case PieceType.Bishop:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kBishopScore;

							//Extra points for the more move options available.
							GetDiagonalMoveOptions(gridPosition, currentPiece, boardToScore, ref availableMoves);
							moveScore += availableMoves.Count;
						}
						else
							pieceScore -= kBishopScore;
						break;

					case PieceType.Rook:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kRookScore;

							//Extra points for the more move options available.
							GetHorizontalAndVerticalMoveOptions(gridPosition, currentPiece, boardToScore, ref availableMoves);
							moveScore += availableMoves.Count;
						}
						else
							pieceScore -= kRookScore;
						break;

					case PieceType.Queen:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kQueenScore;

							//Extra points for the more move options available.
							GetDiagonalMoveOptions(gridPosition, currentPiece, boardToScore, ref availableMoves );
							moveScore += availableMoves.Count;
							GetHorizontalAndVerticalMoveOptions(gridPosition, currentPiece, boardToScore, ref availableMoves);
							moveScore += availableMoves.Count;
						}
						else
							pieceScore -= kQueenScore;
						break;

					case PieceType.King:
						if (currentPiece.pieceColour == colour)
						{
							pieceScore += kKingScore;
						}
						else
						{
							pieceScore -= kKingScore;
						}
						break;

					case PieceType.None:
						break;

					default:
						break;
				}
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//CHECK, CHECKMATE & STALEMATE checks
		//------------------------------------------------------------------------------------------------------------------------

		//Lets see if we are putting the opponent in check.
		bool opponentInCheck = false;
		PieceColour opponentColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
		if (CheckForCheck(boardToScore, opponentColour))
		{
			opponentInCheck = true;
			positionalScore += kCheckScore;

			//If they are in CHECK they may actually be in CHECKMATE.
			if (CheckForCheckmate(boardToScore, opponentColour))
			{
				positionalScore += kCheckmateScore;
			}
		}

		//Can't both be in CHECK, so don't bother checking self if opponent is.
		if (!opponentInCheck)
		{
			//Are we in CHECK?
			if (CheckForCheck(boardToScore, colour))
			{
				positionalScore -= kCheckScore;

				//If we are in CHECK we could also be in CHECKMATE.
				if (CheckForCheckmate(boardToScore, colour))
				{
					positionalScore -= kCheckmateScore;
				}
			}
			else
			{
				//If we are in not in CHECK are we in STALEMATE?
				if (CheckForStalemate(boardToScore, colour))
				{
					positionalScore -= kStalemateScore; //NEVER want a STALEMATE!!!
				}
			}
		}

		//Return the overall score for this board.
		return (pieceScore * kPieceWeight) + (moveScore * kMoveWeight) + (positionalScore * kPositionalWeight);
	}

	//--------------------------------------------------------------------------------------------------
}
