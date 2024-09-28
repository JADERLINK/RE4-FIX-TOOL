using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_FIX_TOOL
{
    internal static class Repack
    {
        internal static void RepackFile(string file) 
        {
            FileInfo fileInfo = new FileInfo(file);
            string baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string baseDiretory = fileInfo.DirectoryName;

            string ImageFolder = Path.Combine(baseDiretory, baseName);

            if (Directory.Exists(ImageFolder)) 
            {
                uint iCount = 0; // quantidade de imagens
                bool asFile = true;

                while (asFile)
                {
                    string ddspath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".dds");
                    string gnfpath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".gnf");
                    string tgapath = Path.Combine(ImageFolder, iCount.ToString("D4") + ".tga");

                    if (File.Exists(ddspath) || File.Exists(gnfpath) || File.Exists(tgapath))
                    {
                        iCount++;
                    }
                    else
                    {
                        asFile = false;
                    }
                }

                Console.WriteLine("Count: " + iCount);
                if (iCount == 0)
                {
                    Console.WriteLine("It is not possible to create a fix file with zero images.");
                    return;
                }

                BinaryWriter fixFile = null;

                try
                {
                    string fixName = baseName + ".fix";
                    fixName = baseName.Length > 0 ? fixName : "NoName.fix";

                    FileInfo packFileInfo = new FileInfo(Path.Combine(baseDiretory, fixName));
                    fixFile = new BinaryWriter(packFileInfo.Create());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + Environment.NewLine + ex);
                }

                if (fixFile != null)
                {

                    uint offsetToOffset = 0;
                    uint nextOffset = iCount * 3 * 4; // são 3 campos e cada campo tem 4 bytes

                    for (int i = 0; i < iCount; i++)
                    {
                        string ddspatch = Path.Combine(ImageFolder, i.ToString("D4") + ".dds");
                        string gnfpath = Path.Combine(ImageFolder, i.ToString("D4") + ".gnf");
                        string tgapatch = Path.Combine(ImageFolder, i.ToString("D4") + ".tga");

                        FileInfo imageFile = null;
                        if (File.Exists(gnfpath))
                        {
                            imageFile = new FileInfo(gnfpath);
                        }
                        else if (File.Exists(ddspatch))
                        {
                            imageFile = new FileInfo(ddspatch);
                        }
                        else if (File.Exists(tgapatch))
                        {
                            imageFile = new FileInfo(tgapatch);
                        }


                        if (imageFile != null)
                        {
                            fixFile.BaseStream.Position = offsetToOffset;
                            fixFile.Write((uint)i);
                            fixFile.Write((uint)imageFile.Length);
                            fixFile.Write((uint)nextOffset);

                            fixFile.BaseStream.Position = nextOffset;

                            var fileStream = imageFile.OpenRead();
                            fileStream.CopyTo(fixFile.BaseStream);
                            fileStream.Close();

                            nextOffset = (uint)fixFile.BaseStream.Position;
                            offsetToOffset += 12;

                            Console.WriteLine("Add file: " + imageFile.Name);
                        }
                        else 
                        {
                            fixFile.BaseStream.Position = offsetToOffset;
                            fixFile.Write((uint)i);
                            fixFile.Write((uint)4);
                            fixFile.Write((uint)nextOffset);

                            fixFile.BaseStream.Position = nextOffset;
                            fixFile.Write((uint)0);

                            nextOffset = (uint)fixFile.BaseStream.Position;
                            offsetToOffset += 12;

                            Console.WriteLine("Error adding file: " + imageFile.Name);
                        }

                    }

                    fixFile.Close();
                }
            }
            else
            {
                Console.WriteLine($"The folder {baseName} does not exist.");
            }
        }

    }
}
