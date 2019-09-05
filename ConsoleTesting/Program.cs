using System;
using System.IO;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Calculating SFV...");

            var songBytes = File.ReadAllBytes(@"D:\Personal\Music\Artists\A Flock of Seagulls\A_Flock_Of_Seagulls_With_The_Prague_Philharmonic_Orchestra-Ascension-CD-2018-D2H\01-a_flock_of_seagulls_with_the_prague_philharmonic_orchestra-i_ran.mp3");
            var result = CollectionManagementLib.CRC32.Compute(songBytes);
            
            Console.WriteLine("{0:X}", result);
            
            //CollectionScanner.GenerateStructure(@"D:\Personal\Music\Artists\A Flock of Seagulls\");
        }
    }
}