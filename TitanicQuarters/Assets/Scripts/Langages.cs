using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Alphabets
{
    MORSE,
    LATIN,
    NAUTIC,
    MILITARY,
}

public class Word
{
    string _latinWord { get; }
    string _morseWord { get; }

    public Word(string word, Alphabets alphabet)
    {
        switch (alphabet)
        {
            case Alphabets.MORSE:
                _morseWord = word;
                _latinWord = Langages.morseToString(word);
                break;
            case Alphabets.LATIN:
                _morseWord = Langages.stringToMorse(word);
                _latinWord = word;
                break;
            default:
                _morseWord = Langages.stringToMorse(word);
                _latinWord = word;
                break;

                //TODO : Add NAUTIC and MILITARY
        }
    }

    public char GetLetter(int index, Alphabets _alphabets)
    {
        char letter = _latinWord[index];
        switch (_alphabets)
        {
            case Alphabets.MORSE:
                return _morseWord[index];
            case Alphabets.LATIN:
                return letter;
            //case alphabet.NAUTIC:
            //    return letter;
            //case alphabet.MILITARY:
            //return letter;
            default:
                return letter;
        }
    }

    public string GetWord(Alphabets _alphabets)
    {
        switch (_alphabets)
        {
            case Alphabets.MORSE:
                return _morseWord;
            case Alphabets.LATIN:
                return _latinWord;
            case Alphabets.NAUTIC:
                return _latinWord;
            case Alphabets.MILITARY:
                return _latinWord;
            default:
                return _latinWord;
        }
    }
}

static class Langages
{
    public static int characterToInt(char c)
    {
        return (int)c - 96;
    }

    public static char intToChar(int i)
    {
        return (char)(i + 96);
    }

    public static string stringToMorse(string s)
    {
        string morse = "";
        foreach (char c in s)
        {
            if (c == ' ')
            {
                morse += " ";
            }
            else
            {
                morse += intToMorse(characterToInt(c));
            }
        }
        return morse;
    }

    public static string intToMorse(int i)
    {
        if (intToMorseDict.ContainsKey(i))
        {
            return intToMorseDict[i];
        }
        Debug.Log(" intToMorse Error: " + i);
        return "!";
    }

    public static string morseToString(string morse)
    {
        string s = "";
        string[] morseArray = morse.Split(' ');
        foreach (char c in morse)
        {
            if (c == ' ')
            {
                s += " ";
            }
            else
            {
                s += intToChar(morseToInt(c));
            }
        }
        return s;
    }

    public static int morseToInt(char c)
    {
        foreach (KeyValuePair<int, string> entry in intToMorseDict)
        {
            if (entry.Value == c.ToString())
            {
                return entry.Key;
            }
        }
        Debug.Log("morseToInt Error: " + c);
        return -1;
    }

    public static Dictionary<int, string> intToMorseDict = new Dictionary<int, string>()
        {
            {1,"•-" },
            {2,"-•••" },
            {3,"-•-•" },
            {4,"-••" },
            {5,"•" },
            {6,"••-•" },
            {7,"--•" },
            {8,"••••" },
            {9,"••" },
            {10,"•---" },
            {11,"-•-" },
            {12,"•-••" },
            {13,"--" },
            {14,"-•" },
            {15,"---" },
            {16,"•--•" },
            {17,"--•-" },
            {18,"•-•" },
            {19,"•••" },
            {20,"-" },
            {21,"••-" },
            {22,"•••-" },
            {23,"•--" },
            {24,"-••-" },
            {25,"-•--" },
            {26,"--••" }

        };
}
