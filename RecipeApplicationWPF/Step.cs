using System.ComponentModel;

// Define a class representing a step in a recipe
public class Step : INotifyPropertyChanged
{
    // Private fields for the description and completion status of the step
    private string description; // Description of the step
    private bool isCompleted; // Completion status of the step

    // Property to get or set the description of the step
    public string Description
    {
        get { return description; } // Get the description
        set
        {
            description = value; // Set the description
            OnPropertyChanged(nameof(Description)); // Notify subscribers that the description property has changed
        }
    }

    // Property to get or set the completion status of the step
    public bool IsCompleted
    {
        get { return isCompleted; } // Get the completion status
        set
        {
            isCompleted = value; // Set the completion status
            OnPropertyChanged(nameof(IsCompleted)); // Notify subscribers that the completion status property has changed
        }
    }

    // Constructor for initializing a step with a description
    public Step(string description)
    {
        Description = description; // Initialize the description of the step
        IsCompleted = false; // Initialize the completion status of the step to false
    }

    // Event to notify subscribers that a property has changed
    public event PropertyChangedEventHandler PropertyChanged;

    // Method to raise the PropertyChanged event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Raise the event with the name of the property that has changed
    }
}
