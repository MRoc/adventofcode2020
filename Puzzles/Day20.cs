using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    // Puzzle 1: 16192267830719
    // Puzzle 2: 1909
    public static class Day20
    {
        public static long Puzzle1()
        {
            var tiles = LoadTiles();

            var picture = tiles.Solve();

            var minX = picture.Keys.Select(p => p.X).Min();
            var maxX = picture.Keys.Select(p => p.X).Max();

            var minY = picture.Keys.Select(p => p.Y).Min();
            var maxY = picture.Keys.Select(p => p.Y).Max();
            
            var corners = new[]
            {
                picture[new Point(minX, minY)],
                picture[new Point(minX, maxY)],
                picture[new Point(maxX, minY)],
                picture[new Point(maxX, maxY)],
            };

            return corners.Select(t =>(long) t.Id).Aggregate((a, b) => a * b);
        }
        
        public static long Puzzle2()
        {
            return LoadTiles()
                .Solve()
                .Normalize()
                .Combine()
                .CreateVariations()
                .Sum(p => p.MarkAndCountSeaMonster());
        }

        private static Tile[] LoadTiles()
        {
            return Input
                .Load("Puzzles.Input.input20.txt").Split("\n\n")
                .Where(l => !string.IsNullOrEmpty(l))
                .Select(Tile.Parse)
                .ToArray();
        }
        
        // TODO Return an tile[][]
        private static Dictionary<Point, Tile> Solve(this Tile[] tilesArray)
        {
            var tiles = tilesArray.ToList();
            
            var picture = new Dictionary<Point, Tile>();

            while (tiles.Any())
            {
                if (!picture.Any())
                {
                    var tile = tiles.First();
                    picture[new Point(0, 0)] = tile;
                    tiles.Remove(tile);
                }
                else
                {
                    foreach (var picTile in picture.ToArray())
                    {
                        // Top
                        var topPosition = new Point(picTile.Key.X, picTile.Key.Y - 1);
                        if (!picture.ContainsKey(topPosition))
                        {
                            var top = tiles.SingleOrDefault(t => t.AsAllNumbers().Contains(picTile.Value.Top));
                            if (top is { })
                            {
                                picture[topPosition] = top.Variations().Single(v => v.Bottom == picTile.Value.Top);
                                tiles.Remove(top);
                            }
                        }

                        // Right
                        var rightPosition = new Point(picTile.Key.X + 1, picTile.Key.Y);
                        if (!picture.ContainsKey(rightPosition))
                        {
                            var right = tiles.SingleOrDefault(t => t.AsAllNumbers().Contains(picTile.Value.Right));
                            if (right is { })
                            {
                                picture[rightPosition] = right.Variations().Single(v => v.Left == picTile.Value.Right);
                                tiles.Remove(right);
                            }
                        }

                        // Bottom
                        var bottomPosition = new Point(picTile.Key.X, picTile.Key.Y + 1);
                        if (!picture.ContainsKey(bottomPosition))
                        {
                            var bottom = tiles.SingleOrDefault(t => t.AsAllNumbers().Contains(picTile.Value.Bottom));
                            if (bottom is { })
                            {
                                picture[bottomPosition] = bottom.Variations().Single(v => v.Top == picTile.Value.Bottom);
                                tiles.Remove(bottom);
                            }
                        }

                        // Left
                        var leftPosition = new Point(picTile.Key.X - 1, picTile.Key.Y);
                        if (!picture.ContainsKey(leftPosition))
                        {
                            var left = tiles.SingleOrDefault(t => t.AsAllNumbers().Contains(picTile.Value.Left));
                            if (left is { })
                            {
                                picture[leftPosition] = left.Variations().Single(v => v.Right == picTile.Value.Left);
                                tiles.Remove(left);
                            }
                        }
                    }
                }
            }

            return picture;
        }

        private static Dictionary<Point, Tile> Normalize(this Dictionary<Point, Tile> picture)
        {
            var minX = picture.Keys.Select(p => p.X).Min();
            var minY = picture.Keys.Select(p => p.Y).Min();

            return picture.ToDictionary(
                i => new Point(i.Key.X - minX, i.Key.Y - minY),
                i => i.Value);
        }

        // TODO Crop data in constructor
        public record Tile(int Id, char[][] Data, int Top, int Right, int Bottom, int Left)
        {
            public static Tile Parse(string text)
            {
                var lines = text.Split('\n');

                var id = int.Parse(lines.First().Substring(5, 4));

                var data = lines
                    .Skip(1)
                    .Select(s => s.Replace('#', '1').Replace('.', '0').ToArray())
                    .ToArray();
                
                return new Tile(
                    id,
                    data,
                    Convert.ToInt32(data.First().AsString(), 2),
                    Convert.ToInt32(data.Select(l => l.Last()).AsString(), 2),
                    Convert.ToInt32(data.Last().AsString(), 2),
                    Convert.ToInt32(data.Select(l => l.First()).AsString(), 2));
            }

            public Tile FlipHorizontal()
            {
                return new Tile(
                    Id,
                    Data.FlipHorizontal(),
                    Top.Reverse10Bits(),
                    Left,
                    Bottom.Reverse10Bits(),
                    Right);
            }

            public Tile RotateRight90()
            {
                return new Tile(
                    Id,
                    Data.RotateRight90(),
                    Left.Reverse10Bits(), 
                    Top, 
                    Right.Reverse10Bits(),
                    Bottom);
            }

            public IEnumerable<int> AsNumbers()
            {
                return new[] {Top, Right, Bottom, Left};
            }
            
            public IEnumerable<int> AsAllNumbers()
            {
                return AsNumbers().Concat(AsNumbers().Select(n => n.Reverse10Bits()));
            }

            public IEnumerable<Tile> Variations()
            {
                yield return this;
                yield return this.RotateRight90();
                yield return this.RotateRight90().RotateRight90();
                yield return this.RotateRight90().RotateRight90().RotateRight90();

                yield return this.FlipHorizontal();
                yield return this.FlipHorizontal().RotateRight90();
                yield return this.FlipHorizontal().RotateRight90().RotateRight90();
                yield return this.FlipHorizontal().RotateRight90().RotateRight90().RotateRight90();
            }

            public char[][] Crop()
            {
                return Data
                    .Skip(1)
                    .Take(Data.Length - 2)
                    .Select(r => r.Skip(1).Take(Data.Length - 2).ToArray())
                    .ToArray();
            }
        }

        record Point(int X, int Y)
        {
        }

        private static string AsString(this IEnumerable<char> chars)
        {
            var sb = new StringBuilder();
            foreach (var c in chars)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        private static int Reverse10Bits(this int value)
        {
            return Enumerable
                .Range(0, 10)
                .Select(i => ((1 << i) & value) != 0)
                .Reverse()
                .Select((b, i) => b ? (1 << i) : 0)
                .Aggregate((a, b) => a | b);
        }
        
        private static char[][] Combine(this Dictionary<Point, Tile> picture)
        {
            var maxX = picture.Keys.Select(p => p.X).Max();
            var maxY = picture.Keys.Select(p => p.Y).Max();

            var d = picture.First().Value.Data.Length - 2;

            var width = (maxX + 1) * d;
            var height= (maxY + 1) * d;

            var result = new char[height][];
            for (int i = 0; i < height; ++i)
            {
                result[i] = new char[width];
            }
            
            for (int tileY = 0; tileY <= maxY; ++tileY)
            {
                for (int tileX = 0; tileX <= maxX; ++tileX)
                {
                    var tile = picture[new Point(tileX, tileY)].Crop();
                    
                    var offsetY = tileY * d;
                    var offsetX = tileX * d;
                    
                    for (int y = 0; y < d; ++y)
                    {
                        for (int x = 0; x < d; ++x)
                        {
                            result[offsetY + y][offsetX + x] = tile[y][x];

                        }
                    }
                }
            }

            return result;
        }

        private static IEnumerable<char[][]> CreateVariations(this char[][] data)
        {
            yield return data;
            yield return data.RotateRight90();
            yield return data.RotateRight90().RotateRight90();
            yield return data.RotateRight90().RotateRight90().RotateRight90();
            yield return data.FlipHorizontal();
            yield return data.FlipHorizontal().RotateRight90();
            yield return data.FlipHorizontal().RotateRight90().RotateRight90();
            yield return data.FlipHorizontal().RotateRight90().RotateRight90().RotateRight90();
        }

        private static char[][] FlipHorizontal(this char[][] data)
        {
            return data.Select(r => r.Reverse().ToArray()).ToArray();
        }

        private static char[][] RotateRight90(this char[][] data)
        {
            var result = data.Select(d => d.ToArray()).ToArray();
            for (int i = 0; i < data.Length; ++i)
            {
                for (int j = 0; j < data.Length; ++j)
                {
                    result[i][j] = data[data.Length - j - 1][i];
                }
            }

            return result;
        }

        private static int MarkAndCountSeaMonster(this char[][] picture)
        {
            var seaMonster = Seamonster().SeamonsterToPoints().ToArray();
            var maxX = seaMonster.Max(t => t.X) + 1;
            var maxY = seaMonster.Max(t => t.Y) + 1;

            var found = false;

            for (int y = 0; y < picture.Length - maxY; ++y)
            {
                for (int x = 0; x < picture[y].Length - maxX; ++x)
                {
                    if (seaMonster.All(p => picture[y + p.Y][x + p.X] == '1'))
                    {
                        found = true;
                        foreach (var p in seaMonster)
                        {
                            picture[y + p.Y][x + p.X] = 'X';
                        }
                    }
                }
            }
            
            var count = 0;

            if (found)
            {
                for (int y = 0; y < picture.Length; ++y)
                {
                    for (int x = 0; x < picture[y].Length; ++x)
                    {
                        if (picture[y][x] == '1')
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static char[][] Seamonster()
        {
            return new []
            {
                "                  # ".ToArray(),
                "#    ##    ##    ###".ToArray(),
                " #  #  #  #  #  #   ".ToArray(),
            };
        }

        private static IEnumerable<Point> SeamonsterToPoints(this char[][] monster)
        {
            for (int y = 0; y < monster.Length; ++y)
            {
                for (int x = 0; x < monster[y].Length; x++)
                {
                    if (monster[y][x] == '#')
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }
    }
}