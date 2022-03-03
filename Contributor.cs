using System.Collections.Generic;

namespace HashCode
{
    class Contributor
    {
        string name { get; }
        List<Skill> skillset { get; }

        public Contributor(string contributorName, List<Skill> contributorSkillset)
        {
            name = contributorName;
            skillset = contributorSkillset;
        }
    }
}