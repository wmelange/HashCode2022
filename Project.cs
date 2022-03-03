namespace HashCode
{
    class Project
    {
        public string name { get; }
        public int duration { get; set; }
        public int score { get; set; }
        public int bestBefore { get; }
        public List<Skill> roles { get; }
        public Project(string projectName, int projectDuration, int projectScore, int projectBestBefore, List<Skill> projectRoles)
        {
            name = projectName;
            duration = projectDuration;
            score = projectScore;
            bestBefore = projectBestBefore;
            roles = projectRoles;
        }
    }
}