using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebsiteGenerator
{
    /// <summary>
    /// Defines the basic strings for the website generator
    /// 
    /// </summary>
    abstract class Website : IWebsite
    {
        private const int FILE_EXTENSION_LENGTH = 4;
        protected string beginningStr;
        protected string endStr;
        protected string websiteString;
        private string websiteName;
        protected static int numOfSchoolWebsites;
        protected static int numOfStyledSchoolWebsites;
        protected static int numOfWebsites;


        /// <summary>
        /// Constructor - set the beginning and end of the website.
        /// </summary>
        public Website()
        {
 
        }

        // Set website name if not already exists
        public bool SetWebsiteName(string name)
        {
            if (File.Exists("Saved Websites.txt"))
            {
                foreach (string line in File.ReadAllLines("Saved Websites.txt"))
                {
                    // Compare the passed file name to each line in Saved Websites (minus the file extension)
                    if (name == line.Substring(0, line.Length - FILE_EXTENSION_LENGTH))
                    {
                        Console.WriteLine("En website med detta namn finns redan. Välj ett annat namn.");
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
