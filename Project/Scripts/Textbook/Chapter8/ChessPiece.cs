using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChessPiece : MonoBehaviour
{
	[SerializeField] private Image pieceImage = null;

	public PieceDetails pieceDetails;

	//Thee must be set up in the order specified in ChessConstants - PieceType
	public List<Sprite> whiteSprites = new List<Sprite>();
	public List<Sprite> blackSprites = new List<Sprite>();

	//--------------------------------------------------------------------------------------------------

	public void SetDetails(PieceDetails details)
	{
		pieceDetails = details;

		SetPieceImage();
	}

	//--------------------------------------------------------------------------------------------------

	// Update is called once per frame
	void Update()
    {
        
    }

	//--------------------------------------------------------------------------------------------------

	public void SetPieceImage()
    {
		if(pieceDetails.pieceColour == PieceColour.White)
        {
			pieceImage.sprite = whiteSprites[(int)pieceDetails.pieceType];
		}
        else
        {
			pieceImage.sprite = blackSprites[(int)pieceDetails.pieceType];
		}
	}

	//--------------------------------------------------------------------------------------------------
}
