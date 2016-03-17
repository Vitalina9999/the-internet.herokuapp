using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
using Keys = System.Windows.Forms.Keys;

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
        }  //??

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
            //http://stackoverflow.com/questions/11428026/select-an-option-from-the-right-click-menu-in-selenium-webdriver-java

            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                //1.Open the browser and visit the page
                IWebDriver driver = new FirefoxDriver();
                driver.Url = "http://the-internet.herokuapp.com/context_menu";

                //2.Find and right-click the area which will render a custom context menu
                IWebElement menu = driver.FindElement(By.Id("hot-spot"));

                //3.Select the context menu option with keyboard/mouse keys
                Actions actions = new Actions(driver);
                actions.ContextClick(menu);
                actions.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                actions.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                actions.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                actions.SendKeys(OpenQA.Selenium.Keys.Enter);
                actions.SendKeys(OpenQA.Selenium.Keys.Enter);
                // actions.Build();
                actions.Perform();

                // Unexpected modal dialog (text: You selected a context menu) The alert disappeared before it could be closed.


                //4.JavaScript alert appears
                IAlert alert = driver.SwitchTo().Alert();

                //5.Grab the text of the JavaScript alert
                string alertTextExpected = alert.Text;
                string alertTextActual = "You selected a context menu";

                //6.Assert that the text from the alert is what we expect
                Assert.AreEqual(alertTextExpected, alertTextActual);

            }
        }   //??

        [TestMethod]
        public void DisappearingElements()
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
                        string textExpected = "Not Found";
                        Assert.AreEqual(textExpected, textActual);
                    }

                    else
                    {
                        string elementNotAppears = "element NOT appears";
                    }

                }

            }
        }

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

                Actions actions = new Actions(webDriverHelper.GetDriver());

                IWebElement source = webDriverHelper.GetDriver().FindElement(By.Id("column-a"));
                IWebElement target = webDriverHelper.GetDriver().FindElement(By.Id("column-b"));

                actions.DragAndDrop(source, target);
                actions.Perform();

                actions.Build();
                actions.Perform();

            }
        }     //??

        [TestMethod]
        public void DropdownList()     // Assert ?
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/dropdown";

                IWebElement dropdownId = webDriverHelper.GetDriver().FindElement(By.Id("dropdown"));
                SelectElement selectElement = new SelectElement(dropdownId);

                selectElement.SelectByText("Option 1");
                bool isOption1Displayed = selectElement.SelectedOption.Displayed;
                Assert.IsTrue(isOption1Displayed, "Option 1");

                selectElement.SelectByText("Option 2");
                bool isOption2Displayed = selectElement.SelectedOption.Displayed;
                Assert.IsTrue(isOption2Displayed, "Option 2");
            }
        }

        [TestMethod]   //??
        public void DynamicContent()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                webDriverHelper.GetDriver().Url = "http://the-internet.herokuapp.com/dynamic_content";

            }
        }  //??

        [TestMethod]
        public void DynamicControls()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/dynamic_controls";

                IWebElement checkboxId = webDriver.FindElement(By.TagName("input"));
                checkboxId.Click();

                IWebElement btnRemove = webDriver.FindElement(By.Id("btn"));
                btnRemove.Click();

                WebDriverWait webDriverWait0 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(4));
                webDriverWait0.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("message")));

                IWebElement messageItsGone = webDriver.FindElement(By.Id("message"));
                string messageTextActual1 = messageItsGone.Text;
                string messageTextExpected1 = "It's gone!";
                Assert.AreEqual(messageTextExpected1, messageTextActual1);

                IWebElement btnAdd = webDriver.FindElement(By.Id("btn"));
                btnAdd.Click();

                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(4));
                webDriverWait1.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("message")));


                IWebElement messageItsBack = webDriver.FindElement(By.Id("message"));
                string messageTextActual2 = messageItsBack.Text;
                string messageTextExpected2 = "It's back!";
                Assert.AreEqual(messageTextExpected2, messageTextActual2);
            }
        }

        [TestMethod]
        public void DynamicLoadingElementOnPageThatIsHidden()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/dynamic_loading/1";

                IWebElement btnStart = webDriver.FindElement(By.TagName("button"));
                btnStart.Click();

                WebDriverWait webDriverWait0 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                webDriverWait0.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("finish")));

                IWebElement messageId = webDriver.FindElement(By.Id("finish"));
                IWebElement messageHelloWorld = messageId.FindElement(By.TagName("h4"));

                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
                webDriverWait1.Until(ExpectedConditions.TextToBePresentInElement(messageHelloWorld, "Hello World!"));

                string messageTextActual = messageHelloWorld.Text;
                string messageTextExpected = "Hello World!";


                Assert.AreEqual(messageTextExpected, messageTextActual);

            }
        }

        [TestMethod]
        public void DynamicLoadingElementOnPageThatIsRenderedAfterTheFact()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/dynamic_loading/2";

                IWebElement btnStart = webDriver.FindElement(By.TagName("button"));
                btnStart.Click();

                WebDriverWait webDriverWait0 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                webDriverWait0.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("finish")));

                IWebElement messageId = webDriver.FindElement(By.Id("finish"));

                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
                webDriverWait1.Until(ExpectedConditions.TextToBePresentInElement(messageId, "Hello World!"));

                string messageTextActual = messageId.Text;
                string messageTextExpected = "Hello World!";


                Assert.AreEqual(messageTextExpected, messageTextActual);

            }
        }

        [TestMethod]
        public void ExitIntend()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/exit_intent";

                Actions actions = new Actions(webDriver);
                actions.MoveByOffset(5000, 0).Perform();

                IWebElement modalBody = webDriver.FindElement(By.ClassName("modal-body"));

                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));

                webDriverWait1.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("modal-body")));

                bool isModalBodyDisplayed = modalBody.Displayed;

                Assert.IsTrue(isModalBodyDisplayed);
            }
        }

        [TestMethod]
        public void FileDownload() //??
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                // Open the browser
                IWebDriver webDriver = webDriverHelper.GetDriver();

                // Load the page
                webDriver.Url = "http://the-internet.herokuapp.com/download";

                // Grab the URL of the first download link
                IWebElement example = webDriver.FindElement(By.ClassName("example"));

                IList<IWebElement> aList = example.FindElements(By.TagName("a"));

                foreach (IWebElement href in aList)
                {
                    string hrefLink = href.GetAttribute("href");
                    href.Click();

                    //FileInfo file = new FileInfo("hrefLink");

                    //if (file.Length > 0)
                    //{

                    //}
                    //else
                    //{
                    //    Assert.Fail();
                    //}

                    Assert.IsNotNull(hrefLink);
                    // Assert.IsNotNull(file.Length);


                }
            }
        }

        [TestMethod]
        public void FileUpload()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                // Open the browser
                IWebDriver webDriver = webDriverHelper.GetDriver();

                // Load the page
                webDriver.Url = "http://the-internet.herokuapp.com/upload";

                // Grab the URL of the first download link
                IWebElement uploadFile = webDriver.FindElement(By.Id("file-upload"));
                uploadFile.Click();

                uploadFile.SendKeys(@"C:\Files\Projects\InternetHerokuapp_git\fileupload.png");

                IWebElement btnSubmit = webDriver.FindElement(By.Id("file-submit"));
                btnSubmit.Click();

                IWebElement h3 = webDriver.FindElement(By.TagName("h3"));
                string h3TextActual = h3.Text;
                string h3TextExpected = "File Uploaded!";

                IWebElement uploadedFile = webDriver.FindElement(By.Id("uploaded-files"));
                string uploadedFileText = uploadedFile.Text;

                Assert.AreEqual(h3TextExpected, h3TextActual);
                Assert.IsNotNull(uploadedFileText);
            }
        }

        [TestMethod]
        public void FloatingMenu()   //??
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {

                IWebDriver webDriver = webDriverHelper.GetDriver();

                webDriver.Url = "http://the-internet.herokuapp.com/floating_menu";


            }
        }

        [TestMethod]
        public void ForgotPassword()  //??
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            //https://github.com/bugfree-software/the-internet-solution-python/blob/master/tests/test_forgot_password.py
            //http://elementalselenium.com/tips/43-forgot-password
            {
                //Open the browser
                IWebDriver webDriver = webDriverHelper.GetDriver();

                //Visit the page and initiate the forgot password workflow
                webDriver.Url = "http://the-internet.herokuapp.com/forgot_password";

                IWebElement emailTbx = webDriver.FindElement(By.Id("email"));
                emailTbx.SendKeys("");   // add your email :)

                IWebElement submitBtn = webDriver.FindElement(By.TagName("button"));
                submitBtn.Click();

                IWebElement validation = webDriver.FindElement(By.Id("content"));
                string validationText = validation.Text;
                string validationTextExpected = "Your e-mail's been sent!";
                Assert.AreEqual(validationTextExpected, validationText);

                //Headlessly access Gmail and retrieve the email message body

                webDriver.Url = "https://mail.google.com/mail/u/0/#inbox";

                IWebElement emailGmail = webDriver.FindElement(By.Id("Email"));
                emailGmail.SendKeys("");   // add your email :)

                IWebElement nextBtnGmail = webDriver.FindElement(By.Id("next"));
                nextBtnGmail.Click();

                WebDriverWait webDriverWait0 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                webDriverWait0.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("Passwd")));

                IWebElement passwordGmail = webDriver.FindElement(By.Id("Passwd"));
                passwordGmail.SendKeys("");     // add your pass :)

                IWebElement signIn = webDriver.FindElement(By.Id("signIn"));
                signIn.Click();

                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                webDriverWait1.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id(":2w")));

                IWebElement emailNoReply = webDriver.FindElement(By.Name("no-reply"));
                string noreplyActual = emailNoReply.GetAttribute("name");

                // string noreplyActual = emailNoReply.Text;
                string noreplyExpected = "no-reply";
                Assert.AreEqual(noreplyExpected, noreplyActual);

                emailNoReply.Click();

                IWebElement emailFirstId = webDriver.FindElement(By.Id(":86"));
                //url =  message_body.scan(/https?:\/\/[\S]+/).last
                //  username = message_body.scan(/username: (.*)$/)[0][0].strip
                //  password = message_body.scan(/password: (.*)$/)[0][0].strip

                //email_address = S("#email").web_element.text

            }
        }

        [TestMethod]
        public void LoginPage()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                //login success
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/login";

                IWebElement usernameTbx = webDriver.FindElement(By.Id("username"));
                usernameTbx.SendKeys("tomsmith");

                IWebElement passwordTbx = webDriver.FindElement(By.Id("password"));
                passwordTbx.SendKeys("SuperSecretPassword!");

                IWebElement loginBtn = webDriver.FindElement(By.TagName("button"));
                loginBtn.Click();

                IWebElement msgSuccess = webDriver.FindElement(By.Id("flash"));
                bool isMsgSuccessAppeared = msgSuccess.Displayed;
                Assert.IsTrue(isMsgSuccessAppeared);

                IWebElement logoutBtn = webDriver.FindElement(By.LinkText("Logout"));

                logoutBtn.Click();

                //login unsuccess
                WebDriverWait webDriverWait1 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                webDriverWait1.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("example")));

                IWebElement loginBtn1 = webDriver.FindElement(By.TagName("button"));
                loginBtn1.Click();

                webDriverWait1.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("flash")));

                IWebElement msgError = webDriver.FindElement(By.Id("flash"));
                bool isMsgErrorAppeared = msgError.Displayed;
                Assert.IsTrue(isMsgErrorAppeared);

                IWebElement usernameTbx1 = webDriver.FindElement(By.Id("username"));
                usernameTbx1.SendKeys("tomsmith");

                IWebElement passwordTbx1 = webDriver.FindElement(By.Id("password"));
                passwordTbx1.SendKeys("IncorrectPassword!");
                Assert.IsTrue(isMsgErrorAppeared);

                IWebElement loginBtn2 = webDriver.FindElement(By.TagName("button"));
                loginBtn2.Click();

                IWebElement msgError2 = webDriver.FindElement(By.Id("flash"));
                bool isMsgErrorAppeared2 = msgError2.Displayed;
                Assert.IsTrue(isMsgErrorAppeared);

            }
        }

        [TestMethod]
        public void NestedFrames()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/nested_frames";

                webDriver.SwitchTo().Frame("frame-top");

                webDriver.SwitchTo().Frame("frame-middle");

                IWebElement contentId = webDriver.FindElement(By.Id("content"));
                string contentTextActual = contentId.Text;
                string contentTextExpected = "MIDDLE";
                Assert.AreEqual(contentTextExpected, contentTextActual);

            }
        }

        [TestMethod]
        public void iFrames()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/iframe";

                //grab the original text and store it
                webDriver.SwitchTo().Frame("mce_0_ifr");

                IWebElement editor = webDriver.FindElement(By.Id("tinymce"));

                string beforeEditorText = editor.Text;

                //clear and input new text
                editor.Clear();

                editor.SendKeys("Hello World!");

                //grab the new text value
                string afterEditorText = editor.Text;

                //assert that the original and new texts are not the same
                Assert.AreNotSame(afterEditorText, beforeEditorText);

                webDriver.SwitchTo().DefaultContent();
                Assert.IsNotNull(webDriver.FindElement(By.CssSelector("h3")));
            }
        }

        [TestMethod]
        public void Geolocation()  //??
        {
            //http://www.softwaretestinghelp.com/handle-alerts-popups-selenium-webdriver-selenium-tutorial-16/
            //http://geolocate-me.com/faq
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/geolocation";

                IWebElement geoBtn = webDriver.FindElement(By.TagName("button"));
                geoBtn.Click();

                //// Crash 

                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                js.ExecuteScript("");

                IWebElement geoAtt = webDriver.FindElement(By.Id("lat-value"));
                IWebElement geoLat = webDriver.FindElement(By.Id("long-value"));

                IWebElement googleLink = webDriver.FindElement(By.Id("map-link"));
                string googleLinkActual = googleLink.Text;
                string googleLinkExpected = "See it on Google";


                Assert.IsTrue(googleLink.Displayed);
                Assert.IsTrue(geoBtn.Displayed);
                Assert.AreEqual(googleLinkExpected, googleLinkActual);
            }
        }

        [TestMethod]
        public void Hovers()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/hovers";

                List<IWebElement> detailsSummaries = webDriver.FindElements(By.ClassName("figcaption")).ToList();

                List<IWebElement> userPhotos = webDriver.FindElements(By.ClassName("figure")).ToList();

                Actions actions = new Actions(webDriver);

                int i = 0;

                foreach (IWebElement userPhoto in userPhotos)
                {
                    i++;

                    actions.MoveToElement(userPhoto).Perform();
                    IWebElement element = detailsSummaries.FirstOrDefault(x => x.Displayed);

                    Assert.IsNotNull(element, "Element cannot be null");

                    IWebElement userName = element.FindElements(By.TagName("h5")).ToList().FirstOrDefault();
                    Assert.IsNotNull(userName, "userName cannot be null");
                    Assert.AreEqual("name: user" + i, userName.Text, "OOOH NO, strings are not mutch!!! uuuuuu");

                    IWebElement viewProfileLink = element.FindElements(By.TagName("a")).ToList().FirstOrDefault();
                    Assert.IsNotNull(viewProfileLink, "viewProfileLink cannot be null");
                    Assert.AreEqual("View profile", viewProfileLink.Text, "OOOH NO, strings are not mutch!!! uuuuuu");
                    Assert.IsTrue(viewProfileLink.GetAttribute("href").Contains("/users/" + i), "OH NOOOOO!! link is WROOOOOOONG!");


                }

            }
        }

        [TestMethod]
        public void InfiniteScroll() //??
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/infinite_scroll";

                IList<IWebElement> infinityScrollList = webDriver.FindElements(By.TagName("a"));

                Actions actions = new Actions(webDriver);


                foreach (IWebElement infinityScroll in infinityScrollList)
                {
                    string href = infinityScroll.GetAttribute("href");


                }

            }
        }

        [TestMethod]
        public void JQueryUIMenu()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/jqueryui/menu";

                IWebElement enableMenu = webDriver.FindElement(By.Id("ui-id-2"));
                enableMenu.Click();

                Actions actions = new Actions(webDriver);
                actions.MoveToElement(enableMenu).Perform();

                IWebElement downloadMenu = webDriver.FindElement(By.Id("ui-id-4"));
                downloadMenu.Click();
                // WebDriverWait webDriverWait0 = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
                // webDriverWait0.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("ui-id-4")));

                actions.MoveToElement(downloadMenu).Perform();

                IWebElement pdfMenu = webDriver.FindElement(By.Id("ui-id-6"));

                ReadOnlyCollection<IWebElement> pageLinks = webDriver.FindElements(By.TagName("a"));

                foreach (IWebElement pdfLink in pageLinks)
                {
                    string hrefLink = pdfLink.GetAttribute("href");
                    string linkPdf = "/download/jqueryui/menu/menu.pdf";


                    if (hrefLink.Contains(linkPdf))
                    {
                        // click on link

                    }

                    else
                    {
                        string elementNotAppears = "element NOT appears";
                    }
                }

            }
        } //??

        [TestMethod]
        public void Javascript_alerts()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/javascript_alerts";

                Actions actions = new Actions(webDriver);
                IWebElement button1 = webDriver.FindElement(By.XPath(("/html/body/div[2]/div/div/ul/li[1]/button")));
                IWebElement button2 = webDriver.FindElement(By.XPath(("/html/body/div[2]/div/div/ul/li[2]/button")));
                IWebElement button3 = webDriver.FindElement(By.XPath(("/html/body/div[2]/div/div/ul/li[3]/button")));

                button1.Click();
                IAlert iAlert = webDriver.SwitchTo().Alert();
                iAlert.Accept();
                IWebElement result = webDriver.FindElement(By.Id("result"));
                string resultTextActual1 = result.Text;
                string resultTextExpected1 = "You successfuly clicked an alert";
                Assert.AreEqual(resultTextExpected1, resultTextActual1);


                button2.Click();
                iAlert.Accept();
                //  IWebElement resultAccept2 = webDriver.FindElement(By.Id("result"));
                string resultAcceptTextActual2 = result.Text;
                string resultAcceptTextExpected2 = "You clicked: Ok";
                Assert.AreEqual(resultAcceptTextExpected2, resultAcceptTextActual2);

                button2.Click();

                iAlert.Dismiss();
                //IWebElement result2 = webDriver.FindElement(By.Id("result"));
                string resultDismissTextActual21 = result.Text;
                string resultDismissTextExpected21 = "You clicked: Cancel";
                Assert.AreEqual(resultDismissTextExpected21, resultDismissTextActual21);

                button3.Click();
                iAlert.SendKeys("Hello");
                iAlert.Accept();

                // IWebElement result = webDriver.FindElement(By.Id("result"));
                string resultTextExpected31 = "You entered: Hello";
                Assert.IsTrue(result.Displayed);

                button3.Click();
                iAlert.Dismiss();
                string resultDismissTextExpected31 = "You entered: null";
                string resultDismissTextActual31 = result.Text;
                Assert.AreEqual(resultDismissTextExpected31, resultDismissTextActual31);

                // Variant 2

                List<IWebElement> buttonList = webDriver.FindElements(By.TagName("button")).ToList();
                int i = 0;
                foreach (IWebElement button in buttonList)
                {
                    i++;
                    button.Click();
                    IAlert iAlert2 = webDriver.SwitchTo().Alert();
                    iAlert2.Accept();
                    IWebElement result2 = webDriver.FindElement(By.Id("result"));
                    Assert.IsTrue(result2.Displayed);
                }

                //List<string> alertMessages = new List<string>();
                //alertMessages.Add("I am a JS Alert");
                //alertMessages.Add("I am a JS Confirm");
                //alertMessages.Add("I am a JS prompt");

                //    int i = 0;
                //    foreach (IWebElement button in buttonList)
                //    {
                //           i++;
                //       button.Click();
                //       IAlert iAlert = webDriver.SwitchTo().Alert();
                //    bool isExistAlertText = alertMessages.Any(x => x == iAlert.Text);
                //    iAlert.Accept();
                //    Assert.IsTrue(isExistAlertText, "OOPS, it should be message");

                // if (iAlert.Text == "I am a JS Alert") //iAlert.Text == "I am a JS Alert"  button.Text == "I am a JS Alert"
                //  // {

                //       iAlert.Accept();
                //       IWebElement result = webDriver.FindElement(By.Id("result"));
                //       string resultTextActual = result.Text;
                //       string resultTextExpected = "You successfuly clicked an alert";
                //       Assert.AreEqual(resultTextExpected, resultTextActual);
                // //  }

                // //  if (button.Text == "I am a JS Confirm")//    if (iAlert.Text == "I am a JS Confirm")
                // //  {
                //       button.Click();
                //       iAlert.Accept();

                //       IWebElement resultAccept = webDriver.FindElement(By.Id("result"));
                //       string resultAcceptTextActual = resultAccept.Text;
                //       string resultAcceptTextExpected = "You clicked: Ok";
                //       Assert.AreEqual(resultAcceptTextExpected, resultAcceptTextActual);

                //       button.Click();

                //       iAlert.Dismiss();
                //       IWebElement result = webDriver.FindElement(By.Id("result"));
                //       string resultDismissTextActual = result.Text;
                //       string resultDismissTextExpected = "You clicked: Cancel";
                //       Assert.AreEqual(resultDismissTextExpected, resultDismissTextActual);

                // //  }

                ////   if (button.Text == "I am a JS prompt")
                ////   {
                //       iAlert.SendKeys("Hello");
                //       // need find decision how to check entered parameter 
                //       //alertEnteredText
                //       //
                //       iAlert.Accept();

                //       IWebElement result = webDriver.FindElement(By.Id("result"));
                //       string resultTextExpected = "You entered:";
                //       //Assert.IsTrue(resultTextExpected + alertEnteredText);

                //       button.Click();
                //       iAlert.Dismiss();
                //       string resultDismissTextExpected = "You entered: null";
                //       string resultDismissTextActual = result.Text;
                //       Assert.AreEqual(resultDismissTextExpected, resultDismissTextActual);
                // //  }

                // }
            }

        }

        [TestMethod]
        public void Javascript_error()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/javascript_error";

                IWebElement pTag = webDriver.FindElement(By.TagName("p"));
                string jsErrorText = pTag.Text;

                Assert.IsTrue(jsErrorText.Contains("This page has a JavaScript error in the onload event"));


            }
        }

        [TestMethod]
        public void KeyPresses()
        {
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/key_presses";

                IWebElement example = webDriver.FindElement(By.ClassName("example"));

                Actions actions = new Actions(webDriver);
                actions.SendKeys(OpenQA.Selenium.Keys.Enter);
                actions.Perform();

                IWebElement result = webDriver.FindElement(By.Id("result"));

                string resultText = result.Text;
                Assert.AreEqual(resultText, "You entered: ENTER");

            }
        }

        [TestMethod]
        public void LargeAndDeepDom()
        {
            //http://elementalselenium.com/tips/verifying-locators
            //http://elementalselenium.com/tips/65-highlight-elements
            //http://www.ufthelp.com/2014/11/how-to-implement-highlight-in-selenium.html
            using (WebDriverHelper webDriverHelper = new WebDriverHelper())
            {
                IWebDriver webDriver = webDriverHelper.GetDriver();
                webDriver.Url = "http://the-internet.herokuapp.com/large";

                //Example 1
                IWebElement element23 = webDriver.FindElement(By.Id("sibling-2.3"));
                IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
                js.ExecuteScript("arguments[0].style.border='2px groove green'", element23);

                //Example 2 with method fnHighlightMe
                fnHighlightMe(webDriver, element23);
             }
         }

        #region
        public bool IsDisplayed()
        {
            return true;
        }

        public static void fnHighlightMe(IWebDriver driver, IWebElement element)
        {
            //Creating JavaScriptExecuter Interface
            IJavaScriptExecutor js = (IJavaScriptExecutor) driver;
            for (int iCnt = 0; iCnt < 3; iCnt++)
            {
                //Execute javascript
                js.ExecuteScript("arguments[0].style.border='4px groove red'", element);
                Thread.Sleep(1000);
                js.ExecuteScript("arguments[0].style.border=''", element);
            }
        }

        #endregion


        //https://stackoverflow.com/questions/31532534/identifying-number-of-iframes-in-a-page-using-selenium

    }
}
