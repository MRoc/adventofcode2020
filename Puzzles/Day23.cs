using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    // Puzzle 1: 27956483
    // Puzzle 2: 0
    public static class Day23
    {
        public static string Puzzle1()
        {
            var list = new CircularLinkedList();
            foreach (var c in "469217538")
            {
                list.Add(new Node(c));
            }

            foreach (var _ in Enumerable.Range(1, 100))
            {
                var current = list.First();
                var pick = list.Skip(1).Take(3).ToArray();
                var next = list.Skip(4).First();

                var destination = current.Value.Dec();
                while (pick.Contains(list[destination]))
                {
                    destination = destination.Dec();
                }

                list.Remove(pick);
                list.Insert(pick, list[destination]);
                list.Start = next;
            }

            list.Start = list['1'];
            return list.Skip(1).Select(n => n.Value.ToString()).Aggregate((a, b) => a + b);
        }

        public static long Puzzle2()
        {
            return 0;
        }

        private static char Dec(this char c)
        {
            c = (char)(c - 1);
            if (c == '0')
            {
                c = '9';
            }
            return c;
        }

        private class Node
        {
            public Node(char value)
            {
                Value = value;
            }

            public char Value { get; }
            public Node Prev { get; set; }
            public Node Next { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        private class CircularLinkedList : IEnumerable<Node>
        {
            public Node Start { get; set; }

            public Node this[char value] => this.First(n => n.Value == value);

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

            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var node in this)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append(node.Value);
                }

                return sb.ToString();
            }
        }
    }
}
