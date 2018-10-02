using System;

namespace Instructions
{
    public class InstructionException : Exception
    {
        public InstructionException(string message) : base(message)
        {
        }
    }
}