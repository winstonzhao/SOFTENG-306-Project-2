using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstruction
{

    void Execute(Instructable target, InstructionExecutor executor);

    void Update();

}
