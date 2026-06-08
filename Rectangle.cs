using System;

// Taken from Classes.txt
public class Rectangle
{
  public readonly float Width, Height;

  public Rectangle (float width, float height)
  {
    Width = width;
    Height = height;
  }

  public void Deconstruct (out float width, out float height)
  {
    width = Width;
    height = Height;
  }
}

// A new class that inherits from Rectangle, using the concepts found in Inheritance.txt
public class Square : Rectangle
{
    public readonly float Side; // Only a single side length since squares' sides are equivalent
    
    // This subclass constructor calls the base class' constructor, since squares are rectangles
    public Square(float Side) : base(Side, Side)
    {
        this.Side = Side;
    }
}

// Main method here
class RectangleProgram
{
    static void Main()
    {
        Rectangle r = new Rectangle(4.5, 6.2); // Creates a rectangle with a width of 4.5 and a height of 6.2
        Square s = new Square(5.0); // Creates a square with side length 5
        Rectangle r2 = s as Rectangle; // Downcasts the square to act like a Rectangle class

        // Testing inheritance
        if(s is Rectangle rect)
            Console.WriteLine($"This rectangle has a width of {rect.width} and a height of {rect.height}!");
        
        // Testing type conversion
        if(r2 is Square sq)
            Console.WriteLine("r2 is a Square!");
        else
            Console.WriteLine("r2 is not a Square!");
    }
}