using UnityEngine;

public class Math_IntroOptions : MonoBehaviour
{
    [SerializeField] private GameObject IntroCanvas = null;
    [SerializeField] private GameObject GameCanvas = null;

    public void StartGame()
    {
        //Activate the game canvas.
        if (GameCanvas != null)
        {
            GameCanvas.SetActive(true);
        }

        //Deactivate self.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        //Activate the game canvas.
        if (IntroCanvas != null)
        {
            IntroCanvas.SetActive(true);
        }

        //Deactivate self.
        if (GameCanvas != null)
        {
            GameCanvas.SetActive(false);
        }
    }
}
