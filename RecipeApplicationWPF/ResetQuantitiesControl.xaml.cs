using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApplicationWPF
{
    public partial class ResetQuantitiesControl : UserControl
    {
        // Constructor for the ResetQuantitiesControl
        public ResetQuantitiesControl()
        {
            InitializeComponent();
            PopulateRecipeListBox();
        }

        // Method to populate the RecipeListBox with recipe names
        private void PopulateRecipeListBox()
        {
            // Clear existing items in the list box
            RecipeListBox.Items.Clear();

            // Add each recipe's name to the list box
            foreach (var recipe in MainWindow.Recipes)
            {
                RecipeListBox.Items.Add(recipe.Name);
            }
        }

        // Event handler for the Reset button click event
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a recipe is selected in the list box
            if (RecipeListBox.SelectedItem != null)
            {
                // Find the selected recipe by name
                var selectedRecipe = MainWindow.Recipes.FirstOrDefault(recipe => recipe.Name == RecipeListBox.SelectedItem.ToString());

                if (selectedRecipe != null)
                {
                    // Reset the quantity of each ingredient in the selected recipe
                    foreach (var ingredient in selectedRecipe.Ingredients)
                    {
                        ingredient.ResetQuantity();
                    }

                    // Update the result text block to inform the user
                    ResultTextBlock.Text = $"Recipe '{selectedRecipe.Name}' quantities have been reset!";
                }
            }
            else
            {
                // Inform the user to select a recipe if none is selected
                ResultTextBlock.Text = "Please select a recipe to reset.";
            }
        }

        // Event handler for the Cancel button click event
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window containing this control
            Window.GetWindow(this).Close();
        }
    }
}
