﻿namespace Shared.HttpModels;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    
    public string? Message { get; set; }
}