# DynamicsFetchToSql
Convert From Dynamics CRM Fetch XML string to SQL Query

It is a simple project for convert Fetch XML string to SQL Query.
2 projects are included in the solution. One of them is an engine and another one is an API project.
Also, the project is live. Anyone can test it like below

https://dynamicsfetchtosql.azurewebsites.net/api/Fetch?fetchXML=

You need the add your fetch XML string to the end. The resulting class is like 

public class ReturnObject
{
    public ReturnObject();

    public string SqlQuery { get; set; }
    public bool IsHasError { get; set; }
    public string Exception { get; set; }
}
