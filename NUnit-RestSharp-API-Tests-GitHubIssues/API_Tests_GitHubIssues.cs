using System;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using RestSharp.Authenticators;

namespace NUnit_RestSharp_API_Tests_GitHubIssues
{
    public class API_Tests_GitHubIssues
    {
        private RestClient client;
        private RestRequest request;
       
        const string GitHubUsername = "put_your_name_here";
        const string GitHubToken = "put_your_api_token_key_here";

        const string RepoName = "put_your_repository_name_here";


        [SetUp]
        public void Setup()
        {
            this.client = new RestClient("https://api.github.com");
            //client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

        }

        [Test]
        public async Task GitHubAPI_GetIssuesbyRepo()
        {
            this.request = new RestRequest("/repos/{user}/{repo}/issues");
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);


            var response = await client.ExecuteAsync(request);
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);
            foreach (var issue in issues)
            {
                Assert.IsNotNull(issue.number);
                Assert.IsNotNull(issue.id, "Issue id must not be null");
            }


        }

        [Test]
        public async Task GitHubAPI_GetIssueByValidNumber()
        {
            this.request = new RestRequest("/repos/{user}/{repo}/issues/12");
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);

            var response = await client.GetAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.Greater(issue.id, 0, "Id is not greater than 0");
            Assert.Greater(issue.number, 0, "Number is not greater than 0");
        }

        [Test]
        public async Task GitHubAPI_GetIssueByInValidNumber()
        {
            this.request = new RestRequest("/repos/{user}/{repo}/issues/{int.MaxValue}");
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);

            var response = await client.GetAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task GitHubAPI_CreateIssueByValidAuthentication()
        {

            client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);
            var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Post);
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);


            request.AddHeader("Content-Type", "application/json");

            string title = "Issue from RestSharp";

            request.AddJsonBody(new { title, body = "Body", labels = new[] { "bug", "question" } });

            var response = await client.ExecuteAsync(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.Greater(issue.id, 0, "Id is  greater than 0");
            Assert.Greater(issue.number, 0, "Number is greater than 0");
            Assert.That(issue.title, Is.EqualTo(title));
        }


        [Test]
        public void GitHubAPI_CreateNewIssue_Unauthorized()
        {

            var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Post);
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = "some title",
                body = "some body",
                labels = new string[] { "bug", "importance:high", "type:UI" }
            });
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GitHubAPI_CreateNewIssue_Authorized()
        {

            client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);


            var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Post);
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                title = "some title",
                body = "some body",
                labels = new string[] { "bug", "importance:high", "type:UI" }
            });
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.IsTrue(response.ContentType.StartsWith("application/json"));

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.IsTrue(issue.id > 0);
            Assert.IsTrue(issue.number > 0);
            Assert.IsTrue(!String.IsNullOrEmpty(issue.title));
            Assert.IsTrue(!String.IsNullOrEmpty(issue.body));

        }
        [Test]
        public async Task GitHub_CreateIssueNoTitle()
        {
            client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);


            var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Post);
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);


            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { title = "", body = "Body" });
            var response = await client.ExecuteAsync(request, Method.Post);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity), "Status code is not 422");

        }


        [Test]
        public void GitHubAPI_CreateNewComment()
        {
            // Create a new comment for issue #27

            client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

            var request = new RestRequest("/repos/{user}/{repo}/issues/27/comments", Method.Post);
            request.AddUrlSegment("user", GitHubUsername);
            request.AddUrlSegment("repo", RepoName);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new
            {
                body = "This is next comment from RestSharp"
            });

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Content, Is.Not.Null);



            var newComment = JsonSerializer.Deserialize<CommentResponse>(response.Content);
            Assert.IsNotNull(newComment.id);
            Assert.IsNotEmpty(newComment.body);
        }

        //[Test]

        //public void GitHubAPI_DeleteComment()
        //{
        //    Delete comment Id 1157978171
        //    clientDel = new RestClient("https://api.github.com/repos/miglenamag/PostmanDemoForGitHubIssuesAPI/issues/1157978171");

        //    clientDel.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

        //    var delRequest = new RestRequest("/repos /{user} /{repo}/issues /comments/1157978171", Method.Delete);
        //    delRequest.AddUrlSegment("user", GitHubUsername);
        //    delRequest.AddUrlSegment("repo", RepoName);


        //    var delResponse = clientDel.Execute(delRequest);
        //    Assert.That(delResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        //}


        internal class CommentResponse
        {
            public long id { get; set; }
            public string body { get; set; }
        }
    }
}
