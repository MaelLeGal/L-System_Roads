using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Our grammar, the letter used in the different sequences and what they represent
*/
public enum EncodingLetters
{
    unknown = '1',
    save = '[',
    load = ']',
    draw = 'F',
    drawSecondary = 'S',
    turnRight = '+',
    turnLeft = '-',
    turnRightSecondary = '/',
    turnLeftSecondary = '\\'
}
