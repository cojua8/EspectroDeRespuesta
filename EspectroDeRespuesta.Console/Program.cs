// See https://aka.ms/new-console-template for more information
using CsvHelper;
using CsvHelper.Configuration;
using EspectroDeRespuesta.Library.Models;
using EspectroDeRespuesta.Library.SolutionMethods;
using System.Globalization;

Console.WriteLine("Hello, World!");

var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };

var forceRecordings = new ForceRecordings();

using (var reader = new StreamReader("D:\\Downloads\\Libro2.csv"))
using (var csv = new CsvReader(reader, config) )
{
    var records = csv.GetRecords<TimeForcePair>();

    forceRecordings.Forces = records.ToList();
}

var structuralProperties = new StructuralProperties { Mass = 0.2533f, Damping = 0.1592f, Stiffness = 10 };

var centralDifference = new Newmark(structuralProperties, forceRecordings, new ConstantAcceleration());

centralDifference.Calculate();    

var timeHistory = new List<TimePositionPair>(centralDifference.Position.Count());

for (int i = 0; i < centralDifference.Position.Count; i++)
{
    var time = forceRecordings.Forces[i].Time;
    var position = centralDifference.Position[i];

    timeHistory.Add(new TimePositionPair() { Position = position , Time=time});
}

using (var writer = new StreamWriter("D:\\Downloads\\output.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(timeHistory);
}