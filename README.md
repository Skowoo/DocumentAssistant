# DocumentAssistant - simple WPF CRUD app
Application created as semester project. Application is designed and created to replace and simplify workflow of small translators team. UI language is Polish.

## Used technologies:
* .NET 6.0
* C# 10
* XAML
* Entity Framework Core 7.0.8
* LINQ

## Launch
During every startup application checks if connection with database is possible. If it's not possible, for example during first launch when there is no database yet, application will ask user whether to create it. If user agrees new database with single administrator account will be created automatically.

## Features:
* Custom user managment system with few levels of access
* All CRUD applictaion funcionalities for noted documents
* Sortable paginated list of documents in main window
* Simple statistics for documents - data scope can be customized
* Random documents generator - to easily check all functionalities directly after first run
