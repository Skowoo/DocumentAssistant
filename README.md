# DocumentAssistant - simple WPF CRUD app
Application created as semester project. Designed and created to simplify workflow of small translators team by tracking status of documents scheduled for translation.

## Features:
* All CRUD applictaion funcionalities
* Custom user managment system with few levels of access
* 3 UI Languages available - Polish, English and Japanese
* Sortable paginated list of documents in main window
* Simple statistics for documents - data scope can be customized
* Random documents generator - to easily check all functionalities directly after first run

## Used technologies:
* .NET 6.0
* C# 10
* XAML
* Entity Framework Core 7.0.11
* LINQ

## Launch
During every startup application checks if connection with database is possible. If it's not possible, for example during first launch when there is no database yet, application will ask user whether to create it. If user agrees new database with single administrator account will be created automatically using EF Migrations.
