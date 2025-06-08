using System;
using System.Collections.Generic;

namespace WebApplication12.Models;

public partial class Student
{
    public int Id { get; set; }

    public string StudnetName { get; set; } = null!;//Null Forgiving Operator

    public string StudentGender { get; set; } = null!;//Null Forgiving Operator

    public int Age { get; set; }

    public int Standard { get; set; }

    public string? FatherName { get; set; }// nullable operator
}
