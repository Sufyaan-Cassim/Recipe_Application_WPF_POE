using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RecipeApplicationWPF
{
    public partial class MainWindow : Window
    {
        // Properties to store ingredients, recipes, and steps
        private List<Ingredient> Ingredients { get; set; }
        public static ObservableCollection<Recipe> Recipes { get; set; } = new ObservableCollection<Recipe>();
        private List<Step> Steps { get; set; }
        public event EventHandler RecipesChanged; // Event to notify when recipes change
        private List<Recipe> filteredRecipes; // List to store filtered recipes
        private Window displayWindow; // Window to display recipes

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
            displayWindow = CreateDisplayWindow(); // Initialize display window
        }

        // Event handler for the Enter Recipe button
        private void EnterRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new EnterRecipeControl(); // Show the EnterRecipeControl in ContentArea
        }

        // Event handler for the Display Recipe button
        // Event handler for the Display Recipe button
        private void DisplayRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                MessageBox.Show("No recipes available to display."); // Show message if no recipes are available
                return;
            }

            displayWindow = CreateDisplayWindow(); // Create a new display window
            var (filterStackPanel, filterResultListBox) = CreateFilterStackPanel(); // Create filter UI elements
            var allRecipesStackPanel = CreateAllRecipesStackPanel(); // Create UI elements for displaying all recipes

            var allRecipesListBox = new ListBox { Margin = new Thickness(10) };
            var sortedRecipes = Recipes.OrderBy(recipe => recipe.Name).ToList(); // Sort recipes by name

            foreach (var recipe in sortedRecipes)
            {
                // Concatenate the recipe name, total ingredients, and total steps
                var displayText = $"{recipe.Name} | Ingredients: {recipe.Ingredients.Count} | Steps: {recipe.Steps.Count}";
                var textBlock = new TextBlock();

                // Set color for each part of the display text
                textBlock.Inlines.Add(new Run(recipe.Name) { FontWeight = FontWeights.Bold, Foreground = Brushes.DarkBlue }); // Recipe name
                textBlock.Inlines.Add(new Run($" | Ingredients: {recipe.Ingredients.Count}") { FontWeight = FontWeights.Bold, Foreground = Brushes.DarkGreen }); // Ingredients count
                textBlock.Inlines.Add(new Run($" | Steps: {recipe.Steps.Count}") { FontWeight = FontWeights.Bold, Foreground = Brushes.DarkRed }); // Steps count

                allRecipesListBox.Items.Add(textBlock);
            }

            // Event handler for selecting a recipe from the ListBox
            allRecipesListBox.SelectionChanged += (s, args) =>
            {
                if (allRecipesListBox.SelectedItem != null)
                {
                    var selectedRecipe = sortedRecipes.FirstOrDefault(recipe =>
                    {
                        var selectedItemText = ((TextBlock)allRecipesListBox.SelectedItem).Inlines.OfType<Run>().FirstOrDefault()?.Text;
                        return selectedItemText == recipe.Name;
                    });

                    if (selectedRecipe != null)
                    {
                        ContentArea.Content = new DisplayRecipeControl(selectedRecipe); // Display selected recipe
                        displayWindow.Close(); // Close the display window
                    }
                }
            };

            // Add UI elements to the stack panel
            allRecipesStackPanel.Children.Add(new TextBlock
            {
                Text = "Available Recipes",
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = Brushes.DarkBlue
            });
            allRecipesStackPanel.Children.Add(allRecipesListBox);

            // Create a grid to display filters and all recipes side by side
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(filterStackPanel);
            grid.Children.Add(allRecipesStackPanel);
            Grid.SetColumn(filterStackPanel, 0);
            Grid.SetColumn(allRecipesStackPanel, 1);

            displayWindow.Content = grid; // Set the grid as the content of the display window
            displayWindow.ShowDialog(); // Show the display window
        }

        // Method to create a display window
        private Window CreateDisplayWindow()
        {
            return new Window
            {
                Title = "Display Recipes",
                Width = 800,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.LightGray
            };
        }

        // Method to create filter UI elements
        private (StackPanel filterStackPanel, ListBox filterResultListBox) CreateFilterStackPanel()
        {
            var filterStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10),
                Background = Brushes.LightBlue
            };

            var ingredientLabel = new Label { Content = "Enter the name of the ingredient:" };
            var ingredientTextBox = new TextBox { Width = 200, Margin = new Thickness(0, 5, 0, 5) };

            var foodGroupLabel = new Label { Content = "Choose a food group:" };
            var foodGroupComboBox = new ComboBox
            {
                ItemsSource = new List<string>
                {
                    "",
                    "Starchy foods",
                    "Vegetables and fruits",
                    "Dry beans, peas, lentils and soya",
                    "Chicken, fish, meat and eggs",
                    "Milk and dairy products",
                    "Fats and oil",
                    "Water"
                },
                Width = 200,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var maxCaloriesLabel = new Label { Content = "Enter max number of calories:" };
            var maxCaloriesTextBox = new TextBox { Width = 200, Margin = new Thickness(0, 5, 0, 5) };

            var filterButton = new Button
            {
                Content = "Filter",
                Background = Brushes.LightGreen,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 10, 0, 0)
            };

            var filterResultListBox = new ListBox { Margin = new Thickness(10), SelectionMode = SelectionMode.Single };

            // Event handler for the Filter button click event
            filterButton.Click += (s, args) =>
            {
                string ingredientName = ingredientTextBox.Text;
                string foodGroup = foodGroupComboBox.SelectedItem?.ToString() ?? string.Empty;
                int maxCalories = int.TryParse(maxCaloriesTextBox.Text, out int result) ? result : int.MaxValue;

                if (string.IsNullOrEmpty(ingredientName) && string.IsNullOrEmpty(foodGroup) && maxCalories == int.MaxValue)
                {
                    MessageBox.Show("No filters chosen.");
                    return;
                }

                UpdateFilteredRecipesListBox(filterResultListBox, ingredientName, foodGroup, maxCalories);
            };

            // Event handler for selecting a filtered recipe from the ListBox
            filterResultListBox.SelectionChanged += (s, args) =>
            {
                if (filterResultListBox.SelectedItem != null)
                {
                    var selectedRecipeName = filterResultListBox.SelectedItem.ToString();
                    var selectedRecipe = filteredRecipes.FirstOrDefault(recipe => recipe.Name == selectedRecipeName);
                    if (selectedRecipe != null)
                    {
                        if (ContentArea.Content is DisplayRecipeControl)
                        {
                            ContentArea.Content = new DisplayRecipeControl(selectedRecipe);
                        }
                        else
                        {
                            ContentArea.Content = new DisplayRecipeControl(selectedRecipe);
                            displayWindow.Close();
                        }
                    }
                }
            };

            // Add filter UI elements to the stack panel
            filterStackPanel.Children.Add(new TextBlock { Text = "Filter Recipes", FontWeight = FontWeights.Bold, FontSize = 16, Foreground = Brushes.DarkBlue });
            filterStackPanel.Children.Add(ingredientLabel);
            filterStackPanel.Children.Add(ingredientTextBox);
            filterStackPanel.Children.Add(foodGroupLabel);
            filterStackPanel.Children.Add(foodGroupComboBox);
            filterStackPanel.Children.Add(maxCaloriesLabel);
            filterStackPanel.Children.Add(maxCaloriesTextBox);
            filterStackPanel.Children.Add(filterButton);
            filterStackPanel.Children.Add(filterResultListBox);

            return (filterStackPanel, filterResultListBox); // Return the stack panel and the list box
        }

        // Method to create UI elements for displaying all recipes
        private StackPanel CreateAllRecipesStackPanel()
        {
            return new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10),
                Background = Brushes.LightGray
            };
        }

        // Method to update the filtered recipes list box based on filter criteria
        private void UpdateFilteredRecipesListBox(ListBox filterResultListBox, string ingredientName, string foodGroup, int maxCalories)
        {
            filterResultListBox.Items.Clear();

            filteredRecipes = FilterRecipes(ingredientName, foodGroup, maxCalories);

            if (filteredRecipes.Any())
            {
                foreach (var recipe in filteredRecipes)
                {
                    filterResultListBox.Items.Add(recipe.Name);
                }
            }
            else
            {
                filterResultListBox.Items.Add("No matching recipes found.");
            }
        }

        // Method to filter recipes based on ingredient name, food group, and maximum calories
        private List<Recipe> FilterRecipes(string ingredientName, string foodGroup, int maxCalories)
        {
            if (maxCalories == 0)
            {
                maxCalories = int.MaxValue;
            }

            return Recipes.Where(recipe =>
                (string.IsNullOrEmpty(ingredientName) || recipe.Ingredients.Any(ingredient => ingredient.Name.Contains(ingredientName, StringComparison.OrdinalIgnoreCase))) &&
                (string.IsNullOrEmpty(foodGroup) || recipe.Ingredients.Any(ingredient => ingredient.FoodGroup.Equals(foodGroup, StringComparison.OrdinalIgnoreCase))) &&
                recipe.Ingredients.Sum(ingredient => ingredient.Calories) <= maxCalories).ToList();
        }

        // Event handler for the Scale Recipe button
        private void ScaleRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ScaleRecipeControl(); // Show the ScaleRecipeControl in ContentArea
        }

        // Event handler for the Reset Quantities button
        private void ResetQuantitiesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                MessageBox.Show("No recipes available to reset."); // Show message if no recipes are available
                return;
            }

            // Create and show the reset quantities window
            var resetQuantitiesWindow = new Window
            {
                Title = "Reset Recipe Quantities",
                Width = 400,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Content = new ResetQuantitiesControl()
            };

            resetQuantitiesWindow.ShowDialog();
        }

        // Method to reset quantities of ingredients in a recipe
        private void ResetRecipe(Recipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.ResetQuantity();
            }
        }

        // Event handler for the Clear Data button
        private void ClearDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                MessageBox.Show("There are no recipes to clear. No data to clear yet.");
                ContentArea.Content = null;
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to clear all recipes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Recipes.Clear();
                OnRecipesChanged();
                MessageBox.Show("All recipe data cleared successfully!");
                ContentArea.Content = null;
            }
        }

        // Method to raise the RecipesChanged event
        private void OnRecipesChanged()
        {
            RecipesChanged?.Invoke(this, EventArgs.Empty);
        }

        // Event handler for the Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the application?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
