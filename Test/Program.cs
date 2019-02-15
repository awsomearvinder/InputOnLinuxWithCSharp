using System;
using InputLibrary;
using System.Threading;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string[] Inputs = {"Inputs"};
            InputDevices Keyboard = new InputDevices();
            Thread.Sleep(10000);
            Environment.Exit(0);
        }
    }
}

