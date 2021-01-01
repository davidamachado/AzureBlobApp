# AzureBobApp
A Template for Uploading and Viewing Images Using Azure Blob Storage 

### Version
1.0

![AzureBlobAppImage](https://github.com/davidamachado/AzureBlobApp/blob/master/TestWebAzureApp/wwwroot/images/AzureBlobApp.JPG?raw=true)

### Description
This is a web application which was created to provide a template for uploading images to an Azure Storage account and viewing them in the browser. 

### Features
* Ability to upload image files from the user's personal drive

* Provides a list of current images found within the container and a preview of each image

* Creates the necessary container with privacy level set to "Blob" if one does not exist

### Setup
Clone this repository and open project in Visual Studio IDE or any other IDE that supports ASP.NET Core MVC Web App development. Copy the connection string associated with your Azure Blob Storage account access key and paste it into the appsettings.json file. It should be placed between the quotation marks located after the colon within the same row as the AzureConnection variable as seen below.

![AzureBlobAppImage](https://github.com/davidamachado/AzureBlobApp/blob/master/TestWebAzureApp/wwwroot/images/appsettings_json.JPG?raw=true)
