using System;
using System.Collections.Generic;

namespace RedBlackTree;

public enum NodeColor
{
    Black, Red
}

public class Node<T>
{
    public int Key;
    public T Value;
    public NodeColor Color;
    public Node<T> Parent;
    public Node<T> Left;
    public Node<T> Right;

    public Node(int key, T value, NodeColor color)
    {
        Key = key;
        Value = value;
        Color = color;
    }

    public Node<T> GetSibling()
    {
        if (Parent == null) throw new NullReferenceException("Parent was null");
        return Parent.Left == this ? Parent.Right : Parent.Left;
    }
    
    public bool IsBlack() => Color == NodeColor.Black;
    public bool IsRed() => Color == NodeColor.Red;
}

public class RedBlackTree<T>
{
    public Node<T> Root;
    public static readonly Node<T> NullNode = new (-1, default, NodeColor.Black);

    public RedBlackTree()
    {
        Root = NullNode;
    }
    
    public RedBlackTree(int initialKey, T initialValue)
    {
        Root = new (initialKey, initialValue, NodeColor.Black)
        {
            Left = NullNode,
            Right = NullNode
        };
    }

    public void Delete(int key)
    {
        DeleteNode(FindNode(key, Root));
    }

    private void DeleteNode(Node<T> node)
    {
        var parent = node.Parent;

        // case 1 - leaf
        if (node.Left == NullNode && node.Right == NullNode)
        {
            Console.WriteLine("deletion case 1");

            if (node.IsBlack())
            {
                FixDoubleBlack(node);
            }
            else
            {
                Node<T> sibling = node.GetSibling();
                if (sibling != NullNode)
                {
                    sibling.Color = NodeColor.Red;
                }
            }

            if (parent.Left == node) parent.Left = NullNode;
            else parent.Right = NullNode;
            
            return;
        }
        
        // case 2 - one child
        // left exists
        if (node.Left != NullNode && node.Right == NullNode)
        {
            Console.WriteLine("deletion case 2-l");
            var child = node.Left;
            child.Parent = parent;
            
            if (parent.Left == node) parent.Left = child;
            else if (parent.Right == node) parent.Right = child;
            
            if (node.IsBlack() && child.IsBlack())
            {
                FixDoubleBlack(child);
            }
            else
            {
                child.Color = NodeColor.Black;
            }

            return;
        }
        // right exists
        if (node.Right != NullNode && node.Left == NullNode)
        {
            Console.WriteLine("deletion case 2-r");
            var child = node.Right;
            child.Parent = parent;
            
            if (parent.Left == node) parent.Left = child;
            else if (parent.Right == node) parent.Right = child;

            if (node.IsBlack() && child.IsBlack())
            {
                FixDoubleBlack(child);
            }
            else
            {
                child.Color = NodeColor.Black;
            }

            return;
        }
        
        // case 3 - two children
        Console.WriteLine("deletion case 3");
        
        Node<T> successor = node.Right;
        while (successor.Left != NullNode)
        {
            successor = successor.Left;
        }

        node.Key = successor.Key;
        node.Value = successor.Value;
        
        DeleteNode(successor);
    }

    private void FixDoubleBlack(Node<T> node)
    {
        if (node == Root) return;
        
        var sibling = node.GetSibling();
        var parent = node.Parent;

        if (sibling == NullNode)
        {
            FixDoubleBlack(parent);
        }
        else
        {
            if (sibling.IsRed())
            {
                parent.Color = NodeColor.Red;
                sibling.Color = NodeColor.Black;
                if (sibling == sibling.Parent.Left)
                {
                    RotateRight(parent);
                }
                else
                {
                    RotateLeft(parent);
                }
                FixDoubleBlack(node);
            }
            else
            {
                if (sibling.Left.IsRed() || sibling.Right.IsRed())
                {
                    if (sibling.Left != NullNode && sibling.Left.IsRed())
                    {
                        if (sibling.Parent.Left == sibling)
                        {
                            sibling.Left.Color = sibling.Color;
                            sibling.Color = parent.Color;
                            RotateRight(parent);
                        }
                        else
                        {
                            sibling.Left.Color = parent.Color;
                            RotateRight(sibling);
                            RotateLeft(parent);
                        }
                    }
                    else
                    {
                        if (sibling.Parent.Left == sibling)
                        {
                            sibling.Right.Color = parent.Color;
                            RotateLeft(sibling);
                            RotateRight(parent);
                        }
                        else
                        {
                            sibling.Right.Color = sibling.Color;
                            sibling.Color = parent.Color;
                            RotateLeft(parent);
                        }
                    }

                    parent.Color = NodeColor.Black;
                }
                else
                {
                    sibling.Color = NodeColor.Red;
                    if (parent.IsBlack())
                    {
                        FixDoubleBlack(parent);
                    }
                    else
                    {
                        parent.Color = NodeColor.Black;
                    }
                }
            }
        }
    }

    public T Get(int key)
    {
        return FindNode(key, Root).Value;
    }

    public void Set(int key, T value)
    {
        FindNode(key, Root).Value = value;
    }

    private Node<T> FindNode(int key, Node<T> current)
    {
        while (true)
        {
            if (current == NullNode)
            {
                throw new KeyNotFoundException();
            }

            if (current.Key == key)
            {
                return current;
            }

            current = current.Key < key ? current.Right : current.Left;
        }
    }

    private void FixInsert(Node<T> k)
    {
        while (k != Root && k.Parent.Color == NodeColor.Red)
        {
            if (k.Parent == k.Parent.Parent.Left)
            {
                Node<T> uncle = k.Parent.Parent.Right;
                if (uncle.Color == NodeColor.Red)
                {
                    k.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    k.Parent.Parent.Color = NodeColor.Red;
                    k = k.Parent.Parent;
                }
                else
                {
                    if (k == k.Parent.Right)
                    {
                        k = k.Parent;
                        RotateLeft(k);
                    }
                    k.Parent.Color = NodeColor.Black;
                    k.Parent.Parent.Color = NodeColor.Red;
                    RotateRight(k.Parent.Parent);
                }
            }
            else
            {
                Node<T> uncle = k.Parent.Parent.Left;
                if (uncle.Color == NodeColor.Red)
                {
                    k.Parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    k.Parent.Parent.Color = NodeColor.Red;
                    k = k.Parent.Parent;
                }
                else
                {
                    if (k == k.Parent.Left)
                    {
                        k = k.Parent;
                        RotateRight(k);
                    }
                    k.Parent.Color = NodeColor.Black;
                    k.Parent.Parent.Color = NodeColor.Red;
                    RotateLeft(k.Parent.Parent);
                }
            }
        }

        Root.Color = NodeColor.Black;
    }

    public void Insert(int key, T value)
    {
        Node<T> newNode = new (key, value, NodeColor.Red)
        {
            Left = NullNode,
            Right = NullNode
        };

        if (Root == NullNode || Root is null)
        {
            newNode.Color = NodeColor.Black;
            Root = newNode;
            return;
        }

        Node<T> current = Root;
        Node<T> parent = null;

        while (current != NullNode)
        {
            parent = current;
            if (newNode.Key < current.Key)
            {
                current = current.Left;
            }
            else if (newNode.Key > current.Key)
            {
                current = current.Right;
            }
            else
            {
                throw new Exception($"Key {newNode.Key} is already present in the tree");
            }
        }
        
        newNode.Parent = parent;

        if (parent == null)
        {
            Root = newNode;
        }
        else if (newNode.Key < parent.Key)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }

        if (newNode.Parent == null)
        {
            newNode.Color = NodeColor.Black;
            return;
        }

        if (newNode.Parent.Parent == null)
        {
            return;
        }

        FixInsert(newNode);
    }

    private void RotateRight(Node<T> node)
    {
        var child = node.Left;
        node.Left = child.Right;

        if (child.Right != NullNode)
        {
            child.Right.Parent = node;
        }
        
        child.Parent = node.Parent;

        if (node.Parent == null)
        {
            Root = child;
        }
        else if (node == node.Parent.Right)
        {
            node.Parent.Right = child;
        }
        else
        {
            node.Parent.Left = child;
        }
        
        child.Right = node;
        node.Parent = child;
    }

    private void RotateLeft(Node<T> node)
    {
        var child = node.Right;
        node.Right = child.Left;
        
        if (child.Left != NullNode)
        {
            child.Left.Parent = node;
        }
        
        child.Parent = node.Parent;

        if (node.Parent == null)
        {
            Root = child;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = child;
        }
        else
        {
            node.Parent.Right = child;
        }
        
        child.Left = node;
        node.Parent = child;
    }
}