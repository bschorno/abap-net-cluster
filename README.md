# ABAPNet.Cluster
A library to convert data in .NET into the cluster format for ABAP

It provides functionality to import from or export to a SAP cluster format. Not all data types are supported yet.

## Getting Started
ABAPNet.Cluster is distributed via NuGet as a NuGet package. It can be installed into your project using NuGet Package Manager in Visual Studio.

**Package Manager CLI**
```Install-Package ABAPNet.Cluster -Version 1.2.3```

**.NET CLI**
```dotnet add package ABAPNet.Cluster --version 1.2.3``` 

A documentation will be added soon.

## How it works

### From .NET to SAP
Create a structure or class as the data interface and define the ABAP-types with the provided attributes
```csharp 
struct ClusterData
{
    [ClusterFieldName("EMPLOYEES")]
    [FlatTable]
    public Employee[] Employees { get; set; }
}

struct Employee
{
    [Int4]
    public int Id { get; set; }

    [Char(32)]
    public string Firstname { get; set; }

    [Char(32)]
    public string Lastname { get; set; }

    [String]
    public string Email { get; set; }

    [Dats]
    public DateOnly Birthday { get; set; }
}
``` 

The data can then be exported using the `DataBuffer` class. The resulting byte-array can be used in SAP for importing the data with `IMPORT ... FROM DATA BUFFER ...`
```chsarp
ClusterData data = new()
{
    Employees = new Employee[]
    {
        new() { Id = 1, Firstname = "Olivia", Lastname = "Bennett", Email = "olivia.benett@example.com", Birthday = new DateOnly(1990, 06, 15) },
        new() { Id = 2, Firstname = "Ethan", Lastname = "Mitchell", Email = "ethan.mitchell@example.com", Birthday = new DateOnly(1985, 11, 3) },
        new() { Id = 3, Firstname = "Sophia", Lastname = "Anderson", Email = "sophia.anderson@example.com", Birthday = new DateOnly(1995, 04, 22) }
    }
};

DataBuffer dataBuffer = new DataBuffer();
byte[] dataExport = dataBuffer.Export(data);
```

```abap
TYPES:
  BEGIN OF t_employee,
    id        TYPE i,
    firstname TYPE c LENGTH 32,
    lastname  TYPE c LENGTH 32,
    email     TYPE string,
    birthday  TYPE d,
  END OF t_employee.

DATA: lt_employees TYPE STANDARD TABLE OF t_employee WITH DEFAULT KEY.

DATA: lv_data_export TYPE xstring VALUE 'FF060201010280003431303300000000...'.
IMPORT employees = lt_employees FROM DATA BUFFER lv_data_export.
```

### From ABAP to .NET
The structures can be defined as above. Only the export and import side are switched. 

```abap
EXPORT employees = lt_employees TO DATA BUFFER lv_data_export.
```

```csharp
DataBuffer dataBuffer = new DataBuffer();
data = dataBuffer.Import<ClusterData>(dataExport);
```