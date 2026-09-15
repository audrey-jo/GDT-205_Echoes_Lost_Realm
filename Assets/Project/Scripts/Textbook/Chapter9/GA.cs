using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GA : MonoBehaviour
{
    //How many ships per generation?
    public static int kNumberofChromosomes = 50;

    //How many generations do we want to run this for?
    public static int kMaxNumberOfGenerations = 1000;

    //What percenatge rate should we mutate genes?
    public static float kMutationRate = 0.5f;

    //What percentage rate should we cross over?
    public static int kCrossoverRate = 30;

    [SerializeField] private Transform  startPosition       = null;
    [SerializeField] private Transform  finishLinePosition  = null;
    [SerializeField] private Transform  shipParent          = null;
    [SerializeField] private GameObject prefabToSpawn       = null;
    [SerializeField] private Text       generationText      = null;
    [SerializeField] private Text       succsessfulText     = null;
    private GameData gameData = null;

    List<Chromosome>     chromosomes         = new List<Chromosome>();
    List<Chromosome>     selectedChromosomes = new List<Chromosome>();
    List<int>            currentGeneIndex    = new List<int>();
    List<float>          chromosomeTimes     = new List<float>();
    List<float>          chromosomeFitness   = new List<float>();
    List<ShipController> ships               = new List<ShipController>();

    private int  iSuccessfulLastGeneration  = 0;
    private int  iCurrentGeneration         = 0;
    private int  iNumberOfEliteSelections   = 0;
    private bool bAllowMutation             = true;

    //--------------------------------------------------------------------------------------

    void Start()
    {
        gameData = FindObjectOfType<GameData>();
    }

    //--------------------------------------------------------------------------------------

    public void ResetGA()
    {
        if (shipParent != null && prefabToSpawn != null && startPosition != null)
        {
            //Kill any previously used ships.
            foreach (Transform child in shipParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            //Clear any old data.
            ships.Clear();
            chromosomes.Clear();
            chromosomeTimes.Clear();
            chromosomeFitness.Clear();
            currentGeneIndex.Clear();
            selectedChromosomes.Clear();

            iCurrentGeneration = 0;
            iSuccessfulLastGeneration = 0;

            //Reset the Genetic Algorithm.
            for (int index = 0; index < kNumberofChromosomes; index++)
            {
                //Create a new chromosome.
                Chromosome chromosome = new Chromosome();
                chromosome.GenerateRandomChromosome();

                //Add chromosome to list, along with other required elements.
                chromosomes.Add(chromosome);
                chromosomeTimes.Add(0.0f);
                chromosomeFitness.Add(0.0f);
                currentGeneIndex.Add(0);

                selectedChromosomes.Add(new Chromosome());

                //Generate a ship for each chromosome.
                GameObject newShip = Instantiate(prefabToSpawn, startPosition.position, Quaternion.identity, shipParent);

                if (newShip != null)
                {
                    ShipController shipController = newShip.GetComponent<ShipController>();

                    if (shipController != null)
                    {
                        shipController.SetStartPosition(startPosition.position);
                        shipController.SetIsAI(true);
                        ships.Add(shipController);
                    }
                }
            }

            RestartGA();
        }

        //Show the Generation text.
        if (generationText != null && succsessfulText != null)
        {
            generationText.gameObject.SetActive(true);
            succsessfulText.gameObject.SetActive(true);
        }
    }

    //--------------------------------------------------------------------------------------

    void Update()
    {
        //Early out if game is not active.
        if (gameData != null && !gameData.bGameActive)
        {
            return;
        }

        if (shipParent != null && prefabToSpawn != null && startPosition != null)
        {
            bool bAreAnyShipsAlive = false;

            //Give each ship the next instruction in their corresponding chromosome.
            for (int chromosomeIndex = 0; chromosomeIndex < kNumberofChromosomes; chromosomeIndex++)
            {
                if (ships[chromosomeIndex] != null && ships[chromosomeIndex].IsAlive())
                {
                    bAreAnyShipsAlive = true;

                    //Deduct delta time from the current wait time.
                    chromosomeTimes[chromosomeIndex] -= Time.deltaTime;

                    //Is it time for the next instruction?
                    if (chromosomeTimes[chromosomeIndex] <= 0.0f)
                    {
                        //Move on to the next instruction for this ship - if valid.
                        if ((currentGeneIndex[chromosomeIndex] + 1) < Chromosome.kNumberNofGenes)
                        {
                            string debugOutput = "";
                            currentGeneIndex[chromosomeIndex] = currentGeneIndex[chromosomeIndex] + 1;

                            //Sample the instruction, and instruct the ship.
                            switch (chromosomes[chromosomeIndex].genes[currentGeneIndex[chromosomeIndex]].eInstruction)
                            {
                                case GAInstruction.Thrust:      ships[chromosomeIndex].Thrust();        debugOutput += "Thrust";        break;
                                case GAInstruction.RotateLeft:  ships[chromosomeIndex].RotateLeft();    debugOutput += "RotateLeft";    break;
                                case GAInstruction.RotateRight: ships[chromosomeIndex].RotateRight();   debugOutput += "RotateRight";   break;
                                default: break;

                            }

                            //Store the delay to the next instruction.
                            chromosomeTimes[chromosomeIndex] = chromosomes[chromosomeIndex].genes[currentGeneIndex[chromosomeIndex]].fDuration;

                            debugOutput += " : " + chromosomeTimes[chromosomeIndex];
                            //Debug.Log(debugOutput);
                        }
                    }
                }
            }

            //If none of the ships are alive, then its time to evolve.
            if(bAreAnyShipsAlive == false)
            {
                GenerationComplete();
            }
        }
    }

    //--------------------------------------------------------------------------------------

    void GenerationComplete()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------

    void Evolve()
    {
        Debug.Log("---Evolving now---");

        //Select the chromosomes to be used for the net generation.
        iNumberOfEliteSelections = 0;
        Selection();

        //Crossover selected chromosomes to create next genneration.
        Crossover();

        //Mutate any chromosomes.
        if (bAllowMutation)
        {
            Mutation();
        }

        Debug.Log("---Evolved---");
    }

    //--------------------------------------------------------------------------------------

    void CalculateFitness()
    {
        if (finishLinePosition != null)
        {
            float fDistFromStartToFinish = VectorMath.Magnitude(finishLinePosition.position - startPosition.position);

            //We are going to score each ship based on how close it got to the finish line.
            for (int currentShip = 0; currentShip < kNumberofChromosomes; currentShip++)
            {
                float fDistFromShipToFinish = VectorMath.Magnitude(finishLinePosition.position - ships[currentShip].transform.position);

                //Fitness of 1 is good (reached the finish line), Fitness of 0 is bad.
                chromosomeFitness[currentShip] = 1.0f - (fDistFromShipToFinish / fDistFromStartToFinish);

                //Debug.Log("Ship[" + currentShip + "] fitness = " + chromosomeFitness[currentShip]);

                //Keep track of how many ships were successful.
                if(ships[currentShip].WasSuccessful())
                {
                    iSuccessfulLastGeneration++;
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------

    void Selection()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------

    void EliteSelection()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------------------

    void TournamentSelection()
    {
        //Todo: Add code here.  
    }

    //--------------------------------------------------------------------------------------------------

    void RouletteWheelSelection()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------------------

    void StochasticSelection()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------

    void Crossover()
    {
        //Clear all information in the last generation of Chromosomes.
        ClearChromosomes();

        //Copy across Elite selections.
        for (int currentChromosome = 0; currentChromosome < iNumberOfEliteSelections; currentChromosome++)
        {
            chromosomes[currentChromosome].Copy(selectedChromosomes[currentChromosome]);
        }

        //Multi-point crossover.
        for (int currentChromosome = iNumberOfEliteSelections; currentChromosome < kNumberofChromosomes; currentChromosome += 2)
        {
            for (int currentGene = 0; currentGene < Chromosome.kNumberNofGenes; currentGene += 2)
            {
                if (Random.Range(0, 100) < kCrossoverRate)
                {
                    //SWAP
                    //Chromosome 1.
                    chromosomes[currentChromosome].genes[currentGene].Copy(selectedChromosomes[currentChromosome].genes[currentGene]);
                    chromosomes[currentChromosome].genes[currentGene + 1].Copy(selectedChromosomes[currentChromosome + 1].genes[currentGene + 1]);
                    //Chromosome 2.
                    chromosomes[currentChromosome + 1].genes[currentGene].Copy(selectedChromosomes[currentChromosome + 1].genes[currentGene]);
                    chromosomes[currentChromosome + 1].genes[currentGene + 1].Copy(selectedChromosomes[currentChromosome].genes[currentGene + 1]);
                }
                else
                {
                    //STICK
                    //Chromosome 1.
                    chromosomes[currentChromosome].genes[currentGene].Copy(selectedChromosomes[currentChromosome].genes[currentGene]);
                    chromosomes[currentChromosome].genes[currentGene + 1].Copy(selectedChromosomes[currentChromosome].genes[currentGene + 1]);
                    //Chromosome 2.
                    chromosomes[currentChromosome + 1].genes[currentGene].Copy(selectedChromosomes[currentChromosome + 1].genes[currentGene]);
                    chromosomes[currentChromosome + 1].genes[currentGene + 1].Copy(selectedChromosomes[currentChromosome + 1].genes[currentGene + 1]);
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------

    void Mutation()
    {
        //Todo: Add code here.
    }

    //--------------------------------------------------------------------------------------

    void RestartGA()
    {
        for (int chromosomeIndex = 0; chromosomeIndex < kNumberofChromosomes; chromosomeIndex++)
        {
            currentGeneIndex[chromosomeIndex] = 0;
            chromosomeFitness[chromosomeIndex] = 0.0f;
            chromosomeTimes[chromosomeIndex] = 0.0f;
            ships[chromosomeIndex].ResetShip();

            //Clear out the chromosomes we had from the last generation.
            selectedChromosomes[chromosomeIndex].ClearChromosome();
        }

        iCurrentGeneration++;
        Debug.Log("---GENERATION " + iCurrentGeneration + "---");

        //If we have the text object on screen hooked up.
        if(generationText != null && succsessfulText != null)
        {
            generationText.text = "Generation: " + iCurrentGeneration;
            succsessfulText.text = "Successful: " + iSuccessfulLastGeneration;
        }

        iSuccessfulLastGeneration = 0;
    }

    //--------------------------------------------------------------------------------------

    void ClearChromosomes()
    {
        for (int index = 0; index < kNumberofChromosomes; index++)
        {
            chromosomes[index].ClearChromosome();
        }
    }

    //--------------------------------------------------------------------------------------------------
}
