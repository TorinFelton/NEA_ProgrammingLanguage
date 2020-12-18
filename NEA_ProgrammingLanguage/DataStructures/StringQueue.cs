using System;

namespace DataStructures
{
    class StringQueue
    {
        private string raw_value = "";
        private int index = 0;        
        public StringQueue(string value)
        {
            raw_value = value;
        }

        public char Next()
        {
            if (raw_value.Length > 0 && index < raw_value.Length)
            {
                return raw_value[index];
            }
            else throw new IndexOutOfRangeException();
        }

        public char MoveNext()
        {
            return raw_value[index++]; // Returns index THEN increments it
        }

        public bool More()
        {
            return index < raw_value.Length;
        }

        public override string ToString()
        {
            return raw_value;
        }
    }
}
