// Define a class representing a recipe
public class Recipe
{
    // Properties of the recipe
    public string Name { get; set; } // Name of the recipe
    public List<Ingredient> Ingredients { get; set; } // List of ingredients in the recipe
    public List<Step> Steps { get; set; } // List of steps in the recipe
    public string FoodGroup { get; set; } // Food group of the recipe
    public int Calories { get; set; } // Total calories of the recipe

    // Constructor for initializing a recipe with name, food group, and calories
    public Recipe(string name, string foodGroup, int calories)
    {
        Name = name; // Initialize the name of the recipe
        FoodGroup = foodGroup; // Initialize the food group of the recipe
        Calories = calories; // Initialize the calories of the recipe
        Ingredients = new List<Ingredient>(); // Initialize the list of ingredients
        Steps = new List<Step>(); // Initialize the list of steps
    }

    // Method to calculate the total calories of the recipe
    public int CalculateTotalCalories()
    {
        int totalCalories = 0; // Initialize total calories counter
        foreach (var ingredient in Ingredients) // Iterate through each ingredient
        {
            totalCalories += ingredient.Calories; // Add calories of the ingredient to total calories
        }
        return totalCalories; // Return the total calories
    }

    // Method to scale the recipe by a given factor
    public void Scale(double scalingFactor)
    {
        foreach (var ingredient in Ingredients) // Iterate through each ingredient
        {
            ingredient.Quantity = ingredient.OriginalQuantity * scalingFactor; // Scale the quantity of the ingredient
        }
    }
}
