namespace HashCode
{
    class Project
    {
        public string name { get; }
        public int duration { get; set; }
        public int score { get; set; }
        public int bestBefore { get; }
        public List<Skill> roles { get; }
        public double sortValue { get; set; }
        public Project(string projectName, int projectDuration, int projectScore, int projectBestBefore, List<Skill> projectRoles)
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
            Console.WriteLine(name + " has score " + sortValue);
        }

        public int CompareTo(Project y, int t)
        {
            return sortValue - y.sortValue < 0 ? 1 : sortValue == y.sortValue ? 0 : -1;
        }
    }
}