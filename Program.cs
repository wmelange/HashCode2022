using System;
using System.IO;
using System.Collections.Generic;

namespace HashCode
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\Users\wille\OneDrive\Documents\Hash Code\data", "*.in.txt");
            foreach (var file in files)
            {
                Console.WriteLine("Start processing " + file);
                List<Contributor> contributors;
                List<Project> projects;
                List<Project> projectsDone = new List<Project>();
                
                ReadInFile(file, out contributors, out projects); 

                /*foreach(var contributor in contributors) 
                {
                    Console.WriteLine($"Contributor {contributor.name} started with skillset:");
                    foreach(var skill in contributor.skillset)
                    {
                        Console.WriteLine($"Skill {skill.name} of level {skill.level}");
                    }
                }*/ 

                int currentTime = 0;

                int previousCount = projects.Count;
                while(projects.Count > 0)
                {
                    Console.WriteLine($"Current time is {currentTime}");
                    Console.WriteLine("Sort projects");
                                        
                    projects.ForEach(project => project.SetSortValue(0));
                    projects.Sort((x,y) => x.CompareTo(y,0)); 

                    Console.WriteLine($"Assign contributors to {projects.Count()} projects");
                                        
                    //Usefull for assignment
                    foreach(var project in projects)
                    {
                        var possibleContributors = contributors.Where(contributor => contributor.daysBusy == 0);
                        if(project.AssignCandidates(ref possibleContributors))
                        {
                            projectsDone.Add(project);
                        }
                    }

                    projects.RemoveAll(project => project.done);
                    Console.WriteLine("Skip days");
                    
                    if(contributors.Where(contributor => contributor.daysBusy == 0).Count() == contributors.Count())                  
                    {
                        break;
                    }
                    previousCount = projects.Count;

                    //TODO make this smarter
                    var busyContributors = contributors.Where(contributor => contributor.daysBusy != 0);
                    int minStep = busyContributors.Min(contributor => contributor.daysBusy);
                                            
                    currentTime = currentTime + minStep;
                    
                    foreach(var contributor in busyContributors) 
                    {
                        contributor.daysBusy = contributor.daysBusy - minStep;
                    }  
                } 

                /*foreach(var project in projectsDone) 
                {
                    Console.WriteLine($"Project {project.name} is done");
                    foreach(var role in project.roles)
                    {
                        Console.WriteLine($"Role {role.skill.name} is done by {role.assignedContributor}");
                    }
                } 

                foreach(var contributor in contributors) 
                {
                    Console.WriteLine($"Contributor {contributor.name} finished with skillset:");
                    foreach(var skill in contributor.skillset)
                    {
                        Console.WriteLine($"Skill {skill.name} of level {skill.level}");
                    }
                }*/  
                Console.WriteLine("Start writing output to file");
                WriteOutFile(file, projectsDone);                    
            }                              

            Console.WriteLine("Finished processing file!");
        }

        static void ReadInFile(string file, out List<Contributor> contributors, out List<Project> projects) 
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
                List<Role> roles = new List<Role>();
                for(int j = 0; j < nrOfRoles; j++)
                {
                    splittedLine = lines[i].Split(' ');
                    string skillName = splittedLine[0];
                    int level = Int32.Parse(splittedLine[1]);
                    roles.Add(new Role(skillName, level));
                    i++;
                }
                projects.Add(new Project(projectName, duration, score, bestBefore, roles));
                nrOfProjectsProcessed++;
            }

            Console.WriteLine("Finished reading file!");
        }

        static async void WriteOutFile(string filePath, List<Project> projectsDone)
        {
            using StreamWriter file = new(filePath.Replace("in", "out"));
            await file.WriteLineAsync(projectsDone.Count.ToString());
            foreach(var project in projectsDone)
            {
                await file.WriteLineAsync(project.name);
                await file.WriteLineAsync(string.Join(" ", project.roles.Select(role => role.assignedContributor)));
            }
        }
    }
}