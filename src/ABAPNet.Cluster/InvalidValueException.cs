namespace ABAPNet.Cluster
{
    internal class InvalidValueException : Exception
    {
        public InvalidValueException()
            : base() { }

        public InvalidValueException(string message)
            : base(message) { }

        public InvalidValueException(object value, string message)
            : base(string.Format("{{0}} {1}", value, message)) { }
    }
}
