using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatmapCell : MonoBehaviour
{
    [SerializeField] private Text cellText;
    [SerializeField] private Image cellImage;

    private float fDecaySpeed = 0.05f;

    public HeatmapCellStatus eCellStatus = HeatmapCellStatus.Accessible;
    private float fStartingValue = 0.0f;
    public float fValue;

    public int row;
    public int col;

    //-----------------------------------------------------------------------------------

    void Update()
    {
        Refresh();
    }

    //-----------------------------------------------------------------------------------

    public void SetPosition(int iRow, int iColumn)
    {
        col = iColumn;
        row = iRow;
    }

    //--------------------------------------------------------------------------------------------

    public void Refresh()
    {
        DecayValue();

        if (cellImage != null)
        {
            //Only change the colour of accessible cells.
            if (eCellStatus == HeatmapCellStatus.Accessible)
            {
                Color newColour = Color.white;
                newColour.r = fValue;
                newColour.b = 1.0f - fValue;
                newColour.g = 0.0f;
                cellImage.color = newColour;
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void DecayValue()
    {
        if (eCellStatus == HeatmapCellStatus.Accessible)
        {
            if (fValue > 0.0f)
            {
                fValue -= Time.deltaTime * fDecaySpeed;
            }
            else
            {
                fValue = 0.0f;
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SetStatus(HeatmapCellStatus cellStatus)
    {
        if (cellImage != null && cellText != null)
        {
            eCellStatus = cellStatus; 
            fValue = fStartingValue;
            cellText.text = "";

            switch (cellStatus)
            {
                case HeatmapCellStatus.Accessible: 
                    cellImage.color = Color.white; 
                    break;

                case HeatmapCellStatus.InAccessible: 
                    cellImage.color = Color.grey; 
                    break;

                case HeatmapCellStatus.Food: 
                    cellImage.color = Color.green; 
                    fValue = 0.0f;
                    cellText.text = "F"; 
                    break;

                case HeatmapCellStatus.Home: 
                    cellImage.color = Color.white;
                    fValue = 1.0f;
                    cellText.text = "H"; 
                    break;
            }
        }
    }

    //--------------------------------------------------------------------------------------------

    public void SetValue(float value)
    {
        if (eCellStatus == HeatmapCellStatus.Accessible)
        {
            //Keep the value between 0 and 1.
            fValue = Mathf.Max(0.0f, value);
            fValue = Mathf.Min(1.0f, value);
        }
    }

    //--------------------------------------------------------------------------------------------

    public float GetValue()
    {
        return fValue;
    }

    //--------------------------------------------------------------------------------------------
}
