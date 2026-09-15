using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chromosome
{
    public static int kNumberNofGenes = 50;

    public List<Gene> genes = new List<Gene>();

    //--------------------------------------------------------------------------------------

    public Chromosome()
    {
        for (int geneIndex = 0; geneIndex < kNumberNofGenes; geneIndex++)
        {
            genes.Add(new Gene());
        }
    }

    //--------------------------------------------------------------------------------------

    public void ClearChromosome()
    {
        for (int geneIndex = 0; geneIndex < kNumberNofGenes; geneIndex++)
        {
            genes[geneIndex].ClearGene();
        }
    }

    //--------------------------------------------------------------------------------------

    public void GenerateRandomChromosome()
    {
        for (int geneIndex = 0; geneIndex < kNumberNofGenes; geneIndex++)
        {
            genes[geneIndex].GenerateRandomGene();
        }
    }

    //--------------------------------------------------------------------------------------

    public void Copy(Chromosome chromosomeToCopy)
    {
        for (int geneIndex = 0; geneIndex < kNumberNofGenes; geneIndex++)
        {
            genes[geneIndex].Copy(chromosomeToCopy.genes[geneIndex]);
        }
    }

    //--------------------------------------------------------------------------------------
}
