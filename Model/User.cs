using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class User : BaseEntity
    {
        private string name;
        private int age;

        public override string ToString()
        {
            return base.ToString() + $"Name: {name}, Age: {age}";
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }
    }
}
