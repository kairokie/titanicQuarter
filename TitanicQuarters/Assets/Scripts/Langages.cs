using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Alphabets
{
    NONE,
    LATIN,
    MORSE, // Each character is separated by a space
    NAUTIC,
    MILITARY,
}
public class Word
{
    string _latinWord { get; }
    string _morseWord { get; }

    public Word(string word)
    {
        _latinWord = word;
        _morseWord = Langages.stringToMorse(word);
        //TODO : Add NAUTIC and MILITARY
    }

    public string GetWord(Alphabets _alphabets)
    {
        switch (_alphabets)
        {
            case Alphabets.NONE:
                return "";
            case Alphabets.LATIN:
                return _latinWord;
            case Alphabets.MORSE:
                return _morseWord;
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
        return (int)c - 97;
    }


    public static string stringToMorse(string s)
    {
        string morse = "";
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c != ' ')
            {
                morse += morseList[characterToInt(c)];
            }
            if (i < s.Length - 1)
            {
                morse += " ";
            }
        }
        foreach (char c in s)
        {
            

        }
        return morse;
    }


    public static List<string> morseList = new List<string>()
    {
            {"•-" },
            {"-•••" },
            {"-•-•" },
            {"-••" },
            {"•" },
            {"••-•" },
            {"--•" },
            {"••••" },
            {"••" },
            {"•---" },
            {"-•-" },
            {"•-••" },
            {"--" },
            {"-•" },
            {"---" },
            {"•--•" },
            {"--•-" },
            {"•-•" },
            {"•••" },
            {"-" },
            {"••-" },
            {"•••-" },
            {"•--" },
            {"-••-" },
            {"-•--" },
            {"--••" }
     };
}
