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
        int vertices = 6;
        DinicAlgorithm graph = new DinicAlgorithm(vertices);

        // Пример добавления ребер с определенной пропускной способностью
        graph.AddEdge(0, 1, 16);
        graph.AddEdge(0, 2, 13);
        graph.AddEdge(1, 2, 10);
        graph.AddEdge(1, 3, 12);
        graph.AddEdge(2, 1, 4);
        graph.AddEdge(2, 4, 14);
        graph.AddEdge(3, 2, 9);
        graph.AddEdge(3, 5, 20);
        graph.AddEdge(4, 3, 7);
        graph.AddEdge(4, 5, 4);

        Console.WriteLine("Максимальный поток: " + graph.MaxFlow(0, 5)); // Выводим максимальный поток от узла 0 к узлу 5
    }
}
