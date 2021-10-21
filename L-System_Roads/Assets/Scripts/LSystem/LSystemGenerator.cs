using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LSystemGenerator : MonoBehaviour
{
    /*
    The list of rules the L-System have, and will use
    */
    public Rule[] rules;

    /*
    The first sequence of the L-System
    */
    public string rootSentence;

    /*
    The maximum depth/iterations of our L-System
    */
    [Range(0,10)]
    public int maxDepth = 1;

    /*
    A boolean to activate/deactivate the random ignorance of the rule for a part of a branch
    */
    public bool randomIgnoreRuleModifier = true;

    /*
    The probability to ignore a rule once the boolean for the randomness on the ignorance of rule
    */
    [Range(0,1)]
    public float chanceToIgnoreRule = 0.3f;

    public void Start(){
        Debug.Log(GenerateSentence());
    }

    /*
    Generate the full sentence of our L-System
    Take in parameter a starting sentence or null
    Null parameter will use the root sentence defined as the starting sentence
    Call the GrowRecursive method to create the full sentence
    */
    public string GenerateSentence(string word = null){
        if(word == null){
            word = rootSentence;
        }

        return GrowRecursive(word);
    }

    public string GeneratePrimaryNetwork(string word = null){
        return GenerateSentence(word);
    }

    public string GenerateSecondaryNetwork(string word = null){
        return GenerateSentence(word);
    }

    /*
    Process all characters of the current sequence to get the next sequence
    Call the ProcessRuleRecursively method to get the next part of the sequence
    */
    public string GrowRecursive(string word, int depth = 0){

        if(depth >= maxDepth){
            return word;
        }

        StringBuilder newWord = new StringBuilder();
        foreach(char c in word){
            newWord.Append(c);
            ProcessRuleRecursively(newWord, c, depth);
        }

        return newWord.ToString();
    }

    /*
    Process the rule on the character passed as a parameter
    Call the GrowRecursive method to get the next sequence to process
    */
    public void ProcessRuleRecursively(StringBuilder newWord, char c, int depth)
    {   
        foreach(Rule rule in rules){
            if(rule.letter == c.ToString())
            {
                if(randomIgnoreRuleModifier && depth > 3){ // maxDepth Remove maxDepth and change it to value
                    if(Random.value < chanceToIgnoreRule){
                        return;
                    }
                }
                newWord.Append(GrowRecursive(rule.GetResult(),depth+1));
            }
        }
    }

}
