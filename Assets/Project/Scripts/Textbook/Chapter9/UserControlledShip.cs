/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControlledShip : MonoBehaviour
{
    [SerializeField] private Transform startPosition = null;
    [SerializeField] private Transform shipParent = null;
    [SerializeField] private GameObject prefabToSpawn = null;
    [SerializeField] private Text generationText = null;

    private GameData gameData = null;

    //--------------------------------------------------------------------------------------
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
    }

    //--------------------------------------------------------------------------------------

    public void ResetUserControlledShip()
    {
        if (shipParent != null && prefabToSpawn != null && startPosition != null)
        {
            //Kill any previously used ships.
            foreach (Transform child in shipParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            //Generate a ship for the player to control.
            GameObject newShip = Instantiate(prefabToSpawn, startPosition.position, Quaternion.identity, shipParent);

            if (newShip != null)
            {
                ShipController shipController = newShip.GetComponent<ShipController>();

                if (shipController != null)
                {
                    shipController.SetStartPosition(startPosition.position);
                    shipController.SetIsAI(false);
                }
            }
        }

        //Remove the Generation text.
        if(generationText != null)
        {
            generationText.gameObject.SetActive(false);
        }
    }

    //--------------------------------------------------------------------------------------
}
*/