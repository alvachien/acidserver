# acidserver
Server for identities which cross web apps, built on IdentityServer


## appsettings.json
The file appsettings.json has been removed.

An example file as following:

{

  "ConnectionStrings": {
  
    "DefaultConnection": "Data Source=XXX;Initial Catalog=XXX;Persist Security Info=True;User ID=XXX;Password=XXX;",
	
    "DebugConnection": "Server=XXX;Database=XXX;Integrated Security=SSPI;MultipleActiveResultSets=true"
	
  },
  
  "Logging": {
  
    "IncludeScopes": false,
	
    "LogLevel": {
	
      "Default": "Debug",
	  
      "System": "Information",
	  
      "Microsoft": "Information"
	  
    }
	
  }
  
}

