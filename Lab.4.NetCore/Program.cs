using System;

namespace Lab._4.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] rariffs = {  
                { 6, 8, 14, 4 },
                { 5, 2, 2, 8 },
                { 7, 6, 7, 5 },
                { 4, 5, 12, 7 }
            };

            int[] sellers = { 25, 30, 35, 10 };
            int[] consumers = { 40, 20, 20, 20 };


            TransportTask transporTask = new TransportTask(rariffs, sellers, consumers);
            int[,] result = transporTask.Calculate();

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                    Console.Write(result[i, j] + "\t");
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("F = " + transporTask.GetFuncResult());

            Console.ReadKey();
        }
    }
}
