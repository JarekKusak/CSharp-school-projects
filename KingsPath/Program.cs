using System;
using System.Collections.Generic;

namespace Kral
{
    class Position // třída na ukládání pozic s předchůdci
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int PredchudceX { get; set; }
        public int PredchudceY { get; set; }

        public Position(int x, int y, int predchudceX, int predchudceY)
        {
            X = x;
            Y = y;
            PredchudceX = predchudceX;
            PredchudceY = predchudceY;
        }
    }

    internal class Program
    {
        const int NON_VISITED = -1;
        private static void Main(string[] args)
        {
            int r = 10; // velikost řádku (dva řádky šachovnice navíc na hranici)
            int s = 10; // velikost sloupce (dva řádky šachovnice navíc na hranici)
            int[,] sachovnice = new int[s, r]; // šachovnice 8x8 je ohraničena řádkem a sloupcem navíc 

            NastavSachovnici(sachovnice, r, s);
            string line;
            int pocetPrekazek = int.Parse(Console.ReadLine());
            for (int i = 0; i < pocetPrekazek; i++)
            {
                // na vstupu (na jednom řádku) zadá uživatel souřadnice x, y překážky (rozsah [1,8])
                // (není ošetřený vstup)
                line = Console.ReadLine(); // souř. startovacího políčka
                var prekazka = line.Split(' ');
                var prekazkax = int.Parse(prekazka[0]);
                var prekazkay = int.Parse(prekazka[1]);  // vytvoří pole z dvou zadaných hodnot na jednom řádku
                sachovnice[prekazkax, prekazkay] = -2; // nastaví překážku
            }
            line = Console.ReadLine(); // souř. startovacího políčka
            var start = line.Split(' ');
            var startx = int.Parse(start[0]);
            var starty = int.Parse(start[1]);

            sachovnice[startx, starty] = 0;

            line = Console.ReadLine(); // souř. startovacího políčka
            var cil = line.Split(' ');
            var cilx = int.Parse(cil[0]);
            var cily = int.Parse(cil[1]);

            List<Position> positions = new List<Position>();

            positions.Add(new Position(startx, starty, 0, 0));
            int pocetKroku = NajdiCestu(sachovnice, startx, starty, cilx, cily, positions);
            if (pocetKroku != -1)
                NajdiCestuZpet(positions, startx, starty, cilx, cily);
            else
                Console.WriteLine(pocetKroku);
            //VytiskniSachovnici(sachovnice, r, s);
        }

        static int NajdiCestu(int[,] sachovnice, int startx, int starty, int cilx, int cily, List<Position> positions)
        {
            int k = 0;
            bool done = false;
            while (!done)
            {
                done = true;
                for (int x = 1; x < 9; x++)
                {
                    for (int y = 1; y < 9; y++)
                    {
                        if (sachovnice[x, y] == k)
                        {
                            // procházení okolních 8 políček nalezeného políčka
                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    if (sachovnice[i + x, j + y] == NON_VISITED)
                                    {
                                        sachovnice[i + x, j + y] = k + 1; // vyplní šachovnici aktuální "vlnou"
                                        positions.Add(new Position(i + x, j + y, x, y)); // nynější políčko a jeho předchůdce
                                        done = false;
                                    }
                                }
                            }
                        }
                    }
                }
                k++;
            }
            return sachovnice[cilx, cily]; // pokud nenalezne, vrátí -1 (jinak hodnotu vlny, tedy kroku)
        }

        // rekurzivní funkce
        static void NajdiCestuZpet(List<Position> positions, int startx, int starty, int x, int y)       
        {
            foreach (var p in positions) // projede všechny uložené pozice
            {
                if (x == startx && y == starty) // pokud nalezneme startovací políčko -> vypiš a konec
                {
                    Console.WriteLine($"{x} {y}");
                    return;
                }
                else if (x == p.X && y == p.Y)
                {
                    // rekurzivní volání s předchůdci
                    NajdiCestuZpet(positions, startx, starty, p.PredchudceX, p.PredchudceY);
                    Console.WriteLine($"{x} {y}");
                    return;
                }
            }
        }
        
        static void NastavSachovnici(int[,] sachovnice, int r, int s)
        {
            // hranice šachovnice
            for (int i = 0; i < s; i++)
            {
                sachovnice[i, 0] = -2;
            }
            for (int i = 0; i < s; i++)
            {
                sachovnice[i, r - 1] = -2;
            }
            for (int j = 0; j < r; j++)
            {
                sachovnice[0, j] = -2;
            }
            for (int j = 0; j < r; j++)
            {
                sachovnice[s - 1, j] = -2;
            }

            // vyplnění hracího pole -1 (pro případné nenalezení cesty stačí jen navrátit cílové políčko figurky)
            for (int i = 1; i < s - 1; i++)
            {
                for (int j = 1; j < r - 1; j++)
                {
                    sachovnice[i, j] = NON_VISITED;
                }
            }
        }


        /// tisk sachovnice
        static void VytiskniSachovnici(int[,] sachovnice, int r, int s)
        {
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    Console.Write(sachovnice[j, i]);
                }
                Console.Write("\n");
            }
        }

    }
}
