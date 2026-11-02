/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessOpenings : MonoBehaviour
{
	public PieceColour colour = PieceColour.White;

	Openings selectedOpening;
	int currentMove = 0;

	List<Move> Moves_RuyLopez = new List<Move>();
	List<Move> Moves_SicilianDefence = new List<Move>();
	List<Move> Moves_QueensGambit = new List<Move>();
	List<Move> Moves_AlekhineDefence = new List<Move>();
	List<Move> Moves_ModernDefence = new List<Move>();
	List<Move> Moves_KingsIndian = new List<Move>();
	List<Move> Moves_EnglishOpening = new List<Move>();
	List<Move> Moves_DutchDefence = new List<Move>();
	List<Move> Moves_StonewallAttack = new List<Move>();

	//--------------------------------------------------------------------------------------------------

	void Start()
    {
		SetUpOpenings();
		RestartGame();
	}

	//--------------------------------------------------------------------------------------------------

	void SetUpOpenings()
	{
		//Classic Openings - Or variations of:
		if (colour == PieceColour.White)
		{
			//White moves first.

			//RuyLopez Opening
			Moves_RuyLopez.Add(new Move(1, 4, 3, 4));
			Moves_RuyLopez.Add(new Move(0, 6, 2, 5));

			//SicilianDefence
			Moves_SicilianDefence.Add(new Move(1, 4, 3, 4));

			//QueensGambit
			Moves_QueensGambit.Add(new Move(1, 3, 3, 3));
			Moves_QueensGambit.Add(new Move(1, 2, 3, 2));

			//AlekhineDefence
			Moves_AlekhineDefence.Add(new Move(1, 4, 3, 4));
			Moves_AlekhineDefence.Add(new Move(0, 1, 2, 2));

			//ModernDefence
			Moves_ModernDefence.Add(new Move(1, 4, 3, 4));
			Moves_ModernDefence.Add(new Move(1, 3, 3, 3));

			//KingsIndian
			Moves_KingsIndian.Add(new Move(0, 6, 2, 5));
			Moves_KingsIndian.Add(new Move(1, 6, 2, 6));
			Moves_KingsIndian.Add(new Move(0, 5, 1, 6));

			//EnglishOpening
			Moves_EnglishOpening.Add(new Move(1, 2, 3, 2));

			//DutchDefence
			Moves_DutchDefence.Add(new Move(1, 3, 3, 3));

			//StonewallAttack
			Moves_StonewallAttack.Add(new Move(1, 3, 3, 3));
			Moves_StonewallAttack.Add(new Move(1, 4, 2, 4));
			Moves_StonewallAttack.Add(new Move(2, 5, 2, 3));
		}
		else
        {
			//Black goes second.

			//RuyLopez Opening
			Moves_RuyLopez.Add(new Move(6, 4, 4, 4));
			Moves_RuyLopez.Add(new Move(7, 1, 5, 2));

			//SicilianDefence
			Moves_SicilianDefence.Add(new Move(6, 2, 4, 2));

			//QueensGambit
			Moves_QueensGambit.Add(new Move(6, 3, 4, 3));

			//AlekhineDefence
			Moves_AlekhineDefence.Add(new Move(7, 6, 5, 5));
			Moves_AlekhineDefence.Add(new Move(6, 3, 4, 3));

			//ModernDefence
			Moves_ModernDefence.Add(new Move(6, 3, 5, 3));
			Moves_ModernDefence.Add(new Move(6, 6, 5, 6));

			//KingsIndian
			Moves_KingsIndian.Add(new Move(7, 6, 5, 5));
			Moves_KingsIndian.Add(new Move(6, 6, 5, 6));
			Moves_KingsIndian.Add(new Move(7, 5, 6, 6));

			//EnglishOpening
			Moves_EnglishOpening.Add(new Move(6, 4, 4, 4));

			//DutchDefence
			Moves_DutchDefence.Add(new Move(6, 5, 4, 5));

			//StonewallAttack
			Moves_StonewallAttack.Add(new Move(6, 3, 4, 3));
			Moves_StonewallAttack.Add(new Move(6, 4, 5, 4));
			Moves_StonewallAttack.Add(new Move(7, 6, 5, 5));
		}
	}

	//--------------------------------------------------------------------------------------------------

	void RestartGame()
	{
		currentMove = 0;
		SetARandomOpening();
	}

	//--------------------------------------------------------------------------------------------------

	void SetARandomOpening()
	{
		selectedOpening = (Openings)(Random.Range((int)Openings.RuyLopez, (int)Openings.StonewallAttack));
		DebugOutput();
	}

	//--------------------------------------------------------------------------------------------------

	public bool IsAnotherMoveAvailable()
	{
		switch (selectedOpening)
		{
			case Openings.RuyLopez:
				return (currentMove < Moves_RuyLopez.Count);
				break;

			case Openings.SicilianDefence:
				return (currentMove < Moves_SicilianDefence.Count);
				break;

			case Openings.QueensGambit:
				return (currentMove < Moves_QueensGambit.Count);
				break;

			case Openings.AlekhineDefence:
				return (currentMove < Moves_AlekhineDefence.Count);
				break;

			case Openings.ModernDefence:
				return (currentMove < Moves_ModernDefence.Count);
				break;

			case Openings.KingsIndian:
				return (currentMove < Moves_KingsIndian.Count);
				break;

			case Openings.EnglishOpening:
				return (currentMove < Moves_EnglishOpening.Count);
				break;

			case Openings.DutchDefence:
				return (currentMove < Moves_DutchDefence.Count);
				break;

			case Openings.StonewallAttack:
				return (currentMove < Moves_StonewallAttack.Count);
				break;

			default:
				return false;
				break;
		}

		//Out of moves.
		return false;
	}

	//--------------------------------------------------------------------------------------------------

	public Move GetNextMove()
	{
		Move nextMove = new Move();

		switch (selectedOpening)
		{
			case Openings.RuyLopez:
				if (currentMove < Moves_RuyLopez.Count)
				{
					nextMove = Moves_RuyLopez[currentMove];
				}
				break;

			case Openings.SicilianDefence:
				if (currentMove < Moves_SicilianDefence.Count)
				{
					nextMove = Moves_SicilianDefence[currentMove];
				}
				break;

			case Openings.QueensGambit:
				if (currentMove < Moves_QueensGambit.Count)
				{
					nextMove = Moves_QueensGambit[currentMove];
				}
				break;

			case Openings.AlekhineDefence:
				if (currentMove < Moves_AlekhineDefence.Count)
				{
					nextMove = Moves_AlekhineDefence[currentMove];
				}
				break;

			case Openings.ModernDefence:
				if (currentMove < Moves_ModernDefence.Count)
				{
					nextMove = Moves_ModernDefence[currentMove];
				}
				break;

			case Openings.KingsIndian:
				if (currentMove < Moves_KingsIndian.Count)
				{
					nextMove = Moves_KingsIndian[currentMove];
				}
				break;

			case Openings.EnglishOpening:
				if (currentMove < Moves_EnglishOpening.Count)
				{
					nextMove = Moves_EnglishOpening[currentMove];
				}
				break;

			case Openings.DutchDefence:
				if (currentMove < Moves_DutchDefence.Count)
				{
					nextMove = Moves_DutchDefence[currentMove];
				}
				break;

			case Openings.StonewallAttack:
				if (currentMove < Moves_StonewallAttack.Count)
				{
					nextMove = Moves_StonewallAttack[currentMove];
				}
				break;

			default:
				break;
		}

		//Increment to next move.
		currentMove++;

		//Out of moves.
		return nextMove;
	}

	//--------------------------------------------------------------------------------------------------

	void DebugOutput()
    {
		string col = "White";
		if(colour == PieceColour.Black)
        {
			col = "Black";
        }

		switch (selectedOpening)
		{
			case Openings.RuyLopez:
				Debug.Log("Colour: " + col + " Opening: RuyLopez");
				break;

			case Openings.SicilianDefence:
				Debug.Log("Colour: " + col + " Opening: SicilianDefence");
				break;

			case Openings.QueensGambit:
				Debug.Log("Colour: " + col + " Opening: QueensGambit");
				break;

			case Openings.AlekhineDefence:
				Debug.Log("Colour: " + col + " Opening: AlekhineDefence");
				break;

			case Openings.ModernDefence:
				Debug.Log("Colour: " + col + " Opening: ModernDefence");
				break;

			case Openings.KingsIndian:
				Debug.Log("Colour: " + col + " Opening: KingsIndian");
				break;

			case Openings.EnglishOpening:
				Debug.Log("Colour: " + col + " Opening: EnglishOpening");
				break;

			case Openings.DutchDefence:
				Debug.Log("Colour: " + col + " Opening: DutchDefence");
				break;

			case Openings.StonewallAttack:
				Debug.Log("Colour: " + col + " Opening: StonewallAttack");
				break;

			default:
				Debug.Log("Invalid opening.");
				break;
		}
	}
	//--------------------------------------------------------------------------------------------------
}
*/