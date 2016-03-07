using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using DataGrid = System.Windows.Forms.DataGrid;

namespace InternetHerokuapp
{
    [TestClass]
    public class HerokuappPageTests
    {
        [TestMethod]
        public void ABTesting()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/";

                IWebElement abTestingLink = webDriverHelper.GetDriver().FindElement(By.LinkText("A/B Testing"));

                abTestingLink.Click();

                IWebElement h3Tag = webDriverHelper.GetDriver().FindElement(By.TagName("h3"));
                string h3TextActual = h3Tag.Text;
                string h3TextExpected = "A/B Test Variation 1";

                IWebElement pTag = webDriverHelper.GetDriver().FindElement(By.TagName("p"));
                string pTextActual = pTag.Text;
                string pTextExpected = "Also known as split testing. This is a way" +
                                     " in which businesses are able to simultaneously" +
                                     " test and learn different versions of a page to see" +
                                     " which text and/or functionality works best towards a" +
                                     " desired outcome (e.g. a user action such as a click-through).";


                Assert.AreEqual(h3TextExpected, h3TextActual);
                Assert.AreEqual(pTextExpected, pTextActual);

            }
        }

        [TestMethod]
        public void BasicAuth()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                webDriverHelper.GetDriver().Url = "http://admin:admin@the-internet.herokuapp.com/basic_auth/";
                IWebElement congratulationTagP = webDriverHelper.GetDriver().FindElement(By.TagName("p"));
                string congratulationTextActual = congratulationTagP.Text;
                string congratulationTextExpected = "Congratulations! You must have the proper credentials.";

                Assert.AreEqual(congratulationTextExpected, congratulationTextActual);
            }
        }

        [TestMethod]
        public void BrokenImages()
        {

            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/";

                IWebElement brokenImagesLink = webDriverHelper.GetDriver().FindElement(By.LinkText("Broken Images"));
                brokenImagesLink.Click();

                IList<IWebElement> divContainers = webDriverHelper.GetDriver().FindElements(By.ClassName("example"));

                IWebElement divElement = divContainers.FirstOrDefault();

                List<IWebElement> emages = divElement.FindElements(By.TagName("img")).ToList();

                foreach (IWebElement image in emages)
                {
                    string src = image.GetAttribute("src");

                    RestClient restClient = new RestClient(src);

                    RestRequest restRequest = new RestRequest();

                    IRestResponse response = restClient.Execute(restRequest);
                    HttpStatusCode actualStatusCode = response.StatusCode;

                    Assert.AreEqual(HttpStatusCode.OK, actualStatusCode);
                }

            }
        }

        [TestMethod]
        public void ChallengingDom()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/challenging_dom";

                DataGridView dataGrid;
                ContextMenuStrip contextMenuStrip;

                //public Form1()
                //{
                //    InitializeComponent();

                //    dataGrid = new DataGridView();
                //    Controls.Add(dataGrid);
                //    dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                //    //dataGrid.ColumnHeaderMouseClick += ColumnHeaderMouseClick;
                //    dataGrid.MouseDown += MouseDown;
                //    dataGrid.DataSource = new Dictionary<string, string>().ToList();

                //    contextMenuStrip = new ContextMenuStrip();
                //    contextMenuStrip.Items.Add("foo");
                //    contextMenuStrip.Items.Add("bar");
                //}

                //private void ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
                //{
                //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                //    {
                //        contextMenuStrip.Show(PointToScreen(e.Location));
                //    }
                //}

                //private void MouseDown(object sender, MouseEventArgs e)
                //{
                //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                //    {
                //        if (dataGrid.HitTest(e.X, e.Y).Type == DataGridViewHitTestType.ColumnHeader)
                //        {
                //            contextMenuStrip.Show(dataGrid.PointToScreen(e.Location));
                //        }
                //    }
                //}

            }
        }

        [TestMethod]
        public void Checkboxes()
        {
            //Open the browser
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                //Visit the page
                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/checkboxes";

                //Find all of the checkboxes on the page
                IList<IWebElement> checkboxesInputs = webDriverHelper.GetDriver().FindElements(By.CssSelector("input[type=checkbox"));

                Assert.IsNotNull(checkboxesInputs);
                Assert.AreEqual(2, checkboxesInputs.Count);
                Assert.IsFalse(checkboxesInputs[0].Selected);

                bool isLastChecked = checkboxesInputs.Last().Selected;
                Assert.IsTrue(isLastChecked);
                //Assert.IsTrue(checkboxesInputs[1].Selected);   == dublicate, for example

                //Close the browser
            }
        }

        [TestMethod]
        public void ContextMenu() // only in FF
        {
            //http://elementalselenium.com/tips/63-right-click

            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                //1.Open the browser and visit the page
                IWebDriver driver = new FirefoxDriver();
                driver.Url = "http://the-internet.herokuapp.com/context_menu";

                //2.Find and right-click the area which will render a custom context menu
                IWebElement menuArea = driver.FindElement(By.Id("hot-spot"));

                //3.Select the context menu option with keyboard keys
                Actions actions = new Actions(webDriverHelper.GetDriver());
                actions.ContextClick(menuArea);

                IAlert alert1 = driver.SwitchTo().Alert();

                MouseButtons mouseButtons = new MouseButtons();

                ContextMenu contextMenu = new ContextMenu();

                //4.JavaScript alert appears
                IAlert alert = driver.SwitchTo().Alert();
                // alert.Text = "You selected a context menu";

                //5.Grab the text of the JavaScript alert
                //6.Assert that the text from the alert is what we expect

            }
        }   //??

        [TestMethod]
        public void DisappearingElements()   //&&&&&&&&&&&&&
        {
            //http://stackoverflow.com/questions/27639550/how-to-test-whether-an-element-is-displayed-on-the-page
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();

                webDriver.Url = "http://the-internet.herokuapp.com/disappearing_elements";

                ReadOnlyCollection<IWebElement> pageLinks = webDriver.FindElements(By.CssSelector("li"));

                foreach (IWebElement galleryLink in pageLinks)
                {
                    string hrefText = galleryLink.Text;
                    string galleryText = "Gallery";


                    if (galleryText.Contains(hrefText))
                    {

                        galleryLink.Click();

                        IWebElement h1Text = webDriver.FindElement(By.CssSelector("h1"));
                        string textActual = h1Text.Text;
                        string textExpected = "NotFound";
                        Assert.AreEqual(textExpected, textActual);
                    }

                    else
                    {
                        string elementNotAppears = "element NOT appears";
                    }

                }

            }
        }   //??

        [TestMethod]
        public void DragAndDrop()
        {
            //http://comments.gmane.org/gmane.comp.web.selenium.user/33020   
            //http://selenium.googlecode.com/svn/trunk/docs/api/rb/Selenium/WebDriver/ActionBuilder.html
            //http://elementalselenium.com/tips/39-drag-and-drop
            //https://gist.github.com/rcorreia/2362544

            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/drag_and_drop";

                //string filePath = @"C:\Users\Vitalina\documents\visual studio 2013\Projects\InternetHerokuapp\InternetHerokuapp\Scripts\drag_and_drop_helper.js";

                Actions actions = new Actions(webDriverHelper.GetDriver());

                IWebElement source = webDriverHelper.GetDriver().FindElement(By.Id("column-a"));
                IWebElement target = webDriverHelper.GetDriver().FindElement(By.Id("column-b"));
                actions.DragAndDrop(source, target);
                actions.Build();
                actions.Perform();

            }
        }     //??

        [TestMethod]
        public void DropdownList()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/dropdown";

                IWebElement dropdownId = webDriverHelper.GetDriver().FindElement(By.Id("dropdown"));
                SelectElement selectElement = new SelectElement(dropdownId);
                selectElement.SelectByText("Option 1");
                selectElement.SelectByText("Option 2");

            }
        }   //??

        [TestMethod]

        #region
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            // http://stackoverflow.com/questions/1718389/right-click-context-menu-for-datagridview
            //http://stackoverflow.com/questions/973721/c-sharp-detecting-if-the-shift-key-is-held-when-opening-a-context-menu

            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new System.Windows.Forms.MenuItem("Cut"));
                m.MenuItems.Add(new System.Windows.Forms.MenuItem("Copy"));
                m.MenuItems.Add(new System.Windows.Forms.MenuItem("Paste"));

                //int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                //if (currentMouseOverRow >= 0)
                //{
                //    m.MenuItems.Add(new System.Windows.Forms.MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                //}

                //m.Show(dataGridView1, new Point(e.X, e.Y));

            }
        }

        public bool IsDisplayed()
        {
            return true;
        }
        #endregion


        //https://stackoverflow.com/questions/31532534/identifying-number-of-iframes-in-a-page-using-selenium


    }
}
