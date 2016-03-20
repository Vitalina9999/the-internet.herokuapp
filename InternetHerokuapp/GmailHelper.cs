using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Google.Apis.Gmail.v1;

namespace InternetHerokuapp
{
    public class GmailHelper
    {
        public void Login(string login, string password)
        {
            //// If modifying these scopes, delete your previously saved credentials
            //// at ~/.credentials/gmail-dotnet-quickstart.json
            //string[] Scopes = { GmailService.Scope.GmailReadonly };
            //string ApplicationName = "Gmail API .NET Quickstart";

            ////UserCredential credential;

            //using (var stream =
            //    new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            //{
            //    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //    credPath = Path.Combine(credPath, "client_secret.json");

            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //    //Console.WriteLine("Credential file saved to: " + credPath);
            //}

            //// Create Gmail API service.
            //var service = new GmailService(new Google.Apis.Services.BaseClientService+Initializer())

            //// Define parameters of request.
            //UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

            //// List labels.
            //IList<Label> labels = request.Execute().Labels;
            //Console.WriteLine("Labels:");
            //if (labels != null && labels.Count > 0)
            //{
            //    foreach (var labelItem in labels)
            //    {
            //        //Console.WriteLine("{0}", labelItem.Name);
            //    }
            //}
            //else
            //{
            //    //Console.WriteLine("No labels found.");
            //}
            ////Console.Read();


        }
    }
}
