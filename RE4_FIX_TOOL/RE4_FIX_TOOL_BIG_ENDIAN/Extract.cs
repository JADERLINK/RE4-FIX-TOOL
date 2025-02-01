using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SimpleEndianBinaryIO;

namespace RE4_FIX_TOOL_BIG_ENDIAN
{
    internal static class Extract
    {
        internal static void ExtractFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            string baseName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string baseDirectory = fileInfo.DirectoryName;

            var fix = new EndianBinaryReader(fileInfo.OpenRead(), Endianness.BigEndian);

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

            uint positionCheck = (uint)fix.BaseStream.Length;
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

            Directory.CreateDirectory(Path.Combine(baseDirectory, baseName));

            //extrai imagens

            for (int i = 0; i < imgList.Count; i++)
            {
                fix.BaseStream.Position = imgList[i].offset;

                byte[] imagebytes = new byte[imgList[i].length];
                fix.BaseStream.Read(imagebytes, 0, imagebytes.Length);

                uint imagemagic = 0;
                try { imagemagic = BitConverter.ToUInt32(imagebytes, 0); } catch (Exception) { }

                string Extension = "error";

                
                if (imagemagic == 0x2B435450) { Extension = "ptc"; }
                
                
                File.WriteAllBytes(Path.Combine(baseDirectory, baseName, i.ToString("D4") + "." + Extension), imagebytes);
                Console.WriteLine("Extracted file: " + baseName + "\\" + i.ToString("D4") + "." + Extension);
            }
            fix.Close();

            var idx = new FileInfo(Path.Combine(baseDirectory, baseName + ".idxbigfix")).CreateText();
            idx.WriteLine("# RE4 FIX TOOL");
            idx.WriteLine("# By: JADERLINK");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("# github.com/JADERLINK");
            idx.WriteLine("# only info = count: " + imgList.Count);
            idx.Close();
        }
    }
}
