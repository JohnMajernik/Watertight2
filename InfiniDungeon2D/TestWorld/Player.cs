using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Watertight.Framework;
using Watertight.Input;
using Watertight.SFML.Components;

namespace InfiniDungeon2D.TestWorld
{
    class Player : Actor
    {

        public override void BeginPlay()
        {
            InputProcessor.BindInput("Up", MoveUp, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Down", MoveDown, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Left", MoveLeft, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Right", MoveRight, InputProcessor.InputEvent.Pressed);

            base.BeginPlay();
        }

        public void MoveUp()
        {
            Location += new Vector3(0, 10, 0);
        }

        public void MoveDown()
        {
            Location += new Vector3(0, -10, 0);
        }

        public void MoveLeft()
        {
            Location += new Vector3(10, 0, 0);
        }

        public void MoveRight()
        {
            Location += new Vector3(-10, 0, 0);
        }
    }
}
