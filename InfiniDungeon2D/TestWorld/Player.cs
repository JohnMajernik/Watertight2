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
        Vector3 ControlInput
        {
            get;
            set;
        }


        public override void BeginPlay()
        {
            InputProcessor.BindInput("Up", ()=> { VerticalControl(1); }, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Up", () => { VerticalControl(-1); }, InputProcessor.InputEvent.Released);

            InputProcessor.BindInput("Down", () => { VerticalControl(-1); }, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Down", () => { VerticalControl(1); }, InputProcessor.InputEvent.Released);

            InputProcessor.BindInput("Left", () => { HorizontalControl(1); }, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Left", () => { HorizontalControl(-1); }, InputProcessor.InputEvent.Released);

            InputProcessor.BindInput("Right", () => { HorizontalControl(-1); }, InputProcessor.InputEvent.Pressed);
            InputProcessor.BindInput("Right", () => { HorizontalControl(1); }, InputProcessor.InputEvent.Released);

            base.BeginPlay();
        }

        public void VerticalControl(float Dir)
        {
            ControlInput += new Vector3(0, Dir, 0);
        }

        public void HorizontalControl(float Dir)
        {
            ControlInput += new Vector3(Dir, 0, 0);
        }

        public override void Tick(float DeltaTime)
        {
            Location += ControlInput * 1 * DeltaTime;
            base.Tick(DeltaTime);
        }

    }
}
