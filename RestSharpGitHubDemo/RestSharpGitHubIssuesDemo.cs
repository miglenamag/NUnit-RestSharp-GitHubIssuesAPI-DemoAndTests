using System;
using System.Collections.Generic;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;

var client = new RestClient("https://api.github.com/");
const string GitHubUsername = "put_your_name_here";
const string RepoName = "put_your_repository_name_here";
const string GitHubToken = "put_your_api_token_key_here";

//  "put_your_api_token_key_here";


//var request = new RestRequest("/repos/{user}/{repo}/issues");

//request.AddUrlSegment("user", GitHubUsername);
//request.AddUrlSegment("repo", RepoName);

//var response = await client.ExecuteAsync(request);

//// 1. example of Get request
///First - uncomment lines 17-22 and the next lines, too

//Console.WriteLine("Status Code: " + response.StatusCode);
//Console.WriteLine("Body: " + response.Content);


////2. example of GET request with deserializing
//First - uncomment lines 17-22 and the next lines, too

//Console.WriteLine("Status Code: " + response.StatusCode);

//var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

////other ways for deserializing:
////var issues = new SystemTextJsonSerializer().Deserialize<List<Issue>>(response.Content);
////var issues = JSonConvert.DeserializeObject<List<Issue>>(response.Content);

//Console.WriteLine("Total Number of issues " + issues.Count);
//Console.WriteLine("***************************************");

//foreach (var issue in issues)
//{
//    Console.WriteLine("Issue Name: " + issue.title);
//    Console.WriteLine("Issue Body Text: " + issue.body);
//    Console.WriteLine("Issue Number:: " + issue.number);
//    Console.WriteLine("Issue Id: " + issue.id);
//    Console.WriteLine("******");
//}


// 3.example of POST request for creating new issue

client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

// Before execution of this example - comment lines 17-22

var request = new RestRequest("/repos/{user}/{repo}/issues", Method.Post);

request.AddUrlSegment("user", GitHubUsername);
request.AddUrlSegment("repo", RepoName);

//request.AddBody(new { title = " Another New Issue from RestSharp" });
request.AddJsonBody(new { title = " Another New Issue from RestSharp", body = "This issue  is  created from RestSharpAPI." });


var response = await client.ExecuteAsync(request);

//var issue = new JSonConvert.DeserializeObject<Issue>(response);
//Console.WriteLine("Title of the created issue: " + issue);

Console.WriteLine("Status Code: " + response.StatusCode);
Console.WriteLine("******");
//Console.WriteLine("Body: " + response.Content);





// 4.example of POST request for creating new comment in issue#12

//client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

//// Before execution of this example - comment lines 10 to 15

//var request = new RestRequest("/repos/{user}/{repo}/issues/12/comments", Method.Post);

//request.AddUrlSegment("user", GitHubUsername);
//request.AddUrlSegment("repo", RepoName);

////request.AddBody(new { title = " New Issue from RestSharp" });
//request.AddJsonBody(new { body = "This comment  is added from RestSharp." });


//var response = await client.ExecuteAsync(request);
//Console.WriteLine("Status Code: " + response.StatusCode);
//Console.WriteLine("******");

//Console.WriteLine("Body: " + response.Content);

// 5.example of PATCH request for editing issue number 11

//client.Authenticator = new HttpBasicAuthenticator(GitHubUsername, GitHubToken);

//// Before execution of this example - comment lines 10 to 15

//var request = new RestRequest("/repos/{user}/{repo}/issues/11", Method.Post);

//request.AddUrlSegment("user", GitHubUsername);
//request.AddUrlSegment("repo", RepoName);

////request.AddBody(new { title = " New Issue from RestSharp" });
//request.AddBody(new { title = "Edited title of Issue from RestSharp." });


//var response = await client.ExecuteAsync(request);
//Console.WriteLine("Status Code: " + response.StatusCode);
//Console.WriteLine("******");

//Console.WriteLine("Body: " + response.Content);






