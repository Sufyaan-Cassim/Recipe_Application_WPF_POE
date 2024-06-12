using RecipeApplicationWPF;
public class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public List<Step> Steps { get; set; }

    public Recipe(string name)
    {
        Name = name;
        Ingredients = new List<Ingredient>();
        Steps = new List<Step>();
    }
    public int CalculateTotalCalories()
    {
        int totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
    }
    public void Display()
    {
        Console.WriteLine("************************************************");
        Console.WriteLine("Recipe name: " + Name);
        Console.WriteLine("************************************************");

        Console.WriteLine("Ingredients:");
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine("- " + ingredient.Quantity + " " + ingredient.Unit + " of " + ingredient.Name);
        }
        Console.WriteLine();

        Console.WriteLine("Steps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + Steps[i].Description);
        }
    }

    public void Scale(double scalingFactor)
    {
        foreach (var ingredient in Ingredients)
        {
            ingredient.Quantity *= scalingFactor;
        }
    }

    public void ResetQuantities(List<double[]> originalQuantities)
    {
        for (int i = 0; i < Ingredients.Count; i++)
        {
            Ingredients[i].Quantity = originalQuantities[i][0];
        }
    }
}