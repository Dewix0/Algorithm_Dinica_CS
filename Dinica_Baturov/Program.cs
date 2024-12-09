using System;
using System.Collections.Generic;

public class DinicAlgorithm
{
    private int vertices; // количество вершин в графе
    private int[,] capacity; // матрица для хранения вместимости ребер

    // Конструктор класса
    public DinicAlgorithm(int vertices)
    {
        this.vertices = vertices; // инициализируем количество вершин
        capacity = new int[vertices, vertices]; // инициализируем матрицу смежности для хранения вместимости между узлами
    }

    // Метод для нахождения увеличивающего пути с использованием BFS
    private bool BFS(int source, int sink, int[] parent)
    {
        bool[] visited = new bool[vertices]; // массив для отслеживания посещенных узлов
        Queue<int> queue = new Queue<int>(); // очередь для обхода узлов в ширину
        queue.Enqueue(source); // добавляем источник в очередь
        visited[source] = true; // отмечаем источник как посещенный

        while (queue.Count > 0)
        {
            int u = queue.Dequeue(); // извлекаем первый узел из очереди

            // Проверяем все соседние узлы
            for (int v = 0; v < vertices; v++)
            {
                // Если узел не посещен и есть возможность передачи потока
                if (!visited[v] && capacity[u, v] > 0)
                {
                    parent[v] = u; // сохраняем путь от v к u
                    if (v == sink) // если достигнут сток
                        return true; // путь найден
                    queue.Enqueue(v); // добавляем узел v в очередь
                    visited[v] = true; // отмечаем узел v как посещенный
                }
            }
        }

        return false; // путь не найден
    }

    // Метод для нахождения максимального потока в сети
    public int MaxFlow(int source, int sink)
    {
        int[] parent = new int[vertices]; // массив для хранения пути от истока до стока
        int max_flow = 0; // переменная для хранения максимального потока

        // Пока существует увеличивающий путь от истока до стока
        while (BFS(source, sink, parent))
        {
            // Найти минимальный поток на найденном пути (bottleneck capacity)
            int path_flow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                path_flow = Math.Min(path_flow, capacity[u, v]); // минимальное значение пропускной способности на пути
            }

            // Обновить потоки в сети
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                capacity[u, v] -= path_flow; // уменьшить поток обратно
                capacity[v, u] += path_flow; // увеличить поток вперед
            }

            max_flow += path_flow; // добавить поток к общему значению
        }

        return max_flow; // вернуть максимальный поток
    }

    // Метод для добавления ребра с определенной пропускной способностью
    public void AddEdge(int from, int to, int cap)
    {
        capacity[from, to] += cap; // добавляем поток между узлами с указанной пропускной способностью
    }
}

public class Program
{
    public static void Main()
    {
        // Тест 1: Простой граф
        Console.WriteLine("Тест 1: Простой граф");
        DinicAlgorithm graph1 = new DinicAlgorithm(4);
        graph1.AddEdge(0, 1, 10);
        graph1.AddEdge(0, 2, 5);
        graph1.AddEdge(1, 2, 15);
        graph1.AddEdge(1, 3, 10);
        graph1.AddEdge(2, 3, 10);

        int maxFlow1 = graph1.MaxFlow(0, 3);
        Console.WriteLine($"Ожидаемый результат: 15, Реальный результат: {maxFlow1}");
        Console.WriteLine(maxFlow1 == 15 ? "Тест пройден" : "Тест провален");

        // Тест 2: Граф с несколькими путями
        Console.WriteLine("\nТест 2: Граф с несколькими путями");
        DinicAlgorithm graph2 = new DinicAlgorithm(6);
        graph2.AddEdge(0, 1, 16);
        graph2.AddEdge(0, 2, 13);
        graph2.AddEdge(1, 2, 10);
        graph2.AddEdge(1, 3, 12);
        graph2.AddEdge(2, 4, 14);
        graph2.AddEdge(3, 5, 20);
        graph2.AddEdge(4, 5, 4);

        int maxFlow2 = graph2.MaxFlow(0, 5);
        Console.WriteLine($"Ожидаемый результат: 23, Реальный результат: {maxFlow2}");
        Console.WriteLine(maxFlow2 == 16 ? "Тест пройден" : "Тест провален");

        // Тест 3: Граф без доступного пути
        Console.WriteLine("\nТест 3: Граф без доступного пути");
        DinicAlgorithm graph3 = new DinicAlgorithm(4);
        graph3.AddEdge(0, 1, 10);
        graph3.AddEdge(2, 3, 5);

        int maxFlow3 = graph3.MaxFlow(0, 3);
        Console.WriteLine($"Ожидаемый результат: 0, Реальный результат: {maxFlow3}");
        Console.WriteLine(maxFlow3 == 0 ? "Тест пройден" : "Тест провален");

        // Тест 4: Полносвязный граф
        Console.WriteLine("\nТест 4: Полносвязный граф");
        DinicAlgorithm graph4 = new DinicAlgorithm(4);
        graph4.AddEdge(0, 1, 20);
        graph4.AddEdge(0, 2, 10);
        graph4.AddEdge(1, 2, 5);
        graph4.AddEdge(1, 3, 10);
        graph4.AddEdge(2, 3, 15);

        int maxFlow4 = graph4.MaxFlow(0, 3);
        Console.WriteLine($"Ожидаемый результат: 25, Реальный результат: {maxFlow4}");
        Console.WriteLine(maxFlow4 == 25 ? "Тест пройден" : "Тест провален");

        // Тест 5: Самопетли (должны игнорироваться)
        Console.WriteLine("\nТест 5: Самопетли");
        DinicAlgorithm graph5 = new DinicAlgorithm(3);
        graph5.AddEdge(0, 1, 10);
        graph5.AddEdge(1, 2, 5);
        graph5.AddEdge(1, 1, 15); // Самопетля

        int maxFlow5 = graph5.MaxFlow(0, 2);
        Console.WriteLine($"Ожидаемый результат: 5, Реальный результат: {maxFlow5}");
        Console.WriteLine(maxFlow5 == 5 ? "Тест пройден" : "Тест провален");

        Console.WriteLine("\nВсе тесты завершены.");
    }
}
