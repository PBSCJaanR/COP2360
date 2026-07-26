using System;
using System.Collections.Generic;
					
public class Contractor
{
	// class properties - automatically implemented accessor and mutators
	public string Name {get; set;} 
	public string Number {get; set;} // string because employee IDs can often contain letters
	// Defaults start date to current day if not specified. Cannot be changed after initialized
	public DateOnly StartDate {get; init;} = DateOnly.FromDateTime(DateTime.Now);
	
	// Constructors
	public Contractor() // no-arg constructor - sets properties to default data that can later be parsed
	{
		Name = "";
		Number = "";
	}
	public Contractor(string name, string number) // Constructor for a contractor starting today
	{
		Name = name;
		Number = number;
	}
	public Contractor(string name, string number, DateOnly startDate) // constructor for all properties
	{
		Name = name;
		Number = number;
		StartDate = startDate;
	}
	// The ever-useful toString method that will return the object as a string
	public override string ToString()
	{
		return "Contractor " + Name + ": Number " + Number + ", began work on " + StartDate;
	}
}

public class Subcontractor : Contractor
{
	// Class properties
	// Shows whether the subcontractor works day or night shift. Day = 1, night = 2
	public int Shift
	{
		get;
		// Input validation for mutator. If attempted to be set to a value out of range, sets to day shift by default
		set => field = (value > 2) || (value < 1) ? 1 : value;
	}
	public double HourlyPayRate
	{
		get;
		set => field = Math.Round(value, 2); // Rounds input value to 2 decimal places since we are working with currency
	}
	
	// Constructors
	public Subcontractor() : base() // no-arg constructor
	{
		Shift = 1; // day shift
		HourlyPayRate = 15.00; // minimum wage
	}
	public Subcontractor(int shift, double hourlyPay) : base() // All parameters; invokes Contractor's no-arg constructor
	{
		Shift = shift;
		HourlyPayRate = hourlyPay;
	}
	// Invokes Contractor's parameterized constructor
	public Subcontractor(string name, string number, int shift, double hourlyPay) : base(name, number)
	{
		Shift = shift;
		HourlyPayRate = hourlyPay;
	}
	// Allows for specification of subcontractor's state date in addition to other identifying info
	public Subcontractor(string name, string number, DateOnly startDate, int shift, double hourlyPay) : base(name, number, startDate)
	{
		Shift = shift;
		HourlyPayRate = hourlyPay;
	}
	
	// Class method - compute pay with 3% shift differential for night shift employees
	public float ComputePay(int hoursWorked)
	{
		// Shift differential of 3%
		double shiftDifferential = 0;
		if(Shift == 2)
			shiftDifferential = HourlyPayRate * 0.03;
		
		// Since spec requires the method to return float, we cast a float to the result of the operation
		float totalPay = (float) (hoursWorked * (HourlyPayRate + shiftDifferential));
		return totalPay;
	}
	
	// Overriding the parent class' ToString method to add information specific to the Subcontractor class
	public override string ToString()
	{
		// Dynamically construct the string containing additional info for the class based on the member properties
		string addition = ", works shift ";
		if(Shift == 1)
			addition += "1 (Day), ";
		else
			addition += "2 (Night), ";
		addition += $"paid {HourlyPayRate:C2} per hour"; // Automatically formats pay rate into currency using string interpolation
		
		// Returns all of the info from the base class + all of the new info formatted above
		return base.ToString() + addition;
	}
}

public class TestContractor
{
	public static void Main()
	{
		Console.WriteLine("Subcontractor class demonstration:");
		Subcontractor joe = new Subcontractor("Joe", "2319", 2, 17.89); // Our first subcontractor. Note the shift number, indicating night
		// Outputting our subcontractor Joe's attributes to make sure they were assigned correctly
		Console.WriteLine("Name of subcontractor: " + joe.Name);
		Console.WriteLine("ID of subcontractor: " + joe.Number);
		Console.WriteLine("Subcontractor started working on " + joe.StartDate);
		Console.WriteLine("Shift of subcontractor: " + (joe.Shift == 1 ? "Day" : "Night"));
		Console.WriteLine($"Subcontractor's hourly pay: {joe.HourlyPayRate:C2}");
		if(joe.Shift == 2) // Making note of the difference in pay depending on shift
			Console.WriteLine("Because they work the night shift, this subcontractor gets a 3% shift differential.");
		Console.WriteLine($"Subcontractor's total pay after 40 hours (a full work week): {joe.ComputePay(40):C2}");
		
		// A loop will allow the user to create and read as many Contractor and Subcontractor instances as they please
		List<Contractor> contractorList = new List<Contractor>(); // This list will hold all of the Contractor instances the user creates
		
		Console.WriteLine("Welcome, user!");
		Console.WriteLine();
		int input = 0;
		do
		{
			// Main menu for the interactive program
			Console.WriteLine("1. Create Contractor");
			Console.WriteLine("2. Create Subcontractor");
			Console.WriteLine("3. View all Contractors");
			Console.WriteLine("4. Exit");
			Console.WriteLine("Choose an option (1 - 4):");
			if(int.TryParse(Console.ReadLine(), out input)) // ReadLine can only read in strings, so it must be parsed as an int
			{
				if(input < 1 || input > 4) // Invalid input safety
				{
					Console.WriteLine("Please enter a number from 1 to 4.");
					continue;
				}
				else if(input == 1) // Add Contractor
				{
					// Read in user input for the properties of the Contractor
					Console.WriteLine("Enter the Contractor's name:");
					string name = Console.ReadLine();
					Console.WriteLine("Enter the Contractor's number/ID:");
					string number = Console.ReadLine();
					Console.WriteLine("Would you like to enter a starting date? (y/n):");
					string confirm = Console.ReadLine();
					// Starting date is an optional property. The following loop serves as input validation
					while(confirm.ToLower() != "y" && confirm.ToLower() != "n")
					{
						Console.WriteLine("Invalid input.\nWould you like to enter a starting date? (y/n):");
						confirm = Console.ReadLine();
					}
					// Declare Contractor object first so it can be initialized depending on option chosen
					Contractor newContractor;
					if(confirm.ToLower() == "y")
					{
						Console.WriteLine("Enter the Contractor's starting date:");
						// since ReadLine can only read strings from user input, must parse as DateOnly
						string dateString = Console.ReadLine();
						DateOnly startDate = DateOnly.Parse(dateString);
						// Initialize Contractor object
						newContractor = new Contractor(name, number, startDate);
					}
					else // Since inputs were validated earlier, this means the user selected "n"
					{
						newContractor = new Contractor(name, number); // Initialize contractor object assuming current day is start date
					}
					contractorList.Add(newContractor); // Add initialized object to list, able to be viewed later.
					Console.WriteLine();
					Console.WriteLine("Contractor added!"); // Give user feedback before looping
					Console.WriteLine();
				}
				else if(input == 2) // Add Subcontractor
				{
					// Find shift number
					Console.WriteLine("Enter the Subcontractor's shift - Day or Night (d/n):");
					string shiftTime = Console.ReadLine();
					// Input validation to make sure that the user can only choose day or night
					while(shiftTime.ToLower() != "d" && shiftTime.ToLower() != "n")
					{
						Console.WriteLine("Invalid input.\nEnter the Contractor's shift - Day or Night (d/n):");
						shiftTime = Console.ReadLine();
					}
					// Declare Subcontractor object for modification across the process
					Subcontractor newSubcontractor = new Subcontractor();
					if(shiftTime.ToLower() == "d") // Since input has been vetted, we can guarantee that it can only be one of the two options
						newSubcontractor.Shift = 1;
					else
						newSubcontractor.Shift = 2;
					// Find hourly pay
					Console.WriteLine("Enter the Subcontractor's hourly pay:");
					string rawPay = Console.ReadLine();
					double payRate;
					while(true) // More input validation. Loops until the user successfully inputs a value that can be parsed as double
					{
						if(double.TryParse(rawPay, out payRate))
							break; // Only continues in loop if input is invalid; otherwise, immediately breaks out
						Console.WriteLine("Invalid input.\nEnter the Subcontractor's hourly pay:");
						rawPay = Console.ReadLine();
					}
					newSubcontractor.HourlyPayRate = payRate;
					
					// Determine if Contractor properties should be initialized
					Console.WriteLine("Would you like to include more information about the Subcontractor? (y/n):");
					string confirm = Console.ReadLine();
					// More input validation via the loop
					while(confirm.ToLower() != "y" && confirm.ToLower() != "n")
					{
						Console.WriteLine("Invalid input.\nWould you like to enter a starting date? (y/n):");
						confirm = Console.ReadLine();
					}
					if(confirm.ToLower() == "y")
					{
						// Essentially copied from Contractor initialization section
						Console.WriteLine("Enter the Contractor's name:");
						newSubcontractor.Name = Console.ReadLine();
						Console.WriteLine("Enter the Contractor's number/ID:");
						newSubcontractor.Number = Console.ReadLine();
					}
					// No need to change Contractor properties if not desired since Contractor constructor initialized them already
					contractorList.Add(newSubcontractor); // Can add Subcontractor to Contractor list since it is a subclass
					Console.WriteLine();
					Console.WriteLine("Subcontractor added!"); // user feedback before looping
					Console.WriteLine();
				}
				else if(input == 3) // Display Contractors
				{
					if(contractorList.Count == 0)
					{
						Console.WriteLine("No Contractors to display.");
						Console.WriteLine();
						continue;
					}
					foreach(Contractor c in contractorList)
					{
						Console.WriteLine(c); // Use polymorphism to print different information for Contractors and Subcontractors
						if(c is Subcontractor sc) // Re-cast Subcontractors to Subcontractor type to access class-specific method
							Console.WriteLine($"Subcontractor's pay for a full 40-hour work week is ${sc.ComputePay(40)}.");
					}
					Console.WriteLine();
				}
			} 
			else // Parsing invalid input
			{
				Console.WriteLine("Please enter a number.");
				continue;
			}
			
		} while(input != 4);
		Console.WriteLine("Thank you for participating."); // Displays upon exit
	}
}