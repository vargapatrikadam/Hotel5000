using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helpers
{
    public class Setting<T> : ISetting<T>
    {
        public Setting(T option)
        {
            Option = option;
        }

        public T Option { get; }
    }
}