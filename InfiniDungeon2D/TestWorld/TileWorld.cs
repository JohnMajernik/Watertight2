using System;
using System.Collections.Generic;
using System.Text;
using Watertight.Framework;

namespace InfiniDungeon2D.TestWorld
{
    public class TileWorld : Actor
    {
        static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        int[,] TileMap = new int[32,32];

    }
}
