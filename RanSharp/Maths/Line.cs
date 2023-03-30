namespace RanSharp.Maths
{
    public readonly struct Line<T>
    {
        public readonly T A;
        public readonly T B;
        private readonly Dictionary<int, int> _attributes;
        public Line(T a, T b, int? attr = null, int? value = null)
        {
            A = a;
            B = b;
            _attributes = new();
            if (attr is int @iattr && value is int @ivalue) _attributes[iattr] = ivalue;
        }
        public int this[int attr]
        {
            get => _attributes[attr];
            set => _attributes[attr] = value;
        }
    }
}
