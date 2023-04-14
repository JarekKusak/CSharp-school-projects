using System;
using System.Collections.Generic;
using System.IO;
namespace Odstavec;
class Program
{
    static bool queueEmpty = false;
    static int letters = 0;
    static int spaces = 0;
    static void Main(string[] args)
    {
        const string inputFileName = "odst.in";
        const string outputFileName = "odst.out";
        using StreamReader reader = new StreamReader(inputFileName); // try, finally (volá sr.Dispose = Close) 
        using StreamWriter writer = new StreamWriter(outputFileName);

        int charCount;

        try { charCount = int.Parse(reader.ReadLine()); }
        catch { return; }
        Queue<string> words = new Queue<string>();
        while (true)
        {
            string? line = reader.ReadLine();
            if (line == null)
                break;

            foreach (string word in line.Trim().Split(" "))
            {
                if (word == " " || word == "")
                    continue;
                words.Enqueue(word);
            }
        }

        while (words.Count != 0)
        {
            List<string> wordsToWrite = WordsOnLine(words, charCount);
            int numberOfSpacesBetweenWords = 0;
            int additionalSpaces = 0;

            if (queueEmpty == false)
            {
                int spaceCount = charCount - letters;
                if (spaces != 0) // one word on line
                {
                    numberOfSpacesBetweenWords = spaceCount / spaces;
                    additionalSpaces = spaceCount % spaces;
                }
            }
            else
            {
                numberOfSpacesBetweenWords = 1;
                additionalSpaces = 0;
            }
            int counterWords = 0;
            foreach (string s in wordsToWrite)
            {
                writer.Write(s);
                if (counterWords != wordsToWrite.Count)
                    counterWords++;
                else break;
                for (int i = 0; i < numberOfSpacesBetweenWords; i++)
                {
                    writer.Write(" ");
                }
                if (additionalSpaces > 0)
                {
                    writer.Write(" ");
                    additionalSpaces--;
                }    
            }
            writer.WriteLine();
        }
    }

    static List<string> WordsOnLine(Queue<string> words, int charCount)
    {
        letters = 0;
        spaces = 0;
        List<string> wordsOnLine = new List<string>();
        while (letters + spaces + words.Peek().Length < charCount)
        {
            letters += words.Peek().Length;
            spaces++;
            wordsOnLine.Add(words.Dequeue());

            if (words.Count == 0)
            {
                spaces--;
                queueEmpty = true;
                return wordsOnLine;
            }
        }
        if (letters + spaces + words.Peek().Length == charCount) // přidáme ještě jedno slovo, které bude tak akorát :D
        {
            letters += words.Peek().Length;
            wordsOnLine.Add(words.Dequeue());
        }
        else
            spaces--;
        return wordsOnLine;
    }
}
