// Define a class representing an ingredient
public class Ingredient
{
    // Properties of the ingredient
    public string Name { get; set; } // Name of the ingredient
    public double Quantity { get; set; } // Quantity of the ingredient
    public double OriginalQuantity { get; set; } // Original quantity of the ingredient
    public string Unit { get; set; } // Unit of measurement for the quantity
    public int Calories { get; set; } // Calories of the ingredient
    public string FoodGroup { get; set; } // Food group of the ingredient

    // Constructor for initializing an ingredient with specified values
    public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
    {
        Name = name; // Initialize the name of the ingredient
        Quantity = quantity; // Initialize the quantity of the ingredient
        OriginalQuantity = quantity; // Set the original quantity to the initial quantity
        Unit = unit; // Initialize the unit of measurement
        Calories = calories; // Initialize the calories of the ingredient
        FoodGroup = foodGroup; // Initialize the food group of the ingredient
    }

    // Method to reset the quantity of the ingredient to its original value
    public void ResetQuantity()
    {
        Quantity = OriginalQuantity; // Reset the quantity to the original quantity
    }
}
