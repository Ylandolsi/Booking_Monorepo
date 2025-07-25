﻿namespace SharedKernel;

public record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new(
        "General.Null",
        "Null value was provided",
        ErrorType.Failure);

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static Error Failure(string code, string description) => // 500 server error
        new(code, description, ErrorType.Failure);
    public static Error Problem(string code, string description) => // 400 
        new(code, description, ErrorType.Problem);

    public static Error NotFound(string code, string description) => // 404 
        new(code, description, ErrorType.NotFound);


    public static Error Conflict(string code, string description) => // 409
        new(code, description, ErrorType.Conflict);

    public static Error Unauthorized(string code, string description) => // 401 
        new(code, description, ErrorType.Unauthorized);
}
