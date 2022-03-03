using System;
using System.IO;
using System.Collections.Generic;

namespace HashCode
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\Users\wille\OneDrive\Documents\Hash Code\data", "a*.txt");
            foreach (var file in files)
            {
                Console.WriteLine("Start processing " + file);
                List<Contributor> contributors;
                List<Project> projects;
                
                readInFile(file, out contributors, out projects); 

                int currentTime = 0;

                //TODO: needs a better stopping condition when everything is implemented
                while(currentTime < 50)
                {
                    Console.WriteLine("Sort projects");
                    projects.ForEach(project => project.SetSortValue(0));
                    projects.Sort((x,y) => x.CompareTo(y,0)); 

                    foreach (var project in projects)
                    {
                        Console.WriteLine(project.name);
                    }     
                }                         
            }
            
            Console.WriteLine("Finished processing file!");
        }

        static void readInFile(string file, out List<Contributor> contributors, out List<Project> projects) 
        {
            contributors = new List<Contributor>();
            projects = new List<Project>();
            
            string[] lines = System.IO.File.ReadAllLines(file);
            string[] splittedLine = lines[0].Split(' ');
            int nrOfContributors = Int32.Parse(splittedLine[0]);
            int nrOfProjects = Int32.Parse(splittedLine[1]);
            
            //Read in contributors
            Console.WriteLine($"Start reading in {nrOfContributors} contributors");
            int i = 1;
            int nrOfContributorsProcessed = 0;
            while (nrOfContributorsProcessed < nrOfContributors) 
            {
                splittedLine = lines[i].Split(' ');
                string contributorName = splittedLine[0];
                int nrOfSkills = Int32.Parse(splittedLine[1]);
                List<Skill> skillset = new List<Skill>();
                i++;
                for(int j = 0; j < nrOfSkills; j++)
                {
                    splittedLine = lines[i].Split(' ');
                    string skillName = splittedLine[0];
                    int level = Int32.Parse(splittedLine[1]);
                    skillset.Add(new Skill(skillName, level));
                    i++;
                }
                contributors.Add(new Contributor(contributorName, skillset));
                nrOfContributorsProcessed++;
            } 

            //Read in projects
            Console.WriteLine($"Start reading in {nrOfProjects} projects");
            int nrOfProjectsProcessed = 0;
            while (nrOfProjectsProcessed < nrOfProjects) 
            {
                splittedLine = lines[i].Split(' ');
                string projectName = splittedLine[0];
                int duration = Int32.Parse(splittedLine[1]);
                int score = Int32.Parse(splittedLine[2]);
                int bestBefore = Int32.Parse(splittedLine[3]);
                int nrOfRoles = Int32.Parse(splittedLine[4]);
                i++;
                List<Skill> roles = new List<Skill>();
                for(int j = 0; j < nrOfRoles; j++)
                {
                    splittedLine = lines[i].Split(' ');
                    string skillName = splittedLine[0];
                    int level = Int32.Parse(splittedLine[1]);
                    roles.Add(new Skill(skillName, level));
                    i++;
                }
                projects.Add(new Project(projectName, duration, score, bestBefore, roles));
                nrOfProjectsProcessed++;
            }

            Console.WriteLine("Finished reading file!");
        }
    }
}