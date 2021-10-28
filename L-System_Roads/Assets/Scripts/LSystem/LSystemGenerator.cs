using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

[CreateAssetMenu(fileName = "L-System", menuName = "L-System_Roads/L-System", order = 0)]
public class LSystemGenerator : ScriptableObject 
{

    /*
    The points of the L-System ensemble
    A dictionnary with the point as key and the type ("PRIMARY","SECONDARY") as value
    */
    public Dictionary<Vector3, string> LSystemPointsDictionary = new Dictionary<Vector3,string>();

    /*
    The list of rules the L-System have, and will use
    */
    public Rule[] rules;

    /*
    The first sequence of the L-System
    */
    public string rootSentence;

    /*
    The number of secondary L-System that will start on our primary L-System
    */
    public int nbSecondaryLSystem = 1;

    /*
    The list of secondary L-Systems
    */
    private List<LSystemGenerator> secondaryLSystems = new List<LSystemGenerator>();

    /*
    The maximum depth/iterations of our primary L-System
    */
    [Range(0,10)]
    public int maxDepthPrimary = 1;

    /*
    The maximum depth/iterations of our secondary L-Systems
    */
    [Range(0,10)]
    public int maxDepthSecondary = 1;

    /*
    The length of the primary's segments
    */
    public float lengthPrimary = 10;

    /*
    The length of the secondary's segments
    */
    public float lengthSecondary = 6;

    /*
    The angle of the primary L-System
    */
    public float anglePrimary = 20;

    /*
    The angle of the secondary L-Systems
    */
    public float angleSecondary = 60;

    /*
    The starting position of the primary L-System
    */
    private Vector3 position;

    /*
    The starting direction of the primary L-System
    */
    private Vector3 primaryDirection; //TODO Quaternion ?

    /*
    The starting directions of the secondary L-Systems
    */
    private List<Vector3> secondaryDirections; //TODO Quaternion

    /*
    A boolean to activate/deactivate the random ignorance of the rule for a part of a branch
    */
    public bool randomIgnoreRuleModifier = true;

    /*
    The probability to ignore a rule once the boolean for the randomness on the ignorance of rule
    */
    [Range(0,1)]
    public float chanceToIgnoreRule = 0.3f;

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

    public string GenerateSecondaryNetwork(int position, string word){
        string secondarySentence = "[" + GenerateSentence(word) + "]";
        return secondarySentence;
    }

    /*
    Process all characters of the current sequence to get the next sequence
    Call the ProcessRuleRecursively method to get the next part of the sequence
    */
    public string GrowRecursive(string word, int depth = 0){

        if(depth >= maxDepthPrimary){
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
                if(randomIgnoreRuleModifier && depth > 3){
                    if(Random.value < chanceToIgnoreRule){
                        return;
                    }
                }
                newWord.Append(GrowRecursive(rule.GetResult(),depth+1));
            }
        }
    }

    public List<int> SelectIndexes(string sentence, char c)
    {
        List<int> indexes = new List<int>();

        for (int i = sentence.IndexOf(c); i > -1; i = sentence.IndexOf(c, i + 1))
        {
            indexes.Add(i);
        }
        return Enumerable.Range(0,indexes.Count).OrderBy(x => Random.Range(0,indexes.Count)).Take(nbSecondaryLSystem).OrderBy(x => x).ToList();
    }

}
