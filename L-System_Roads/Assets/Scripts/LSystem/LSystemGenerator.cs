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
    A dictionnary with the type ("PRIMARY","SECONDARY") as key and a list of points as values
    */
    public Dictionary<string, List<Vector3Int>> LSystemPointsDictionary = new Dictionary<string, List<Vector3Int>>();

    /* 
    The coord of the L-System ensemble
    */
    private List<Vector3Int> LSystemPointsList;// = new List<Vector3Int>();
    public List<Vector3Int> _LSystemPointsList { get => LSystemPointsList; set => LSystemPointsList = value; }

    /*
    The list of rules the L-System have, and will use
    */
    public Rule[] rules;

    /*
    The first sequence of the L-System
    */
    public string primaryRootSentence;

    /*
    The first sequence of the secondary L-System
    */
    public string secondaryRootSentence;

    /*
    The full sentence
    */
    private string fullSentence;
    public string FullSentence { get => fullSentence; }

    /*
    The number of secondary L-System that will start on our primary L-System
    */
    public int nbSecondaryLSystem = 1;

    /*
    The maximum depth/iterations of our primary L-System
    */
    [Range(0, 10)]
    public int maxDepthPrimary = 1;

    /*
    The maximum depth/iterations of our secondary L-Systems
    */
    [Range(0, 10)]
    public int maxDepthSecondary = 1;

    /*
    The length of the primary's segments
    */
    public int lengthPrimary = 10;

    /*
    The length of the secondary's segments
    */
    public int lengthSecondary = 6;

    /*
    The angle of the primary L-System
    */
    public int anglePrimary = 20;

    /*
    The angle of the secondary L-Systems
    */
    public int angleSecondary = 60;

    /*
    The starting position of the primary L-System
    */
    private Vector3Int position;
    public Vector3Int Position { get => position; set => position = value; }

    /*
    The starting direction of the primary L-System
    */
    private Vector3Int primaryDirection;
    public Vector3Int PrimaryDirection { get => primaryDirection; set => primaryDirection = value; }

    /*
    The starting directions of the secondary L-Systems
    */
    private List<Vector3Int> secondaryDirections;
    public List<Vector3Int> SecondaryDirections { get => secondaryDirections; set => secondaryDirections = value; }

    /*
    The starting direction of the secondary L-Systems
    */
    private Vector3Int secondaryDirection;
    public Vector3Int SecondaryDirection { get => secondaryDirection; set => secondaryDirection = value; }

    /*
    A boolean to activate/deactivate the random ignorance of the rule for a part of a branch
    */
    public bool randomIgnoreRuleModifier = true;

    /*
    The probability to ignore a rule once the boolean for the randomness on the ignorance of rule
    */
    [Range(0, 1)]
    public float chanceToIgnoreRule = 0.3f;

    /*
    Helper function to create the sentence of the different level of networks
    return the full sentence of a network
    */
    public string GenerateSentence(int maxDepth, string word = null)
    {
        return GrowRecursive(maxDepth, word);
    }

    /*
    Generate the full sentence of our Primary Network
    Take in parameter a starting sentence or null
        Null parameter will use the root sentence defined as the starting sentence
        Call the GenerateSentence method to create the full primary network sentence
    return the primary network sentence
    */
    public string GeneratePrimaryNetwork(string word = null)
    {
        if (word == null)
        {
            word = primaryRootSentence;
        }
        return GenerateSentence(maxDepthPrimary, word);
    }

    /*
    Generate the full sentence of our Secondary Network
    Take in parameter :
        An int, the position at which the secondary network will be inserted
        A string, the current sentence of our systems
        An output int, the size of our secondary network sentence
        A string or null, the starting sentence of our secondary network L-System
            Null parameter will use the root sentence defined as the starting sentence
            Call the Generate method to create the full sentence
    Call the GenerateSentence method to create the full secondary network sentence
    return the current sentence modified with the inserted secondary network sentence
    */
    public string GenerateSecondaryNetwork(int position, string sentence, out int secondarySentenceSize, string word = null)
    {
        if (word == null)
        {
            word = secondaryRootSentence;
        }
        string secondarySentence = "[" + GenerateSentence(maxDepthPrimary, word) + "]";
        secondarySentenceSize = secondarySentence.Length;
        sentence = sentence.Insert(position, secondarySentence);
        return sentence;
    }

    /*
    Generate the full sentence of our L-System with primary and secondary network
    return the full sentence of our L-System
    */
    public string GenerateNetwork(string word = null)
    {
        string sequence = GeneratePrimaryNetwork();
        List<int> indexes = SelectIndex(sequence, 'F');

        int offset = 0;
        int secondarySentenceSize;

        foreach (int index in indexes)
        {
            sequence = GenerateSecondaryNetwork(index + offset, sequence, out secondarySentenceSize);
            offset += secondarySentenceSize;
        }
        ProcessSentence(sequence);
        fullSentence = sequence;
        return sequence;
    }

    /*
    Process all characters of the current sequence to get the next sequence
    Call the ProcessRuleRecursively method to get the next part of the sequence
    */
    public string GrowRecursive(int maxDepth, string word, int depth = 0)
    {
        if (depth >= maxDepth)
        {
            return word;
        }

        StringBuilder newWord = new StringBuilder();
        foreach (char c in word)
        {
            newWord.Append(c);
            ProcessRuleRecursively(newWord, c, depth, maxDepth);
        }

        return newWord.ToString();
    }

    /*
    Process the rule on the character passed as a parameter
    Call the GrowRecursive method to get the next sequence to process
    */
    public void ProcessRuleRecursively(StringBuilder newWord, char c, int depth, int maxDepth)
    {
        foreach (Rule rule in rules)
        {
            if (rule.letter == c.ToString())
            {
                if (randomIgnoreRuleModifier && depth > 3)
                {
                    if (Random.value < chanceToIgnoreRule)
                    {
                        return;
                    }
                }
                newWord.Append(GrowRecursive(maxDepth, rule.GetResult(), depth + 1));
            }
        }
    }

    /*
    Select randomly nbSecondaryLSystem index of the specified character in a sentence
    return the index which will correspond to the starting points of our secondary networks
    */
    public List<int> SelectIndex(string sentence, char c)
    {
        List<int> indexes = new List<int>();

        for (int i = sentence.IndexOf(c); i > -1; i = sentence.IndexOf(c, i + 1))
        {
            indexes.Add(i);
        }

        return indexes.OrderBy(x => Random.Range(1, indexes.Count)).Take(nbSecondaryLSystem).OrderBy(x => x).ToList();
    }

    /*
    Process the sentence to create each point of our L-System
    */
    public void ProcessSentence(string sentence)
    {
        LSystemPointsList = new List<Vector3Int>();
        Stack<AgentParameter> savePoints = new Stack<AgentParameter>();
        Vector3Int currentPosition = Vector3Int.FloorToInt(position);
        Vector3Int direction = primaryDirection;
        Vector3 tempPosition = position;

        LSystemPointsDictionary["PRIMARY"] = new List<Vector3Int> { currentPosition };
        LSystemPointsList.Add(currentPosition);
        foreach (char letter in sentence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.unknown:
                    break;
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameter { position = currentPosition, direction = primaryDirection, length = lengthPrimary });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        AgentParameter ap = savePoints.Pop();
                        currentPosition = ap.position;
                        primaryDirection = ap.direction;
                        lengthPrimary = ap.length;
                    }
                    else
                    {
                        throw new System.Exception("No point saved in Stack");
                    }
                    break;
                case EncodingLetters.draw:
                    currentPosition += Vector3Int.FloorToInt(primaryDirection * lengthPrimary);
                    List<Vector3Int> primaryPoints = LSystemPointsDictionary["PRIMARY"];
                    primaryPoints.Add(currentPosition);
                    LSystemPointsDictionary["PRIMARY"] = primaryPoints;
                    LSystemPointsList.Add(currentPosition);
                    break;
                case EncodingLetters.drawSecondary:
                    currentPosition += Vector3Int.FloorToInt(secondaryDirection * lengthSecondary);
                    List<Vector3Int> secondaryPoints;
                    if (!LSystemPointsDictionary.ContainsKey("SECONDARY"))
                    {
                        secondaryPoints = new List<Vector3Int>();
                    }
                    else
                    {
                        secondaryPoints = LSystemPointsDictionary["PRIMARY"];
                    }
                    secondaryPoints.Add(currentPosition);
                    LSystemPointsDictionary["SECONDARY"] = secondaryPoints;
                    LSystemPointsList.Add(currentPosition);
                    break;
                case EncodingLetters.turnRight:
                    primaryDirection = Vector3Int.FloorToInt(Quaternion.AngleAxis(anglePrimary, Vector3.up) * primaryDirection);
                    break;
                case EncodingLetters.turnLeft:
                    primaryDirection = Vector3Int.FloorToInt(Quaternion.AngleAxis(-anglePrimary, Vector3.up) * primaryDirection);
                    break;
                case EncodingLetters.turnRightSecondary:
                    secondaryDirection = Vector3Int.FloorToInt(Quaternion.AngleAxis(angleSecondary, Vector3.up) * secondaryDirection);
                    break;
                case EncodingLetters.turnLeftSecondary:
                    secondaryDirection = Vector3Int.FloorToInt(Quaternion.AngleAxis(-angleSecondary, Vector3.up) * secondaryDirection);
                    break;
                default:
                    break;
            }
        }
    }

}
