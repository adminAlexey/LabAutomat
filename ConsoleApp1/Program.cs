using System;

public class Automat
{                                                       // q0   q1    q2   q3
    public static string[,] automat = new string[,] {   { "b", "a", "-", "e"}, //q0 
                                                        { "-", "-", "e", "c"}, //q1
                                                        { "-", "b", "-", "a"}, //q2
                                                        { "-", "-", "-", "c"}};//q3
    static string[] start = new string[4];
    static string[] end = new string[4];
    static string[] startLast = new string[4];
    static string[] endLast = new string[4];
    static string[,] automat4 = new string[,] { };

    public static string[,] DelEps(string[,] automat1)//удаляет епсилон переходы
    {
        string[] startStep = new string[4];
        string[] endStep = new string[4];
        startStep[0] = "0";//задаю начальную вершину
        endStep[0] = "3";//задаю конечную вершину
        string[,] automat2 = new string[automat1.GetLength(0), automat1.GetLength(1)];

        //копирование массива
        for (int i = 0; i < automat1.GetLength(0); i++)
            for (int j = 0; j < automat1.GetLength(1); j++)
                automat2[i, j] = automat1[i, j];


        //Console.WriteLine("Начальный вид матрицы");
        //Show(automat2, "q", 1);

        //вывод старых вершин
        Console.WriteLine();
        Console.WriteLine("Начальные вершины старые");
        for (int k = 0; k < startStep.Length; k++)
            if (startStep[k] != null)
                Console.WriteLine(startStep[k]);
        Console.WriteLine();
        Console.WriteLine("Конечные вершины старые");
        for (int k = 0; k < endStep.Length; k++)
            if (endStep[k] != null)
                Console.WriteLine(endStep[k]);
        Console.WriteLine();

        string[] arr = new string[4];
        for (int i = 0; i < automat2.GetLength(0); i++)
        {
            int count = 0;
            for (int j = 0; j < automat2.GetLength(1); j++)
                if (automat2[i, j] == "e")
                {
                    Console.WriteLine($"S{i} = q{i}, q{j}");
                    count++;
                    arr[i] = i.ToString() + j.ToString();
                }
            if (count == 0)
            {
                Console.WriteLine($"S{i} = q{i}");
                arr[i] = i.ToString();
            }
        }
        
        for (int i = 0; i < automat1.GetLength(0); i++)
            for (int j = 0; j < automat1.GetLength(1); j++)
            {
                //здесь реализуем условие если есть эпсилон переход, то сложить строки
                if (automat1[i, j] == "e")
                    for (int k = 0; k < automat1.GetLength(1); k++)
                        automat2[i, k] += automat1[j, k];
                if (i.ToString() == startStep[0])
                    startStep[1] = j.ToString();

                //здесь реализуем условие если следовало в вершину, а там есть эпсилон переход, то добавляем вершину
                if (automat1[i, j] != "e" && automat1[i, j] != "-")
                    for (int k = 0; k < automat1.GetLength(1); k++)
                        if (automat1[j, k] == "e")
                            automat2[i, k] += automat1[i, j];

                //здесь реализуем последнее условие
                if (automat1[i, j] == "e")
                    automat2[i, j] += automat1[j, i];
            }

        for (int i = 0; i < automat2.GetLength(0); i++)
            for (int j = 0; j < automat2.GetLength(1); j++)
            {
                automat2[i, j] = automat2[i, j].Replace("-", "");
                automat2[i, j] = automat2[i, j].Replace("e", "");
                if (automat2[i, j] == "")
                    automat2[i, j] += "-";
            }
        
        //Console.WriteLine("Матрица без е-переходов");
        //Show(automat2, "q", 1);

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < startStep.Length; j++)
                if (startStep[j] != null && arr[i].IndexOf(startStep[j]) != -1)
                    start[i] += i;
            for (int j = 0; j < endStep.Length; j++)
            {
                if (endStep[j] != null && arr[i].IndexOf(endStep[j]) != -1)
                    end[i] += i;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Новые начальные");
        for (int i = 0; i < start.Length; i++)
            if (start[i] != null)
                Console.WriteLine(i);
        Console.WriteLine();
        Console.WriteLine("Новые конечные");
        for (int i = 0; i < start.Length; i++)
            if (end[i] != null)
                Console.WriteLine(i);
        //**************************

        return automat2;
    }

    static void Show(string[,] aut, string s, int type)
    {
        if (type == 1)
        {
            Console.Write($"\t{s}0\t{s}1\t{s}2\t{s}3");
        }
        else
        {
            Console.Write($"\ta\tb\tc");
        }
        
        Console.WriteLine();
        for (int i = 0; i < aut.GetLength(0); i++)
        {
            Console.Write($"{s}{i}\t");
            for (int j = 0; j < aut.GetLength(1); j++)
            {
                Console.Write(aut[i, j] + "\t");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static string[,] Determination(string[,] automat1, string[] start, string[] end, string[] startLast, string[] endLast)
    {
        // перевожу в нормальный вид
        string[,] automat3 = new string[automat1.GetLength(0), 3];
        for (int i = 0; i < automat1.GetLength(0); i++)
            for (int j = 0; j < automat1.GetLength(1); j++)
            {
                if (automat1[i, j].IndexOf("a") >= 0)
                    automat3[i, 0] += ("S" + j).ToString();
                if (automat1[i, j].IndexOf("b") >= 0)
                    automat3[i, 1] += ("S" + j).ToString();
                if (automat1[i, j].IndexOf("c") >= 0)
                    automat3[i, 2] += ("S" + j).ToString();
            }
        //вывел в другом виде
        Show(automat3, "S", 2);

        List<string> P = new List<string>();

        for (int i = 0; i < automat3.GetLength(0); i++)
            for (int j = 0; j < automat3.GetLength(1); j++)
                if (automat3[i, j] != "")
                    P.Add(automat3[i, j]);
        

        P = P.Distinct().ToList();
        P.RemoveAll(s => string.IsNullOrEmpty(s));
        string[] arr = P.ToArray();

        for (int i = 0; i < P.Count; i++)
        {
            Console.WriteLine($"P{i} = " + "{" + $"{P[i]}" + "}");
            for (int j = 0; j < start.Length; j++)
            {
                if (start[j] != null && arr[i].IndexOf(start[j]) >= 0)
                    startLast[i] += j; 
                if (end[j] != null && arr[i].IndexOf(end[j]) >= 0)
                    endLast[i] += j;
            }
        }

        automat4 = new string[P.Count, 3];
        //automat3 это предыдущая таблица
        //P это новые символы

        string[,] mas = new string[arr.Length, 3]; 
        for (int i = 0; i < arr.GetLength(0); i++)
            if (arr[i].Length > 2)
            {
                mas[i, 0] = arr[i][..2];
                mas[i, 1] = arr[i][2..];
            }
            else
                mas[i, 0] = arr[i];

        Console.WriteLine();

        for (int i = 0; i < mas.GetLength(0); i++)
            for (int j = 0; j < mas.GetLength(1); j++)
            {
                if (mas[i, j] == "S0")
                    for (int k = 0; k < arr.GetLength(0); k++)
                        if (automat3[0, k] == P[0])
                            automat4[i, k] = "P0";
                        else if (automat3[0, k] == P[1])
                            automat4[i, k] = "P1";
                        else if (automat3[0, k] == P[2])
                            automat4[i, k] = "P2";
                if (mas[i, j] == "S1")
                    for (int k = 0; k < arr.GetLength(0); k++)
                        if (automat3[1, k] == P[0])
                            automat4[i, k] = "P0";
                        else if (automat3[1, k] == P[1])
                            automat4[i, k] = "P1";
                        else if (automat3[1, k] == P[2])
                            automat4[i, k] = "P2";
                if (mas[i, j] == "S2")
                    for (int k = 0; k < arr.GetLength(0); k++)
                        if (automat3[2, k] == P[0])
                            automat4[i, k] = "P0";
                        else if (automat3[2, k] == P[1])
                            automat4[i, k] = "P1";
                        else if (automat3[2, k] == P[2])
                            automat4[i, k] = "P2";
                if (mas[i, j] == "S3")
                    for (int k = 0; k < arr.GetLength(0); k++)
                        if (automat3[3, k] == P[0])
                            automat4[i, k] = "P0";
                        else if (automat3[3, k] == P[1])
                            automat4[i, k] = "P1";
                        else if (automat3[3, k] == P[2])
                            automat4[i, k] = "P2";
            }

        Console.WriteLine("Детерменированный конечный автомат");
        Show(automat4, "P", 2);
        return automat4;
    }
 
    static void Allowed(string str, string[,] automat, string[] startLast, string[] endLast)
    {
        char first = str[0];
        string start = "";
        for (int i = 0; i < automat.GetLength(0); i++)
        {
            if (first == 'a')
            {
                for (int j = 0; j < startLast.Length; j++)
                    if (startLast[j] != null && automat[i, 0] != null && automat[i, 0].IndexOf(j.ToString()) >= 0)
                        start += j.ToString();
            }
            else if (first == 'b')
            {
                for (int k = 0; k < startLast.Length; k++)
                    if (startLast[k] != null && automat[i, 1] != null && automat[i, 1].IndexOf(k.ToString()) >= 0)
                        start += k.ToString();
            }
            else if (first == 'c')
            {
                for (int l = 0; l < startLast.Length; l++)
                    if (startLast[l] != null && automat[i, 2] != null && automat[i, 2].IndexOf(l.ToString()) >= 0)
                        start += l.ToString();
            }
            else
                Console.WriteLine("Первый символ строки не является началом допустимой цепочки");
        }

        string end = "";
        start = new string (start.Distinct().ToArray());
        Console.WriteLine("Старт: " + start);
        start = "12";
        if (str.Length == 1)
        {
            for (int i = 0; i < endLast.Length; i++)
                if (start.IndexOf(i.ToString()) >= 0 && endLast[i] != null)
                {
                    Console.WriteLine("Цепочка состоит из 1 символа, автомат допускает ее (входящая вершина совпадает с конечной)");
                    break;
                }
        }
        else 
        {   
            for (int i = 1; i < str.Length; i++)
            {
                string newStart = "";
                for (int j = 0; j < start.Length; j++)
                {
                    int k = 0;
                    if (start[j] == '0')
                        k = 0;
                    else if (start[j] == '1')
                        k = 1;
                    else if (start[j] == '2')
                        k = 2;
                    //str[i] - порядковая буква в слове
                    //start[j] - автомат из списка доступных
                    if (str[i] == 'a')
                    {
                        if (automat[k, 0] != null)
                        {
                            if (automat[k, 0].IndexOf("0") != -1)
                                newStart += "0";
                            else if (automat[k, 0].IndexOf("1") != -1)
                                newStart += "1";
                            else if (automat[k, 0].IndexOf("2") != -1)
                                newStart += "2";
                        }
                    }
                    else if (str[i] == 'b')
                    {
                        if (automat[k, 1] != null)
                        {
                            if (automat[k, 1].IndexOf("0") != -1)
                                newStart += "0";
                            else if (automat[k, 1].IndexOf("1") != -1)
                                newStart += "1";
                            else if (automat[k, 1].IndexOf("2") != -1)
                                newStart += "2";
                        }
                    }
                    else if (str[i] == 'c')
                    {
                        if (automat[k, 2] != null)
                        {
                            if (automat[k, 2].IndexOf("0") != -1)
                                newStart += "0";
                            else if (automat[k, 2].IndexOf("1") != -1)
                                newStart += "1";
                            else if (automat[k, 2].IndexOf("2") != -1)
                                newStart += "2";
                        }
                    }
                }
                start = new string (newStart.Distinct().ToArray());
                for (int m = 0; m < start.Length; m++)
                    Console.Write(start[m] + " ");
                Console.WriteLine();
            }
            end = start;
        }
        int count = 0;
        for (int i = 0; i < endLast.Length; i++)
        {
            if (end == endLast[i])
            {
                count++;
            }
        }
        if (count > 0)
            Console.WriteLine("\nПоследний автомат" + end);
        else
            Console.WriteLine("\nЦепочка не допускается автоматом" + end);
    }

    public static void Main() 
    {
        string[,] automat5 = Determination(DelEps(automat), start, end, startLast, endLast);

        Console.WriteLine();
        Console.WriteLine("Начальными вершинами являются: ");
        for (int i = 0; i < startLast.Length; i++)
            if (endLast[i] != null)
                Console.WriteLine(i);
        Console.WriteLine();
        Console.WriteLine("Конечными вершинами являются: ");
        for (int i = 0; i < endLast.Length; i++)
            if (endLast[i] != null)
                Console.WriteLine(i);

        Console.WriteLine();
        Console.WriteLine("Введите цепочку, для проверки");
        Allowed(Console.ReadLine(), automat5, startLast, endLast);
    }
}