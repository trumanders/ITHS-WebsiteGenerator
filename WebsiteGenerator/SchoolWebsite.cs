using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Transactions;
using System.Xml.Xsl;

namespace WebsiteGenerator
{
    /// <summary>
    /// Handles information about school-websites
    /// </summary>
    class SchoolWebsite : Website
    {
        protected List<string> messages = new List<string>();
        protected List<string> courses = new List<string>();
        private string className;
        private string welcome;


        public SchoolWebsite()
        {
            SetInfo();
            beginningStr = $"<!DOCTYPE html>\n<html>\n<head>\n<title>{GetWebsiteName()}</title>\n<body>\n<main>\n";
            endStr = "</main>\n</body>\n</html>";
            this.welcome = $"<h1>Välkomna {className}!</h1>\n";
        }

        /// <summary>
        /// Let user enter name of website, name of the class, 
        /// all the courses and messages to the class.
        /// </summary>
        private void SetInfo()
        {
            // Set name of the website
            bool isValid = false;
            while (!isValid)
            {
                Console.Write("Välj ett namn på denna websidan: ");
                isValid = (SetWebsiteName(Console.ReadLine()));
                if (!isValid) Console.WriteLine("En websida med detta namn finns redan.\n");
            }

            // Set name of the class
            Console.Write("Ange klassens namn: ");
            this.className = Console.ReadLine();

            // Set courses
            EnterCourseOrMessage("kurs");
            EnterCourseOrMessage("meddelande");

            // Confirm that the website was created if it was of type SchoolWebsite (StyledSchoolWebsite constructor will not be called)
            if (this.GetType().ToString() == "WebsiteGenerator.SchoolWebsite")
                ConfirmWebsiteCreated();
        }


        /// <summary>
        /// Helper method to enter either courses or messages.
        /// </summary>
        /// <param name="typeOfInfo">Specifies if it is course or message that is entered</param>
        private void EnterCourseOrMessage(string typeOfInfo)
        {
            string answer = "";
            int count = 0;
            string typeOfInfoUppercase;
            do
            {
                count++;
                Console.Write($"Lägg in {typeOfInfo} för denna klass > ");
                typeOfInfoUppercase = typeOfInfo.Substring(0, 1).ToUpper() + typeOfInfo.Substring(1);

                //"Kurs 1: " or "Meddelande 1: "
                string inp = $"<p><b>{typeOfInfoUppercase} {count}:</b>" + Console.ReadLine() + "</p>\n";

                if (typeOfInfo == "kurs") courses.Add(inp);
                if (typeOfInfo == "meddelande") messages.Add(inp);
                do
                {
                    Console.Write("Lägg in fler (j/n) > ");
                    answer = Console.ReadLine();
                } while (answer != "j" && answer != "n");
            } while (answer == "j");
        }


        /// <summary>
        /// Get the welcome string.
        /// </summary>
        protected string GetWelcome()
        {
            return welcome;
        }

        /// <summary>
        /// Convert a List to a string
        /// </summary>
        /// <param name="strList">The List to convert to a string</param>
        /// <returns>A string</returns>
        protected string GetStringFromList(List<string> strList)
        {
            string outputString = "";
            for (int i = 0; i < strList.Count; i++)
                outputString += strList[i];
            return outputString;
        }

        /// <summary>
        /// Put together all strings to create the HTML-string
        /// </summary>
        /// <returns>The HTML-code as a string</returns>
        public override string GenerateWebsiteString()
        {
            string websiteToString = GetBeginning() + GetWelcome() + GetStringFromList(messages) + GetStringFromList(courses) + GetEnding();
            return websiteToString;
        }
    }
}
