using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InternetHerokuapp
{
    public class WebDriverHelper : IDisposable
    {
        private IWebDriver _webDriver;

        public IWebDriver GetDriver()
        {
            if (_webDriver == null)
            {
                _webDriver = new ChromeDriver(@"C:\Users\Vitalina\Documents\Visual Studio 2013\Projects\InternetHerokuapp\InternetHerokuapp\bin\Debug");
            }
            return _webDriver;
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Close();
            }
        }


    }
}
