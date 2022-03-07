using System.Collections.Generic;

namespace HashCode
{
    class Contributor
    {
        public string name { get; }
        public List<Skill> skillset { get; }
        public int daysBusy { set; get; }

        public Contributor(string contributorName, List<Skill> contributorSkillset)
        {
            name = contributorName;
            skillset = contributorSkillset;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Contributor objAsPart = obj as Contributor;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        
        public bool Equals(Contributor other)
        {
            if (other == null) return false;
            return this.name.Equals(other.name);
        }
        // Should also override == and != operators.
    }
}