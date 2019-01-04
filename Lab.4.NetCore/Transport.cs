using System;
using System.Collections.Generic;
using System.Text;

namespace Lab._4.NetCore
{
    class TransportTask
    {
    int[,] tariffs;
    int[] sellers;
    int[] consumers;

    int[,] newTariffs;

    public TransportTask(int[,] tariffs, int[] sellers, int[] consumers)
    {
        this.tariffs = tariffs;
        this.sellers = sellers;
        this.consumers = consumers;

        this.newTariffs = new int[tariffs.GetLength(0), tariffs.GetLength(1)];
    }


    public int[,] Calculate()
    {
        int m = tariffs.GetLength(0), n = tariffs.GetLength(1);

        bool[,] used = new bool[m, n];

        while (!IsWaste(sellers) || !IsWaste(consumers))
        {
            int minRow, minCol;
            GetMinIndex(out minRow, out minCol);

            int min = Math.Min(sellers[minRow], consumers[minCol]);
            newTariffs[minRow, minCol] = min;
            sellers[minRow] -= min;
            consumers[minCol] -= min;
        }

        int idxRow = -1, idxCol = -1;

        while (!CheckOptimal(ref idxRow, ref idxCol))
        {
            int idxRow2 = 0, idxCol2 = 0;
            for (int i = 0; i < m; i++)
            {
                if (i != idxRow)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (j != idxCol && newTariffs[i, j] != 0 && newTariffs[idxRow, j] != 0 && newTariffs[i, idxCol] != 0)
                        {
                            idxRow2 = i;
                            idxCol2 = j;
                        }
                    }
                }
            }

            int min = Math.Min(newTariffs[idxRow, idxCol2], newTariffs[idxRow2, idxCol]);

            newTariffs[idxRow, idxCol2] -= min;
            newTariffs[idxRow2, idxCol] -= min;
            newTariffs[idxRow, idxCol] += min;
            newTariffs[idxRow2, idxCol2] += min;
            idxRow = idxCol = -1;
        }

        return newTariffs;
    }

    public int GetFuncResult()
    {

        int sum = 0;
        for (int i = 0; i < newTariffs.GetLength(0); i++)
        {
            for (int j = 0; j < newTariffs.GetLength(1); j++)
            {
                sum += tariffs[i, j] * newTariffs[i, j];
            }
        }

        return sum;
    }

    void GetMinIndex(out int minRow, out int minCol)
    {
        minRow = minCol = 0;

        int m = tariffs.GetLength(0), n = tariffs.GetLength(1);

        bool first = true;

        for (int i = 0; i < m; i++)
        {
            if (sellers[i] != 0)
            {
                for (int j = 0; j < n; j++)
                {
                    if (consumers[j] != 0)
                    {
                        if (first)
                        {
                            minRow = i;
                            minCol = j;
                            first = !first;
                        }
                        else if (tariffs[minRow, minCol] > tariffs[i, j] && tariffs[i, j] > 0)
                        {
                            minRow = i;
                            minCol = j;
                        }

                    }
                }
            }
        }

        return;
    }

    bool IsWaste(int[] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            if (array[i] > 0) return false;
        }
        return true;
    }

    bool CheckOptimal(ref int idxRow, ref int idxCol)
    {
        int m = tariffs.GetLength(0), n = tariffs.GetLength(1);

        int[] u = new int[m];
        int[] v = new int[n];

        for (int i = 1; i < m; i++)
        {
            u[i] = Int32.MinValue;
        }

        for (int i = 0; i < n; i++)
        {
            v[i] = Int32.MinValue;
        }

        for (int i = 0; i < m; i++) //вычисление потенциалов
        {
            for (int j = 0; j < n; j++)
            {
                if (newTariffs[i, j] > 0)
                {
                    if (u[i] != Int32.MinValue && v[j] == Int32.MinValue)
                    {
                        v[j] = tariffs[i, j] - u[i];
                        i = -1;
                        break;
                    }

                    if (u[i] == Int32.MinValue && v[j] != Int32.MinValue)
                    {
                        u[i] = tariffs[i, j] - v[j];
                        i = -1;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < m; i++) //проверка оптимальности
        {
            for (int j = 0; j < n; j++)
            {
                if (newTariffs[i, j] == 0 && u[i] + v[j] > tariffs[i, j])
                {
                    if (idxRow == -1)
                    {
                        idxRow = i;
                        idxCol = j;
                    }
                    else if (tariffs[i, j] > tariffs[idxRow, idxCol])
                    {
                        idxRow = i;
                        idxCol = j;
                    }
                }

            }
        }
        return idxRow == -1;
    }
}
}
