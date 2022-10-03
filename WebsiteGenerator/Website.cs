using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebsiteGenerator
{
    /// <summary>
    /// Defines the basic strings for the website generator
    /// </summary>
    abstract class Website : IWebsite
    {
        protected const string SAVED_WEBSITES_FILENAME = "Saved Websites.txt";
        private const int FILE_EXTENSION_LENGTH = 4;
        protected string beginningStr;
        protected string endStr;
        private string websiteName;
        protected static List<Website> allWebsites = new List<Website>();

        /// <summary>
        /// Set website name if not already exists
        /// </summary>
        /// <param name="name">The website name</param>
        /// <returns></returns>
        public bool SetWebsiteName(string name)
        {
            if (File.Exists("Saved Websites.txt"))
            {
                // Check if file with the chosen name already exits
                foreach (string line in File.ReadAllLines(SAVED_WEBSITES_FILENAME))
                {
                    if (name == line.Substring(0, line.Length - FILE_EXTENSION_LENGTH))
                        return false;
                }

                // Check if a generated unsaved website already exists with the chosen name
                for (int i = 0; i < allWebsites.Count; i++)
                {
                    if (allWebsites[i].GetWebsiteName() == name)
                    {
                        return false;
                    }
                }
            }
            this.websiteName = name;
            return true;
        }


        public string GetWebsiteName()
        {
            return websiteName;
        }


        public abstract string GenerateWebsiteString();


        public string GetBeginning()
        {
            return beginningStr;
        }


        public string GetEnding()
        {
            return endStr;
        }


        protected void ConfirmWebsiteCreated()
        {
            Console.WriteLine("\nWebsida skapad. Tryck valfri knapp.");
            Console.ReadKey();
        }
    }
}
