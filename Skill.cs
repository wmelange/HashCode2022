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
    }
}