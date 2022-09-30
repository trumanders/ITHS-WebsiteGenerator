using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace WebsiteGenerator
{
    class StyledSchoolWebsite : SchoolWebsite
    {
        public static int numOfStyledSchoolWebsites;
        private string color;
        

        public StyledSchoolWebsite()
        {            
            Console.WriteLine("Välj en färg till websidan: ");
            this.color = Console.ReadLine();
            beginningStr = $"<!DOCTYPE html>\n<html>\n<head>\n<title>{GetWebsiteName()}</title>\n<style>\np {{ color: {color}; }}\n</style>\n</head>\n<body>\n<main>\n";
            numOfSchoolWebsites++;
    }

        public override string GenerateWebsiteString()
        {            
            return GetBeginning() + GetWelcome() + GetStringFromList(messages) + GetStringFromList(courses) + GetEnding();
        }
    }
}
