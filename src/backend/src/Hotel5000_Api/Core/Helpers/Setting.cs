using Core.Interfaces;

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