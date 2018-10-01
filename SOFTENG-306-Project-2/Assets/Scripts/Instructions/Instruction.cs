﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionComponent
{
    public delegate void ComponnentClicked(object args);
    public ComponnentClicked OnComponentClicked { get; set; }
    public string Text { get; set; }

    public InstructionComponent(string text)
    {
        Text = text;
    }
}

public enum Directions
{
    Up, Down, Left, Right
}

public class DropdownInstructionComponent : InstructionComponent
{
    public List<string> values = new List<string> { "up", "down", "left", "right" };

    public DropdownInstructionComponent(string text) : base(text)
    {
    }
}

public abstract class Instruction : MonoBehaviour
{

    // Called once each time the instruction starts to be executed
    public abstract void Execute(Instructable target, InstructionExecutor executor);

    // Called every update while the instruction is the current executing instruction
    // (Move to the next instruction with executor.ExecuteNextInstruction() when done)
    public abstract void UpdateInstruction();

    public abstract List<InstructionComponent> InstructionComponents { get; }

    public abstract bool Editable { get; set; }

}
