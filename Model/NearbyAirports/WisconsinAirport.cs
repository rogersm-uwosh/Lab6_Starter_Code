﻿using System;
using System.ComponentModel;
using CsvHelper.Configuration.Attributes;

namespace Lab6_Starter.Model;

/// <summary>
/// A WIAirport represents a Wisconsin airport, with 
/// </summary>
public class WisconsinAirport
{

    [Index(0)]
    public String Id { get; set; }

    [Index(1)]
    public String Name { get; set; }

    [Index(2)]
    public double Latitude { get; set; }

    [Index(3)]
    public double Longitude { get; set; }

    [Index(4)]
    public String Url { get; set; }

    public double Distance {get;set;}

    public WisconsinAirport(String id, String name, double latitude, double longitude, string url){
        this.Id = id;
        this.Name = name;
        this.Latitude = latitude;
        this.Longitude = longitude;
        this.Url = url;
    }

    override
    public String ToString(){
        return $"{Id}, {Name}, {Latitude}, {Longitude}, {Url}";
    }
}