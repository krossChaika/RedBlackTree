using System;
using System.Collections.Generic;

namespace RedBlackTree;

class Program
{
    static void Main(string[] args)
    {
        var tree = new RedBlackTree<MusicInstrument>();
        tree.Insert(50, null);
        
        const int keysCount = 20;
        List<int> keysHistory = new(keysCount) {50};
        var random = new Random(10);
        
        for (int i = 0; i < keysCount; i++)
        {
            int newKey = random.Next(0, 100);
            while (keysHistory.Contains(newKey))
            {
                newKey = random.Next(0, 100);
            }
            
            Console.WriteLine($"added {newKey}");
            tree.Insert(newKey, new MusicInstrument(newKey, $"id-{newKey}", newKey));
            keysHistory.Add(newKey);
        }
        
        // tree.Delete(95);

        for (int i = 0; i < 10; i++)
        {
            int key = keysHistory[random.Next(0, keysHistory.Count)];
            Console.WriteLine($"deleted {key}");
            tree.Delete(key);
            keysHistory.Remove(key);
        }
        RedBlackVisualizer.Visualize(tree.Root);
        foreach (var key in keysHistory)
        {
            Console.WriteLine(key);
        }

        // tree.Set(29, "asd");
        Console.WriteLine(tree.Get(29));

        // int[] keys = [20,30,40,];//50,60,70,80
        // foreach (var key in keys)
        // {
        //     tree.Insert(key, $"string-{key}");
        // }
        // tree.Delete(30);
        // RedBlackVisualizer.Visualize(tree.Root);
        
        Console.ReadLine();
    }
}

public static class RedBlackVisualizer
{
    public static void Visualize<T>(Node<T> node, int indent = 0)
    {
        if (node is null || node == RedBlackTree<T>.NullNode) return;
        
        for (int i = 0; i < indent; i++)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(">");
        }
        
        if (node.Color == NodeColor.Black)
        {
            // Console.ForegroundColor = ConsoleColor.Black;
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        
        Console.WriteLine($"{node.Key} - {node.Color}");
        
        Console.ResetColor();
        
        Visualize(node.Left, indent + 1);
        Visualize(node.Right, indent + 1);
    }
}