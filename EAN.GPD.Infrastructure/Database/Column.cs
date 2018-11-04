using System;

namespace EAN.GPD.Infrastructure.Database
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class Column : Attribute
    {
        public string Name { get; set; }
        public bool StringNotNullable { get; set; }
        public ushort StringMaxLenght { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class JoinColumn : Attribute
    {
        public string NameColumnReference { get; set; }

        public JoinColumn(string nameColumnReference)
        {
            NameColumnReference = nameColumnReference;
        }
    }
}
