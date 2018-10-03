using System;

namespace Instructions
{
    public class InstructionException : Exception
    {
        public InstructionException() : base()
        {

        }

        public InstructionException(string message) : base(message)
        {
        }
    }
}