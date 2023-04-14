using System;
using System.IO;

class MatrixMultip
{
  // metoda na násobení matic s memoizací
  static int ChainedMatrix(int[] matrices, int i, int j, int[,] arrayMemo, int[,] brackets)
  {
    // násobení jedné matice
    if (i == j)
      return 0;
    // pokud byl již spočítaný výsledek
    if (arrayMemo[i, j] != -1)
      return arrayMemo[i, j];
    // něco velkého :D
    arrayMemo[i, j] = 10000000;

    for (int k = i; k < j; k++)
    {
      /*
      Celý výraz ChainedMatrix(matrices, i, k, arrayMemo, brackets) + 
      + ChainedMatrix(matrices, k + 1, j, arrayMemo, brackets) + matrices[i - 1] * matrices[k] * matrices[j] 
      reprezentuje počet násobení potřebných pro vynásobení dvou intervalů matic a vynásobení výsledné matice těmito intervaly
      */
      int l = ChainedMatrix(matrices, i, k, arrayMemo, brackets) + ChainedMatrix(matrices, k+1, j, arrayMemo, brackets)+matrices[i-1]*matrices[k]*matrices[j];
      if (l < arrayMemo[i,j])
      {
          arrayMemo[i, j] = l;
          brackets[i, j] = k; // ve kterém úseku byl součin matic podrozdělen na dva podúseky
      }
    }
    
    return arrayMemo[i, j];
  }
  static bool twoStars = true; // pokud hrozí dvě "*" vedle sebe při výpisu

  // metoda na výpis uzávorkování
  static void PrintBrackets(int[,] brackets, int i, int j, bool whereStar)
  {
      if (i == j) // pokud i == j, vypisuje se matice 
      {
        if (isOnlyOneMatrix)
        {
          Console.Write($"(A{i})");
          return;
        }
        if (whereStar != true && twoStars)
          Console.Write("*");
        Console.Write($"A{i}");
        if (whereStar)
          Console.Write("*");
          twoStars = false;
        return; 
      }
      if (whereStar != true && twoStars)
        Console.Write("*");
      Console.Write("(");
      PrintBrackets(brackets, i, brackets[i,j], true);
      PrintBrackets(brackets, brackets[i,j] + 1, j, false);
      Console.Write(")");
      twoStars = true;
  }
  
  // hlavní řídící metoda
  static int MatrixChainOrder(int[] array)
  {
      int n = array.Length;
      int[,] arrayMemo = new int[n,n]; // pole na memoizaci
      int[,] brackets = new int[n, n]; // pole na detekci závorek pro budoucí vypisování
      for(int i = 0; i < n; i++) // inicializace
      {
        for(int j = 0; j < n; j++)
        {
            arrayMemo[i, j] = -1;
            brackets[i, j] = -1;
        }
      }

      int result = ChainedMatrix(array, 1, n - 1, arrayMemo, brackets);
      PrintBrackets(brackets, 1, n-1, true);
      return result;
  }
  static bool isOnlyOneMatrix = false; // pouze jedna matice (jiný výpis)
  static void Main(string[] args)
    {
      const string inputFileName = "mch.in";
      using StreamReader reader = new StreamReader(inputFileName); // try, finally (volá sr.Dispose = Close) 
      int n = int.Parse(reader.ReadLine()); // n+1 matic
      if (n == 2)
      isOnlyOneMatrix = true;
      int[] matrices = new int[n]; // rozměry matic
      for (int i = 0; i < n; i++)
        matrices[i] = int.Parse(reader.ReadLine());
      Console.WriteLine("\n"+MatrixChainOrder(matrices));
    } 
}