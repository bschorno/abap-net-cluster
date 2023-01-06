namespace ABAPNet.Cluster
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException() 
            : base() { }

        public InvalidTypeException(string message) 
            : base(message) { }

        public InvalidTypeException(object involvedObject, params Type[] expectedTypes)
            : base(string.Format("Invalid type {0}, expected types where {1}", involvedObject.GetType(), string.Join<Type>(',', expectedTypes))) { }
    }
}
