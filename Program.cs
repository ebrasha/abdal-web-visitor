using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Reflection;

class Program
{
    static void Main()
    {
        // Get Software Version and Change Title
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        Console.Title = "Abdal Web Visitor :: Ver " + version;

        Console.WriteLine("");
        Console.WriteLine("Programmer: Ebrahim Shafiei (EbraSha)");
        Console.WriteLine("Email: Prof.Shafiei@Gmail.com");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("##################################");
        Console.WriteLine("");
        // Read settings
        var settingsXml = XDocument.Load("settings.xml");
        bool infinite = bool.Parse(settingsXml.Root.Element("Infinite").Value);
        int loadWaitTime = int.Parse(settingsXml.Root.Element("LoadWaitTime").Value);
        bool enableScroll = bool.Parse(settingsXml.Root.Element("EnableScroll").Value);
        int scrollWaitTime = int.Parse(settingsXml.Root.Element("ScrollWaitTime").Value);
        int idleTimeAfterScroll = int.Parse(settingsXml.Root.Element("IdleTimeAfterScroll").Value);

        // Read URLs
        var urlsXml = XDocument.Load("urls.xml");
        List<string> urls = urlsXml.Root.Elements("url").Select(x => x.Value).ToList();

        // Open browser
        IWebDriver driver = new ChromeDriver(".");

        try
        {
            do
            {
                foreach (var url in urls)
                {
                    driver.Navigate().GoToUrl(url);
                    Thread.Sleep(loadWaitTime);

                    if (enableScroll)
                    {
                        // Scroll to the middle of the page
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight/2)");

                        Thread.Sleep(scrollWaitTime);

                        // Scroll to the end of the page
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

                        Thread.Sleep(idleTimeAfterScroll);
                    }
                }
            } while (infinite);
        }
        finally
        {
            driver.Quit();
        }
    }
}
