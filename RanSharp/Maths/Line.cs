namespace RanSharp.Maths
{
    /// <summary>
    /// A struct that represents a line between 2 points. The points can be of any type.
    /// </summary>
    public readonly struct Line<T>
    {
        /// <summary>
        /// The first point.
        /// </summary>
        public readonly T A;
        /// <summary>
        /// The second point.
        /// </summary>
        public readonly T B;
        private readonly Dictionary<int, int> _attributes;
        /// <summary>
        /// Creates a new line between 2 points. It is also possible to add attributes to the line.
        /// If attributes are needed, the attribute (<see cref="int"/>) and its value (<see cref="int"/>) should be specified together.
        /// It is advised to implement an enum (specifying their <see cref="int"/> values as well) for the attributes.
        /// </summary>
        public Line(T a, T b, int? attr = null, int? value = null)
        {
            A = a;
            B = b;
            _attributes = new();
            if (attr is int @iattr && value is int @ivalue) _attributes[iattr] = ivalue;
        }
        /// <summary>
        /// Gets or sets the value of the specified attribute (<see cref="int"/>).
        /// It is advised to implement an enum (specifying their <see cref="int"/> values as well) for the attributes.
        /// </summary>
        public int this[int attr]
        {
            get => _attributes[attr];
            set => _attributes[attr] = value;
        }
    }
}
