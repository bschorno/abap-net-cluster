using ABAPNet.Cluster.Attributes;

namespace ABAPNet.Cluster.Test
{
    internal class TestIfcModelEntity
    {
        public static Cluster GetCluster()
        {
            return new Cluster()
            {
                BuildingElements = new IfcBuildingElements()
                {
                    Elements = new Element[0],
                    PropertySets = new PropertySet[0],
                    QuantitySets = new QuantitySet[0]
                }
            };
        }

        internal struct Cluster
        {
            [ClusterFieldName("STRUCT")]
            [DeepStruct]
            public IfcBuildingElements BuildingElements { get; set; }
        }

        internal struct IfcBuildingElements
        {
            [DeepTable]
            public Element[] Elements { get; set; }

            [DeepTable]
            public PropertySet[] PropertySets { get; set; }

            [DeepTable]
            public QuantitySet[] QuantitySets { get; set; }
        }

        internal struct Element
        {
            [Char(60)]
            public string Tag { get; set; }

            [Char(22)]
            public string Guid { get; set; }

            [String]
            public string Name { get; set; }

            [String]
            public string Description { get; set; }

            [String]
            public string ObjectType { get; set; }

            [CharTable(22)]
            public string[] PropertySets { get; set; }

            [CharTable(22)]
            public string[] QuantitySets { get; set; }
        }

        internal struct PropertySet
        {
            [Char(60)]
            public string Tag { get; set; }

            [Char(22)]
            public string Guid { get; set; }

            [String]
            public string Name { get; set; }

            [String]
            public string Description { get; set; }

            [DeepTable]
            public Property[] Properties { get; set; }
        }

        internal struct Property
        {
            [Char(60)]
            public string Tag { get; set; }

            [String]
            public string Name { get; set; }

            [String]
            public string Description { get; set; }

            [DeepStruct]
            public PrpQtyValue Value { get; set; }
        }

        internal struct QuantitySet
        {
            [Char(60)]
            public string Tag { get; set; }

            [Char(22)]
            public string Guid { get; set; }

            [String]
            public string Name { get; set; }

            [String]
            public string Description { get; set; }

            [DeepTable]
            public Quantity[] Quantities { get; set; }
        }

        internal struct Quantity
        {
            [Char(60)]
            public string Tag { get; set; }

            [String]
            public string Name { get; set; }

            [String]
            public string Description { get; set; }

            [DeepStruct]
            public PrpQtyValue Value { get; set; }
        }

        internal struct PrpQtyValue
        {
            [Char(100)]
            public string Type { get; set; }

            [String]
            public string Value { get; set; }

            [String]
            public string Unit { get; set; }
        }
    }
}
