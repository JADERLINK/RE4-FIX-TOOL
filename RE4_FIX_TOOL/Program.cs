using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_FIX_TOOL
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine("# RE4 FIX TOOL");
            Console.WriteLine("# By: JADERLINK");
            Console.WriteLine("# youtube.com/@JADERLINK");
            Console.WriteLine("# VERSION 1.0.1 (2024-09-28)");

            if (args.Length == 0)
            {
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE4-FIX-TOOL");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (File.Exists(args[i]))
                    {
                        FileInfo info = null;

                        try
                        {
                            info = new FileInfo(args[i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error in the path: " + Environment.NewLine + ex);
                        }
                        if (info != null)
                        {
                            Console.WriteLine("File: " + info.Name);

                            if (info.Extension.ToUpperInvariant() == ".FIX")
                            {
                                try
                                {
                                    Extract.ExtractFile(info.FullName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + Environment.NewLine + ex);
                                }

                            }
                            else if (info.Extension.ToUpperInvariant() == ".IDXFIX")
                            {
                                try
                                {
                                    Repack.RepackFile(info.FullName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + Environment.NewLine + ex);
                                }
                            }
                            else
                            {
                                Console.WriteLine("The extension is not valid: " + info.Extension);
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("File specified does not exist: " + args[i]);
                    }

                }
            }
         
            Console.WriteLine("Finished!!!");
        }
    }
}
