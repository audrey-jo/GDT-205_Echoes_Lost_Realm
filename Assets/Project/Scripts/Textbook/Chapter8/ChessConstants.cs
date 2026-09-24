/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------------------

public enum PlayerControl
{
    Human,
    Ai
};

//--------------------------------------------------------------------------------------------------

public enum MoveType
{
	SelectAPiece = 0,
	SelectAPosition = 1,
	PawnPromotion
};

//--------------------------------------------------------------------------------------------------

public enum PieceType
{
	Pawn,
	Knight,
	Bishop,
	Rook,
	Queen,
	King,

	None
};

//--------------------------------------------------------------------------------------------------

public enum PieceColour
{
	White,
	Black,

	None
};

//--------------------------------------------------------------------------------------------------

public enum GameState
{
    Normal = 0, 
	Check = 2, 
	Checkmate = 3,
	Stalemate = 4
}

//--------------------------------------------------------------------------------------------------

public enum TurnState
{
	Pre,
	Play,
	Post
}

//--------------------------------------------------------------------------------------------------

public struct PieceDetails
{
	public PieceType pieceType;
	public PieceColour pieceColour;
	public bool bCanEnPassant;
	public bool bHasMoved;

	public PieceDetails(PieceType type, PieceColour colour)
	{
		pieceType = type;
		pieceColour = colour;
		bCanEnPassant = false;
		bHasMoved = false;
	}

	public void Clear()
    {
		pieceType = PieceType.None;
		pieceColour = PieceColour.None;
		bCanEnPassant = false;
		bHasMoved = false;
	}
};

//--------------------------------------------------------------------------------------------------

public struct GridPosition
{
	public int row;
	public int column;

	public GridPosition(int r, int c)
    {
		row = r;
		column = c;
    }
}

//--------------------------------------------------------------------------------------------------

public struct Move
{
	public int from_Col;
	public int from_Row;
	public int to_Col;
	public int to_Row;
	public int score;  //Required only for ordering moves.

	public Move(int fromRow, int fromCol, int toRow, int toCol)
	{
		from_Row = fromRow;
		from_Col = fromCol;
		to_Row = toRow;
		to_Col = toCol;
		score = 0;
	}
};

//--------------------------------------------------------------------------------------------------

public struct ChessMove
{
	Move theMove;
	string startPosition;
	string endPosition;
};

//--------------------------------------------------------------------------------------------------

public enum Openings
{
	RuyLopez,
	SicilianDefence,
	QueensGambit,
	AlekhineDefence,
	ModernDefence,
	KingsIndian,
	EnglishOpening,
	DutchDefence,
	StonewallAttack
}

//--------------------------------------------------------------------------------------------------*/