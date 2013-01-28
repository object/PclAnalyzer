namespace PclAnalyzer.Core.Tests
{
    public class PortableClass
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is PortableClass)
                return this.Name.Equals((obj as PortableClass).Name);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}