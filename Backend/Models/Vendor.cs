using System.ComponentModel.DataAnnotations;

namespace JWT_Token_Example.Models;

public class Vendor
{
    [Key]
    public Guid Id { get; set; }
    public Guid vendorId { get; set; }
    
    public string? PAN { get; set; }

    public string? AccountNumber { get; set; }

    public string? GST { get; set; }
}