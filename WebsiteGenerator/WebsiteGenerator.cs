using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
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
        // Create lists for the different website types
        private const string SAVED_WEBSITES_FILE = "Saved Websites.txt";
        private static List<Website> allWebsites = new List<Website>();
        private static int numOfSavedFiles;

        public WebsiteGenerator()
        {
            Start();
        }

        private void Start()
        {
            // Call menu until user chooses to quit
            while (getMenuChoise() != 0);     
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
            
            do
            {
                // Choose what to do
                Console.Clear();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("VÄLKOMMEN TILL DEN SUPERROLIGA HEMSIDEGENERATORN (jippy.)");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Vad vill du göra?");
                Console.WriteLine($"\t1. Skapa websida till en skola ({numOfSchoolWebsites} genererade osparade)");
                Console.WriteLine($"\t2. Skapa stylad websida till en skola ({numOfStyledSchoolWebsites} genererade osparade)");
                Console.WriteLine($"\t3. Visa alla nya genererade websidor ({numOfWebsites} genererade osparade)");
                Console.WriteLine("\t4. Spara alla websidor till fil");
                Console.WriteLine($"\t5. Lista sparade websidor ({numOfSavedFiles} sparade)");
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
                    ConfirmWebsiteCreated();
                    numOfStyledSchoolWebsites++;
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
                    ListSavedWebsites();
                    break;
                case 6:
                    ShowAllSavedWebsiteContent();
                    break;
                case 0:
                    // Quit: Save all websites? y/n -quit
                    Console.WriteLine("BYE!");
                    break;
                default:
                    numOfWebsites = numOfSchoolWebsites + numOfStyledSchoolWebsites;
                    break;
            }
            return menuChoise;
        }


        /// <summary>
        /// Show all the newly generated websites from this session (with name header)
        /// </summary>
        private static void ShowAllWebsites()
        {
            if (allWebsites.Length == 0)
                Console.WriteLine("Inga genererade websidor att visa. Du kanske har sparat dem?");
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
                File.WriteAllText($"{fileName}.txt", website.GenerateWebsiteString());

                // Add the saved files' filename to a list of all saved files
                File.AppendAllText(SAVED_WEBSITES_FILE, fileName + ".txt\n");
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
            if (!File.Exists(SAVED_WEBSITES_FILE))
            {
                Console.WriteLine("Inga sparade websidor");
                Console.ReadKey();
            }
            else
            {   
                // If the file exists, output the saved files' file names
                Console.WriteLine("\n" + File.ReadAllText(SAVED_WEBSITES_FILE));
                Console.ReadLine();
            }
        }


        /// <summary>
        /// List the html-code for all saved websites
        /// </summary>
        public void ShowAllSavedWebsiteContent()
        {
            // Read one line at a time from Saved Websites.txt
            foreach (string savedFile in File.ReadAllLines(SAVED_WEBSITES_FILE))
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

        // Update the saved websites file and count number of files
        // if website files should be deleted from outside of the program
        private void UpdateAndCountSavedWebsites()
        {
            string newString = "";
            try
            {
                foreach (string savedFile in File.ReadAllLines(SAVED_WEBSITES_FILE))
                {
                    Console.WriteLine(savedFile);
                    Console.ReadLine();
                    if (File.Exists(savedFile))
                        numOfSavedFiles++;
                    else
                    // Remove the filename from the list
                    {
                        string fileContent = File.ReadAllText(SAVED_WEBSITES_FILE);
                        int savedFileIndex = fileContent.IndexOf(savedFile);
                        newString += fileContent.Substring(0, savedFileIndex) + fileContent.Substring(savedFileIndex + savedFile.Length);
                        Console.WriteLine(newString);
                        Console.ReadLine();
                        File.WriteAllText(SAVED_WEBSITES_FILE, newString);
                    }
                }
            }
            catch
            {
                Console.WriteLine("unable to read file");
            }
        }
    }
}
