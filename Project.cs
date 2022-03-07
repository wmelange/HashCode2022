namespace HashCode
{
    class Project
    {
        public string name { get; }
        public int duration { get; set; }
        public int score { get; set; }
        public int bestBefore { get; }
        public List<Role> roles { get; }
        public double sortValue { get; set; }
        public bool done { get; set; }
        public Project(string projectName, int projectDuration, int projectScore, int projectBestBefore, List<Role> projectRoles)
        {
            name = projectName;
            duration = projectDuration;
            score = projectScore;
            bestBefore = projectBestBefore;
            roles = projectRoles;
        }

        public void SetSortValue(int t) {
            int timeLeft = bestBefore-duration-t;
            int tmpScore = timeLeft <= 0 ? score + timeLeft : score;
            sortValue = tmpScore <= 0 ? -duration : (double) tmpScore / duration / roles.Count / Math.Max(1,timeLeft);
            //Console.WriteLine(name + " has score " + sortValue);
        }

        public int CompareTo(Project y, int t)
        {
            return sortValue - y.sortValue < 0 ? 1 : sortValue == y.sortValue ? 0 : -1;
        }

        public bool AssignCandidates(ref IEnumerable<Contributor> availableContributors)
        {
            if(roles.Count() > availableContributors.Count()) 
            {
                return false;
            }            
            
            List<Contributor> assignment = new List<Contributor>();
            List<Skill> skillsToIncrease = new List<Skill>();
            //Console.WriteLine("Exactly match the levels as much as possible");
            Contributor assignedContributor = new Contributor("", new List<Skill>());
            foreach(var role in roles)
            {
                role.assignedContributor = "";
                var possibleContributors = availableContributors.Where(contributor => contributor.skillset.Contains(role.skill));
                if(checkAndAssign(ref assignedContributor, possibleContributors, ref assignment))
                {
                    role.assignedContributor = assignedContributor.name;
                    skillsToIncrease.Add(assignedContributor.skillset.Find(skill => role.skill.name.Equals(skill.name)));
                }    
            }
            //Console.WriteLine("Fill in roles that are not assigned yet");
            foreach(var role in roles.Where(role => role.assignedContributor == ""))
            {
                var possibleContributors = availableContributors.Where(contributor => 
                    contributor.skillset.Where(skill => (role.skill.name == skill.name) && (role.skill.level <= skill.level)).Count() > 0);
                if(checkAndAssign(ref assignedContributor, possibleContributors, ref assignment))
                {
                    role.assignedContributor = assignedContributor.name;                    
                }                          
            }
            //Console.WriteLine("Increase level if necessary");
            if(roles.Count() == assignment.Count())
            {
                foreach(var contributor in assignment)
                {
                    contributor.daysBusy = duration; 
                    foreach(var skill in skillsToIncrease)
                    {
                        skill.level++;
                    }
                }
                done = true;
                return true;
            }    

            return false;
        }

        public bool checkAndAssign(ref Contributor contributorOut, IEnumerable<Contributor> possibleContributors, ref List<Contributor> assignment)
        {
            //Make sure we didn't use the contributor already
            foreach(var contributor in possibleContributors)
            {
                if(!assignment.Contains(contributor))
                {
                    assignment.Add(contributor);
                    contributorOut = contributor;
                    return true;
                }
            }     
            return false; 
        }
    }
}