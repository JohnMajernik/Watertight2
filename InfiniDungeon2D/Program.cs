using SFML.Graphics;
using System;
using System.IO;
using Watertight;
using Watertight.Filesystem;

namespace InfiniDungeon2D
{
   

    class Program
    {
        static void Main(string[] args)
        {
            InfiniDungeonEngine Engine = new InfiniDungeonEngine();

            Engine.Init();
            Engine.Run();
        }
    }
}
