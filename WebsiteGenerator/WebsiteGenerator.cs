using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace WebsiteGenerator
{
    /// <summary>
    /// Controls the creation of the websites. Handles menu choise.
    /// Holds lists of websites.
    /// </summary>
    class WebsiteGenerator : Website, IFileHandler
    {
        private static int numOfSavedFiles;
        public static int numOfSchoolWebsites = 0;
        public static int numOfStyledSchoolWebsites = 0;
        private int numOfWebsites;

        public WebsiteGenerator()
        {
            Start();
        }

        private void Start()
        {
            // Call menu until user chooses to quit
            while (getMenuChoise() != 0) ;
        }


        /// <summary>
        /// Show menu and get menu choise from user.
        /// </summary>
        /// <returns>The menu choise number</returns>
        private int getMenuChoise()
        {
            const int MAX_MENU_CHOISE = 6;
            int menuChoise = -1;

            // Count saved files before showing menu
            UpdateAndCountSavedWebsites();
            do
            {
                // Choose what to do
                numOfWebsites = numOfSchoolWebsites + numOfStyledSchoolWebsites;
                Console.Clear();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("VÄLKOMMEN TILL DEN SUPERROLIGA HEMSIDEGENERATORN (jippy.)");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Vad vill du göra?");
                Console.WriteLine($"\t1. Skapa websida till en skola\t\t({numOfSchoolWebsites} genererade osparade)");
                Console.WriteLine($"\t2. Skapa stylad websida till en skola\t({numOfStyledSchoolWebsites} genererade osparade)");
                Console.WriteLine($"\t3. Visa alla nya genererade websidor\t({numOfWebsites} genererade osparade)");
                Console.WriteLine("\t4. Spara alla websidor till fil");
                Console.WriteLine($"\t5. Lista sparade websidor\t\t({numOfSavedFiles} sparade filer)");
                Console.WriteLine("\t6. Se innehåll i alla sparade websidor");
                Console.WriteLine("\t0. Avsluta");

                Console.Write("\nVälj: ");
            } while (!Int32.TryParse(Console.ReadLine(), out menuChoise) || menuChoise < 0 || menuChoise > MAX_MENU_CHOISE);

            switch (menuChoise)
            {
                case 1:
                    // Create normal website and enter info by calling constructor                    
                    allWebsites.Add(new SchoolWebsite());
                    numOfSchoolWebsites++;
                    break;
                case 2:
                    // Create styled website and enter info by calling constructor
                    allWebsites.Add(new StyledSchoolWebsite());
                    numOfStyledSchoolWebsites++;
                    ConfirmWebsiteCreated();
                    break;
                case 3:
                    //Show all websites
                    ShowAllWebsites();
                    break;
                case 4:
                    // Save all websites to files
                    SaveWebsitesToFiles();
                    break;
                case 5:
                    UpdateAndCountSavedWebsites();
                    ListSavedWebsites();
                    break;
                case 6:
                    ShowAllSavedWebsiteContent();
                    break;
                case 0:
                    // Quit: Save all websites? y/n -quit
                    Console.WriteLine("BYE!");
                    break;
            }
            return menuChoise;
        }

        public Website GetWebsite(int index)
        {
            return allWebsites[index];
        }

        /// <summary>
        /// Show all the newly generated websites from this session (with name header)
        /// </summary>
        private static void ShowAllWebsites()
        {
            if (allWebsites.Count == 0)
            {
                Console.WriteLine("Inga genererade websidor att visa. Du kanske har sparat dem?");
                Console.ReadKey();
            }
            foreach (Website website in allWebsites)
            {
                Console.WriteLine("\n------------------------");
                Console.WriteLine(website.GetWebsiteName());
                Console.WriteLine("------------------------");
                Console.WriteLine(website.GenerateWebsiteString());
                Console.WriteLine();
                Console.ReadLine();
            }
        }

        public override string GenerateWebsiteString()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Saves the generated websites to separate files
        /// </summary>
        public void SaveWebsitesToFiles()
        {
            // Iterate through all newly generated websites
            foreach (Website website in allWebsites)
            {
                // Set filename same as websiteName
                string fileName = website.GetWebsiteName();

                // Create the file and write the generated HTML-code to the file
                File.WriteAllText($"{fileName}.html", website.GenerateWebsiteString());

                // Add the saved files' filename to a list of all saved files
                File.AppendAllText(SAVED_WEBSITES_FILENAME, fileName + ".html\n");
            }
            // Remove all saved websites from List
            allWebsites.Clear();
            numOfWebsites = 0;
            numOfSchoolWebsites = 0;
            numOfStyledSchoolWebsites = 0;
        }


        /// <summary>
        /// List all the saved websites' files
        /// </summary>
        private void ListSavedWebsites()
        {
            // Íf Saved Websites.txt doesn't exist, output message
            if (!File.Exists(SAVED_WEBSITES_FILENAME))
            {
                Console.WriteLine("Inga sparade websidor");
                Console.ReadKey();
            }
            else
            {
                // If the file exists, output the saved files' file names
                Console.WriteLine("\n" + File.ReadAllText(SAVED_WEBSITES_FILENAME));
                Console.ReadLine();
            }
        }


        /// <summary>
        /// List the html-code for all saved websites
        /// </summary>
        public void ShowAllSavedWebsiteContent()
        {
            // Read one line at a time from Saved Websites.txt
            foreach (string savedFile in File.ReadAllLines(SAVED_WEBSITES_FILENAME))
            {
                // For each line, read the corresponding file's HTML content, line after line
                foreach (string line in File.ReadAllLines(savedFile))
                {
                    // Output the line
                    Console.WriteLine(line);
                }
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Update the saved websites file and count number of files
        /// if website files should be deleted from outside of the program
        /// </summary> 
        private void UpdateAndCountSavedWebsites()
        {
            numOfSavedFiles = 0;
            // If Saved Websites file exists, look through it to count the files, if they exist on the disk
            if (File.Exists(SAVED_WEBSITES_FILENAME))
            {
                // Store the strings to be removed later
                List<string> listToRemove = new List<string>();

                // Save file content into a List
                List<string> savedFilesList = new List<string>();

                foreach (string line in File.ReadAllLines(SAVED_WEBSITES_FILENAME))
                    savedFilesList.Add(line);                

                // Check each line in the Saved Websites file
                foreach (string line in savedFilesList)
                {
                    // If a file with that name exists on the disk, count it.
                    // Else remove that filename from Saved Webisites file.
                    if (File.Exists(line))
                    {
                        numOfSavedFiles++;
                    }
                    else
                        listToRemove.Add(line);
                }

                foreach (string line in listToRemove)
                    savedFilesList.Remove(line);

                // Rewrite Saved Website file with the content of the modified file name list
                File.Create(SAVED_WEBSITES_FILENAME).Close();

                foreach (string line in savedFilesList)
                    File.AppendAllText(SAVED_WEBSITES_FILENAME, line + "\n");

                savedFilesList.Clear();
                listToRemove.Clear();
            }
        }
    }
}

