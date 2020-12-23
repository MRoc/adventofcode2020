using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 27956483
    // Puzzle 2: 18930983775
    public static class Day23
    {
        public static string Puzzle1()
        {
            var cups = 9;
            var moves = 100;

            var nodes = Enumerable.Range(1, cups).Select(i => new Node(i)).ToArray();
            
            var list = new CircularLinkedList(nodes);
            
            foreach (var c in "469217538")
            {
                list.Add(nodes[(int)c - (int)'0' - 1]);
            }
            
            list.RunMoves(moves, cups);
            
            list.Start = list[1];
            return list.Skip(1).Select(n => n.Value.ToString()).Aggregate((a, b) => a + b);
        }

        public static long Puzzle2()
        {
            var cups = 1000000;
            var moves = 10000000;

            var nodes = Enumerable.Range(1, cups).Select(i => new Node(i)).ToArray();
            var list = new CircularLinkedList(nodes);
            
            foreach (var c in "469217538")
            {
                list.Add(nodes[(int)c - (int)'0' - 1]);
            }
            for (int i = 10; i <= cups; ++i)
            {
                list.Add(nodes[i - 1]);
            }
            
            list.RunMoves(moves, cups);
            
            list.Start = list[1];
            
            return list
                .Skip(1)
                .Take(2)
                .Select(i => (long)i.Value)
                .Aggregate((a, b) => a * b);
        }
        
        private static void RunMoves(this CircularLinkedList list, int count, int max)
        {
            foreach (var _ in Enumerable.Range(0, count))
            {
                var current = list.First();
                var pick = list.Skip(1).Take(3).ToArray();
                var next = list.Skip(4).First();

                var destination = current.Value.Dec(max);
                while (pick.Contains(list[destination]))
                {
                    destination = destination.Dec(max);
                }

                list.Remove(pick);
                list.Insert(pick, list[destination]);
                list.Start = next;
            }
        }

        private static int Dec(this int c, int max)
        {
            c--;
            if (c == 0)
            {
                c = max;
            }
            return c;
        }

        private class Node
        {
            public Node(int value)
            {
                Value = value;
            }

            public int Value { get; }
            public Node Prev { get; set; }
            public Node Next { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        private class CircularLinkedList : IEnumerable<Node>
        {
            public CircularLinkedList(Node[] nodes)
            {
                Nodes = nodes;
            }    
            
            public Node Start { get; set; }
            
            private Node[] Nodes { get; }

            public Node this[int value] => Nodes[value - 1];

            public void Add(Node node)
            {
                if (Start is null)
                {
                    Start = node;
                    Start.Prev = node;
                    Start.Next = node;
                }
                else
                {
                    var before = Start.Prev;
                    before.Next = node;
                    node.Prev = before;
                    node.Next = Start;
                    Start.Prev = node;
                }
            }

            public void Remove(IReadOnlyCollection<Node> nodes)
            {
                var first = nodes.First();
                var last = nodes.Last();

                var prev = first.Prev;
                var next = last.Next;

                prev.Next = next;
                first.Prev = null;
                last.Next = null;
                next.Prev = prev;
            }

            public void Insert(IReadOnlyCollection<Node> nodes, Node node)
            {
                var first = nodes.First();
                var last = nodes.Last();

                var prev = node;
                var next = node.Next;

                prev.Next = first;
                first.Prev = prev;
                last.Next = next;
                next.Prev = last;
            }

            public IEnumerator<Node> GetEnumerator()
            {
                var current = Start;

                do
                {
                    yield return current;
                    current = current.Next;
                } while (current != Start);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
