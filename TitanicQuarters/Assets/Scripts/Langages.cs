using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;


public enum Alphabets
{
    NONE,
    LATIN,
    MORSE, // Each character is separated by a space
    NAUTIC,
    MILITARY,
}

public enum WordSignificance
{
    SIGNIFICANCE_0,
    SIGNIFICANCE_1,
}
public class Word
{
    string _latinWord { get; }
    string _morseWord { get; }
    string _militaryWord { get; }

    List<int> _nauticWord;

    WordSignificance _significance = WordSignificance.SIGNIFICANCE_0;
    WordSignificance Significance { get => _significance; set => _significance = value; }
    

    public Word(string word,WordSignificance significance = WordSignificance.SIGNIFICANCE_0)
    {
        _latinWord = word;
        _morseWord = Langages.stringToMorse(word);
        _militaryWord = Langages.stringToMilitary(word);
        _nauticWord = Langages.stringToNauticWord(word);
    }




    public string GetWord(Alphabets _alphabets)
    {
        switch (_alphabets)
        {
            case Alphabets.NONE:
                return "NONE";
            case Alphabets.LATIN:
                return _latinWord;
            case Alphabets.MORSE:
                return _morseWord;
            case Alphabets.NAUTIC:
                return "Error (Nautic)";
            case Alphabets.MILITARY:
                return _militaryWord;
            default:
                return "Error";
        }
    }
}

static class Langages
{
    public static int characterToInt(char c)
    {
        return (int)c - 97;
    }

    public static char intTocharacter(int i)
    {
        return (char)(i + 97);
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
        return morse;
    }

    public static string stringToMilitary(string s)
    {
        string military = "";
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c != ' ')
            {
                military += militaryList[characterToInt(c)] + " ";
            }
            //if (i < s.Length - 1)
            //{
            //    military += " ";
            //}
        }
        return military;
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

    public static List<string> militaryList = new List<string>()
    {
        {"Alpha"},
        {"Bravo"},
        {"Charlie"},
        {"Delta"},
        {"Echo"},
        {"Foxtrot"},
        {"Golf"},
        {"Hotel"},
        {"India"},
        {"Juliett"},
        {"Kilo"},
        {"Lima"},
        {"Mike"},
        {"November"},
        {"Oscar"},
        {"Papa"},
        {"Quebec"},
        {"Romeo"},
        {"Sierra"},
        {"Tango"},
        {"Uniform"},
        {"Victor"},
        {"Whiskey"},
        {"X-ray"},
        {"Yankee"},
        {"Zulu"}
     };

    public static List<int> stringToNauticWord(string s)
    {
        List<int> _nauticWord = new List<int>();
        for (int i = 0; i < s.Length; i++)
        {
            _nauticWord.Add(Langages.characterToInt(s[i]));
        }
        return _nauticWord;
    }
}
