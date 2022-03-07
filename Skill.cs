namespace HashCode
{
    class Skill
    {
        public string name {get;}
        public int level {get; set;}

        public Skill(string skillName, int skillLevel)
        {
            name = skillName;
            level = skillLevel;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Skill objAsPart = obj as Skill;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public override int GetHashCode()
        {
            int hashCode = level.GetHashCode();
            hashCode = hashCode ^ name.GetHashCode();
            return hashCode;
        }

        public bool Equals(Skill other)
        {
            if (other == null) return false;
            return (this.name.Equals(other.name) && (this.level == other.level));
        }
        // Should also override == and != operators.
    }
}