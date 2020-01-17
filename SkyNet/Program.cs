using System;
using System.Collections.Generic;

class MainClass
{
    #region Just setup and random useless(for you) methods  
    public static int BestMove = -999;
    public static int TheMove = 1;
    public static void Delete(bool[] stones)
    {
        int stRowCount = 0;
        for (int i = 0; i < stones.Length; ++i)
        {
            if (stones[i])
            {
                ++stRowCount;
                for (int j = i; j <= i + (3 - stRowCount) && j < stones.Length; ++j)
                {
                    if (!stones[j])
                        for (int o = i; o < j; ++o)
                            stones[o] = false;
                }
            }
            else
                stRowCount = 0;
        }
        stRowCount = 0;
        for (int i = stones.Length-1; i > stones.Length - 4; --i)
            if (stones[i])
                ++stRowCount;
        if(stRowCount != 3)
            for (int i = stones.Length - 1; i > stones.Length - 4; --i)
                stones[i] = false;
    }

    public static void Print(bool[] stones)
    {
        Console.WriteLine();
        foreach (bool st in stones)
        {
            if (st)
                Console.Write("0");
            else
                Console.Write(" ");
        }
        Console.WriteLine();
    }

    public static bool Check(bool[] stones)
    {
        bool end = true;
        int stInRow = 0;

        foreach (bool st in stones)
        {
            if (st)
            {
                ++stInRow;
                if (stInRow == 3)
                {
                    end = false;
                    goto BreakLoops;
                }
            }
            else
                stInRow = 0;
        }

    BreakLoops:
        return end;
    }

    #endregion 

    public static bool[] Player(bool[] stones)
    {
        int n = 0;
        do
        {
            Console.WriteLine("It's your turn, choose the stone");
        OhShHereWeGoAgain:
            try
            {
                n = Convert.ToInt16(Console.ReadLine()) - 1;
            }
            catch
            {
                goto OhShHereWeGoAgain;
            }
            if (n < stones.Length && n > -1)
            {
                int stCount = 0;
                int stRowCount = 0;
                for (int i = 0; i < stones.Length; ++i)
                {
                    if (stones[i])
                    {
                        ++stRowCount;
                        ++stCount;
                        if (n + 1 == stCount)
                        {
                            for(int j = i; j <= i+(3 - stRowCount) && j < stones.Length; ++j)
                            {
                                if (!stones[j])
                                    stRowCount = 0;

                            }

                            if (stRowCount != 0)
                            {
                                stones[i] = false;
                                n = -999;
                                goto TheEnd;
                            }
                        }
                    }
                    else
                        stRowCount = 0;
                }
            }
                Console.WriteLine("Noooo, you can't choose this stone, try one more time");
                n = 0;
            Print(stones);
        } while (n > -1);

        TheEnd:
        Print(stones);
        return stones;
    }

    public static int Skynet(bool[] stones, int value, bool AI)
    {
        List<int> count = new List<int>();
        bool[] newStones = new bool[stones.Length];

        for (int j = 0; j < stones.Length; ++j)
            newStones[j] = stones[j];

        Delete(newStones);
        for (int j = 0; j < newStones.Length; ++j)
            if (newStones[j])
                count.Add(j);

        for (int i = 0; i < count.Count; ++i)
        {
            for (int j = 0; j < stones.Length; ++j)
                newStones[j] = stones[j];

            newStones[count[i]] = false;

            if (Check(newStones))
            {
                if (AI)
                    --value;
                else
                    ++value;
            }
            else
            {
                Delete(newStones);
                value = Skynet(newStones, 0, !AI);
                if (value > BestMove)
                {
                    BestMove = value;
                    TheMove = count[i];
                }
            }
        }

        return value;
    }

    public static void Main(string[] args)
    {
        //The count of stones
        int n = 14;

        bool[] stones = new bool[n];
        for (uint i = 0; i < n; ++i)
            stones[i] = true;

        Print(stones);
        while (true)
        {
            Player(stones);
            if (Check(stones))
            {
                Console.WriteLine("LOL, you lose");
                goto TheEnd;
            }

            BestMove = -999;
            List<int> count = new List<int>();
            bool[] newStones = new bool[stones.Length];

            for (int j = 0; j < stones.Length; ++j)
                newStones[j] = stones[j];

            Delete(newStones);

            for (int i = 0; i < newStones.Length; ++i)
                if (newStones[i])
                {
                    count.Add(i);
                    goto end;
                }
            end:
            TheMove = count[0];

            Skynet(stones, 0, true);
            stones[TheMove] = false;
            Print(stones);
            if (Check(stones))
            {
                Console.WriteLine("Skynet has been defeated");
                goto TheEnd;
            }

        }

    TheEnd:;
    }
}