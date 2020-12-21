using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    // Puzzle 1: 2075
    // Puzzle 2: zfcqk,mdtvbb,ggdbl,frpvd,mgczn,zsfzq,kdqls,kktsjbh
    public static class Day21
    {
        public static long Puzzle1()
        {
            var allergeneMap = CreateAllergenMap();

            return LoadRecipes()
                .SelectMany(r => r.Ingredients)
                .Where(i => !allergeneMap.ContainsKey(i))
                .Count();
        }

        public static string Puzzle2()
        {
            return CreateAllergenMap()
                .OrderBy(i => i.Value)
                .Select(i => i.Key)
                .Aggregate((a, b) => a + "," + b);
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

        private static Recipe[] LoadRecipes()
        {
            return Input
                .LoadLines("Puzzles.Input.input21.txt")
                .Select(Recipe.Parse)
                .ToArray();
        }

        private static Dictionary<string, string> CreateAllergenMap()
        {
            var result = new Dictionary<string, string>();

            var recipes = LoadRecipes();

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
                    result[single.Ingredient] = single.Allergene;

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

            return result;
        }
    }
}
