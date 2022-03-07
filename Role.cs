namespace HashCode
{
    class Role
    {
        public Skill skill {get; set;}
        public string assignedContributor {get;set;}

        public Role(string skillName, int skillLevel)
        {
            skill = new Skill(skillName, skillLevel);
            assignedContributor = "";
        }
    }
}