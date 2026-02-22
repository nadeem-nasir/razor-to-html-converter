# Title
- Azure Functions and Razor templates to convert Razor views to html strings in .net 8 isolated function outside of ASP.NET Core and with a strongly typed model support.
# Introduction 
- A Serverless,modern, low cost, and event driven solution that uses Azure Functions and .NET 8 to generate HTML from Razor views with model support, without using any external NuGet packages.
- Support Layouts and Partials views and strongly typed model.
- No matter what email you need to send, this solution can help you. It can convert Razor views with model to html outside of a Web application for email confirmations, account verifications, order receipts, status updates, or scheduler notifications etc.
# Preqrequisites
- Azure subscription
- Visual Studio 2022 or later 
- .NET 8.0
- Azure Functions Core Tools v4.x
- Azure development workload in Visual Studio
- Azure storage account
- Set copy to output directory to copy always for all the views 
- Views are not embedded resources in to assembly instsead they are copied to output directory and loaded from there
# Azure resources used in the project
- Azure Function App in .NET 8.0, isolated worker process  Linux OS, 
- Azure hosting plan
- Azure Confugurations in Bicep template
- Azure Storage
# To run the application locally
- Clone the repository
- Open the solution in Visual Studio 2022 or later
- Rename template.local.settings.json to local.settings.json
- Build the solutio and wait to restore packages
- Copy the PingFunctions url and paste it in the browser and it should return products in html 
- Bicep template deploy all the required resources and also set the required configurations. so it is easy to deploy the infrastructure and test the application
# Deploying Azure Function Apps using Azure DevOps
- Azure subscription
- Deploy infrastructure using Azure Bicep temnplate from infra folder
- Create a new project in Azure DevOps or GitHub
- Create a new service connection in Azure DevOps or GitHub
- Create variables group in Azure DevOps or GitHub
- Create a new pipeline in Azure DevOps or GitHub using the pipelines.yaml file from deploy folder
- Push the code to the repository
# Learning Outcomes of the Case Study 
- Convert Razor views to html strings in .NET 8 isolated functions with a strongly typed model support, without relying on any third party NuGet packages or any third party services.
- How to write Extension 
- How to add AddJsonFile in program.cs
- How to use Globall using
- Demonstrate the use of ProductsSale.cshtml, a Razor view that displays the sales data of different products in a table format.
- Razor views are particularly well-suited for defining the layout of an HTML-based e-mail or notification message.
- Can be used to attached generated html to email body.
- Can be used with HtmlToPdfFunction to generate pdf from html.
- Event driven way to send emails or notifications.
- Can be used with blob trigger, service bus trigger, queue storage trigger etc, to generate html.
- Convenient for scenarios where you want to generate HTML fragments, such as for generating email content, generating static site content, or for building a content templating engine
- Use the Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore package in the isolated worker model
- Can use layouts and partial views in your project and add them with this code.
```
@{
	Layout = "_Layout.cshtml";
 }
```
```
@{
   await Html.RenderPartialAsync("specify the name of the partial view");
 }
```
- How to use Bogus to generate fake data.
# Extend the Case Study: Todo For Learning 
- Deploy the infrastructure using Azure Bicep template
- Change AzureWebJobsStorage and APPINSIGHTS_INSTRUMENTATIONKEY to your own values in local.settings.json
- Run the application locally and test the function
- Run PingProductsSale url in browser and it should return response in html
- Create new html trigger function and that should return Users list. 