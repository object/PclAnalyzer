using System.Reflection;

namespace PclAnalyzer.Reflection.Tests
{
    public class ClassToParse
    {
        public string Name { get; set; }

        public string GetAssemblyLocationOneStep()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        public string GetAssemblyLocationTwoSteps()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.Location;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is ClassToParse)
                return this.Name.Equals((obj as ClassToParse).Name);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}