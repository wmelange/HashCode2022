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
            List<Contributor> assignment = new List<Contributor>();
            //Exactly match the levels as much as possible
            foreach(var role in roles)
            {
                role.assignedContributor = "";
                var possibleContributors = availableContributors.Where(contributor => contributor.skillset.Contains(role.skill));
                role.assignedContributor = checkAndAssign(possibleContributors, ref assignment);      
            }
            //Fill in roles that are not assigned yet
            foreach(var role in roles.Where(role => role.assignedContributor == ""))
            {
                var possibleContributors = availableContributors.Where(contributor => 
                    contributor.skillset.Where(skill => (role.skill.name == skill.name) && (role.skill.level <= skill.level)).Count() > 0);
                role.assignedContributor = checkAndAssign(possibleContributors, ref assignment);                          
            }
            if(roles.Count() == assignment.Count())
            {
                foreach(var contributor in assignment)
                {
                    contributor.daysBusy = duration; 
                    var skillRole = roles.Where(role => role.assignedContributor.Equals(contributor.name)).ElementAt(0);
                    var skillContributor = contributor.skillset.Where(skill => skill.name.Equals(skillRole.skill.name)).ElementAt(0);
                    if(skillContributor.level <= skillRole.skill.level)
                    {
                        skillContributor.level++;
                    }
                }
                done = true;
                return true;
            }    

            return false;
        }

        public string checkAndAssign(IEnumerable<Contributor> possibleContributors, ref List<Contributor> assignment)
        {
            //Make sure we didn't use the contributor already
            foreach(var contributor in possibleContributors)
            {
                if(!assignment.Contains(contributor))
                {
                    assignment.Add(contributor);
                    return contributor.name;
                }
            }     
            return ""; 
        }
    }
}