using System;

namespace RoboticistsApis.Models.Wrapper
{
    public class PostId
    {
        public Guid Value { get; }

        public PostId(Guid value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}