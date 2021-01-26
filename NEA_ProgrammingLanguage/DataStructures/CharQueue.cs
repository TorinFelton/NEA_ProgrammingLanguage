using System;

namespace DataStructures
{
    class CharQueue
    {
        private string raw_value = "";
        private int index = 0;

        public CharQueue(string value)
        {
            raw_value = value;
        }

        public char Next() // peek
        {
            if (raw_value.Length > 0 && index < raw_value.Length)
            {
                return raw_value[index];
            }
            else throw new IndexOutOfRangeException();
        }

        public char MoveNext() // pop
        {
            return raw_value[index++]; // Returns index THEN increments it
        }

        public bool More() // check if can peek or pop
        {
            return index < raw_value.Length;
        }

        public string Contents() { return raw_value; }
    }
}
