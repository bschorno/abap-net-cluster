namespace ABAPNet.Cluster.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ClusterFieldNameAttribute : Attribute
    {
        public string Name { get; set; }

        public ClusterFieldNameAttribute(string name)
        {
            Name = name.ToUpper();
            if (Name.Length > 132)
                throw new ArgumentException("Name can't be longer than 132 characters");
        }
    }
}
