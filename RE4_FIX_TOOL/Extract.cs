using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE4_FIX_TOOL
{
    internal static class Extract
    {
        internal static void ExtractFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            string baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string baseDiretory = fileInfo.DirectoryName;

            var fix = new BinaryReader(fileInfo.OpenRead());

            uint magic = fix.ReadUInt32();
            if (magic != 0)
            {
                fix.Close();
                Console.WriteLine("Invalid FIX file.");
                return;
            }
            fix.BaseStream.Position = 0;

            //leitura dos offsets
            List<(uint length, uint offset)> imgList = new List<(uint length, uint offset)>();

            uint positionCheck = uint.MaxValue;
            bool _continue;
            do
            {
                _ = fix.ReadUInt32(); //ID, é o mesmo que a ordem
                uint _length = fix.ReadUInt32();
                uint _offset = fix.ReadUInt32();

                imgList.Add((_length, _offset));

                if (_offset < positionCheck)
                {
                    positionCheck = _offset;
                }
                _continue = fix.BaseStream.Position < positionCheck;

            } while (_continue);

            Directory.CreateDirectory(Path.Combine(baseDiretory, baseName));

            //extrai imagens

            for (int i = 0; i < imgList.Count; i++)
            {
                fix.BaseStream.Position = imgList[i].offset;

                byte[] imagebytes = new byte[imgList[i].length];
                fix.BaseStream.Read(imagebytes, 0, imagebytes.Length);

                uint imagemagic = BitConverter.ToUInt32(imagebytes, 0);

                string Extension = "error";

                if (imagemagic == 0x20534444)
                {
                    Extension = "dds";
                }
                else if (imagemagic == 0x20464E47)
                {
                    Extension = "gnf";
                }
                else if (imagemagic == 0x00020000 || imagemagic == 0x000A0000)
                {
                    Extension = "tga";
                }

                File.WriteAllBytes(Path.Combine(baseDiretory, baseName, i.ToString("D4") + "." + Extension), imagebytes);
                Console.WriteLine("Extracted file: " + baseName + "\\" + i.ToString("D4") + "." + Extension);
            }
            fix.Close();

            var idx = new FileInfo(Path.Combine(baseDiretory, baseName + ".idxfix")).CreateText();
            idx.WriteLine("# RE4 FIX TOOL");
            idx.WriteLine("# By: JADERLINK");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("# only info = count: " + imgList.Count);
            idx.Close();
        }
    }
}
