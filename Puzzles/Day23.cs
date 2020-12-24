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
                list.Add(nodes[(int)c - '0' - 1]);
            }
            
            list.RunMoves(moves, cups);
            
            list.Start = list[1];
            return list.Skip(1).Select(n => n.Value.ToString()).Aggregate((a, b) => a + b);
        }

        public static long Puzzle2()
        {
            var cups = 1000000;
            var moves = 10000000;

            var nodes = new Node[cups];
            for (long i = 0; i < cups; ++i)
            {
                nodes[i] = new Node(i + 1);
            }

            var list = new CircularLinkedList(nodes);
            
            foreach (var c in "469217538")
            {
                list.Add(nodes[(long)c - '0' - 1]);
            }
            for (long i = 10; i <= cups; ++i)
            {
                list.Add(nodes[i - 1]);
            }
            
            list.RunMoves(moves, cups);
            
            list.Start = list[1];
            
            return list
                .Skip(1)
                .Take(2)
                .Select(i => i.Value)
                .Aggregate((a, b) => a * b);
        }
        
        private static void RunMoves(this CircularLinkedList list, long count, long max)
        {
            var current = default(Node);
            var pick = new Node[3];
            var next = default(Node);
            
            for (long i=0; i < count; ++i)
            {
                current = list.Start;
                pick[0] = current.Next;
                pick[1] = pick[0].Next;
                pick[2] = pick[1].Next;
                next = pick[2].Next;
                
                var destination = current.Value.Dec(max);

                while (pick.Contains(list[destination]))
                {
                    destination = destination.Dec(max);
                }

                list.Move(current, pick, list[destination]);
                list.Start = next;
            }
        }

        private static long Dec(this long c, long max)
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
            public Node(long value)
            {
                Value = value;
            }

            public long Value;
            public Node Next;
        }

        private class CircularLinkedList : IEnumerable<Node>
        {
            public CircularLinkedList(Node[] nodes)
            {
                Nodes = nodes;
            }    
            
            public Node Start { get; set; }
            
            public Node LastInsert { get; set; }
            
            private Node[] Nodes { get; }

            public Node this[long value] => Nodes[value - 1];

            public void Add(Node node)
            {
                if (Start is null)
                {
                    Start = node;
                    Start.Next = node;
                }
                else
                {
                    LastInsert.Next = node;
                    node.Next = Start;
                }

                LastInsert = node;
            }

            public void Move(Node from, Node[] nodes, Node node)
            {
                var first = nodes[0];
                var last = nodes[^1];

                from.Next = last.Next;
                last.Next = node.Next;
                node.Next = first;
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
