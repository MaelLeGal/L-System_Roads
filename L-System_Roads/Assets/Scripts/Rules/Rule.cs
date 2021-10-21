using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "L-System_Roads/Rule", order = 0)]
public class Rule : ScriptableObject 
{
    /*
    The letter of this rule
    ie : F
    */
    public string letter;

    /*
    The array of output this rule have
    ie : [F -> +F, F -> -F, ...]
    */
    [SerializeField]
    private string[] results = null;

    /*
    A boolean to activate/deactivate the randomness on the outputs
    */
    [SerializeField]
    private bool randomResult = false;


    /*
    Return one of the outputs of the rule
    If the randomness is activated the selection will be random
    If the randomness is deactivated the output will always be the first
    */
    public string GetResult()
    {
        if(randomResult){
            return results[Random.Range(0, results.Length)];
        }
        return results[0];
    }
}

