using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class TestTableDao
{
    [Key]
    public int Id { get; set; }
    public int Year { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}