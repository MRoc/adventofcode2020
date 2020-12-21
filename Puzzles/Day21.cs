using System;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 2075
    // Puzzle 2: 0
    public static class Day21
    {
        public static long Puzzle1()
        {
            var recipes = Input
                .LoadLines("Puzzles.Input.input21.txt")
                .Select(Recipe.Parse)
                .ToArray();

            while (true)
            {
                var allergens = recipes
                    .SelectMany(r => r.Allergens)
                    .Distinct()
                    .ToArray();
                
                var allergensIngredientMap = allergens
                    .ToDictionary(
                        a => a,
                        a => recipes
                            .Where(i => i.Allergens.Contains(a))
                            .Select(i => i.Ingredients)
                            .Aggregate((x, y) => x.Intersect(y).ToArray()));

                var single = allergensIngredientMap
                    .Where(i => i.Value.Length == 1)
                    .Select(i => (Ingredient: i.Value.Single(), Allergene: i.Key))
                    .FirstOrDefault();

                if (single.Ingredient is { })
                {
                    recipes = recipes
                        .Select(r => new Recipe(
                            r.Ingredients.Where(i => i != single.Ingredient).ToArray(),
                            r.Allergens.Where(a => a != single.Allergene).ToArray()))
                        .ToArray();
                }
                else
                {
                    break;
                }
            }

            return recipes.SelectMany(r => r.Ingredients).Count();
        }

        public static string Puzzle2()
        {
            return string.Empty;
        }

        public record Recipe(string[] Ingredients, string[] Allergens)
        {
            public static Recipe Parse(string line)
            {
                var parts = line.Replace(")", "").Split(" (contains ");

                return new Recipe(
                    parts[0].Split(" ").Select(w => w.Trim()).ToArray(),
                    parts[1].Split(", ").Select(w => w.Trim()).ToArray());
            }
        }
    }
}
