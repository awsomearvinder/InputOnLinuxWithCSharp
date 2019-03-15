using System;
namespace Calculating_Pi
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal current = 1.0m;
            decimal denominator=3.0m;
            for(int i =0; i < 9999999999; i++){
                current = current - 1.0m/(denominator)+(1.0m/(denominator+2.0m));
                Console.WriteLine(current*4);
                denominator+=4;
            }
            Console.WriteLine(current*4);
        }
    }
}
