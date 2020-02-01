using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers
{
    public class Option<T> : IOption<T>
    {
        public Option(T option)
        {
            this.option = option;
        }
        public T option { get; }
    }
}
