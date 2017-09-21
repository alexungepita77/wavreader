using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            BinaryReader br = new BinaryReader(File.Open("piano2.wav",FileMode.Open));
            BinaryWriter bw= new BinaryWriter(File.Create("test.wav"));
            MuteRightSpeaker(br,bw);
            br.Close();
            bw.Close();
        }
        static void WriteFile(string filename, string msg)
        {
          
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(msg);
            }
                
       


        }

        static void ReadFile(string filename)
        {
            using (StreamReader Reader = new StreamReader(filename))
            {
              
                while (Reader.Peek() >= 0) 
                {
                    Console.WriteLine(Reader.ReadLine());
                }
               
            }
                
            
        }

        static void MirrorWritting(string input, string output)
        {
            using (StreamReader Reader2 = new StreamReader(input))
            {
                using (StreamWriter Writer2 = new StreamWriter(output))
                {
                    string Content;

                    while (Reader2.Peek() >= 0)
                    {


                        Content = Reader2.ReadLine();
                        int i = Content.Length;
                        while (i != 0)
                        {
                            Writer2.Write(Content[i - 1]);
                            i = i - 1;

                        }
                        Writer2.Write("\n");
                    }
                }

            }

        }
        static bool CheckWav(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                string ChunkID, Format, SubChunk1ID, SubChunk2ID;
                ChunkID = Format = SubChunk1ID = SubChunk2ID = "";

                for (int i = 0; i <40; i++)
                {
                    if (i<4)
                    {
                        ChunkID += (char)reader.ReadByte();
                    }
                    else if (8<=i && i<12)
                    {
                        Format += (char) reader.ReadByte();
                    }
                    else if (12<=i && i<16)
                    {
                        SubChunk1ID += (char) reader.ReadByte();
                    }
                    else if (i >= 36)
                    {
                        SubChunk2ID += (char) reader.ReadByte();
                    }

                    else
                    {
                        reader.ReadByte();
                    }
                }
                
                return ChunkID == "RIFF" && Format == "WAVE" && SubChunk1ID == "fmt " && SubChunk2ID == "data";

            }
            
        }

        static string PrintInfo(int i, int j, Stream s)
        {
            string str = "";
            s.Seek(i, SeekOrigin.Begin);
            for (int k = i; k < j; k++)
            {
                str += (char) s.ReadByte();
            }
            return str;
        }

        static void InitializeHeader(BinaryReader br, BinaryWriter bw)
        {
            Stream s = br.BaseStream;
            s.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < 36; i++)
            {
                bw.Write(br.ReadByte());
            }
            string data = PrintInfo(36,40,s);
            if (data!="data")
            {
                Console.WriteLine("Error");
                return;
            }
            s.Seek(36, SeekOrigin.Begin);
            for (int i = 0; i < 8; i++)
            {
                bw.Write(br.ReadByte());
            }
            
        }

        static void MuteRightSpeaker(BinaryReader br, BinaryWriter bw)
        {
            InitializeHeader(br, bw);
            Stream s = br.BaseStream;
            s.Seek(44, SeekOrigin.Begin);
            //bool shift = false;
            for (int i = 0; s.Position < s.Length - 1 ; i++)
            {
                //if (i % 10000 == 0)
               // {
                //    shift = !shift;
                //}
                //if (i % 2 != 0 + Convert.ToInt16(shift))
                if (i % 2 != 0)
                {
                    bw.Write((short)0);
                    br.ReadInt16();
                }
                else
                {
                    bw.Write(br.ReadInt16());
                }
            }
        }
    }
    
}
