# RanSharp

TomLBZ's Personal C# Library

It is said that a computer builds fantasy out of reality. To achieve this purpose, it must be easy enough to use. 
Sometimes we are just stuck with a lot of boilerplate code, such as looping through lists and arrays to perform 
computation based on each element. This library is designed to make it easier to write code that achieves the same purpose, 
but simpler and faster.

# Usage
```csharp
using RanSharp.Performance;
using RanSharp.Maths;

// Using the ArrVector<T> struct ================================
// Create a new ArrVector<int> with 5 elements
ArrVector<int> a = new(1,2,3,4,5); 
ArrVector<int> b = new(5, i => i); // Same effect
// there are many more ways to create an ArrVector. Use your intellisense:)

// Print line
Console.WriteLine((a + b) * 2); // Prints (4, 8, 12, 16, 20)

// Create a new ArrVector<double> with 1e6 elements, each initialized to the square root of its index
ArrVector<double> c = new(1e6, i => Math.Sqrt(i));
var sin_c = c.Map(Math.Sin); // Apply the sine function to each element and store the result in a new ArrVector<double>

// Using the Loop class
double data = new double[1000000];


```