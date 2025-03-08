using System.ComponentModel;

namespace JWT_Token_Example.VendorProfileModels.VendorProfileModels.VendorProfileDTO;

public class VendorProfileDTO
{
    public string UserId { get; set; }

    [DefaultValue("unknown")]
    public string emailId { get; set; }

    public string name { get; set; }

    [DefaultValue("unknown")]
    public string Password { get; set; }

    [DefaultValue(-1)]
    public int MobileNo { get; set; }

    public DateTime AccountCreated { get; set; }

    public DateTime LastLogin { get; set; }

    public string UserType { get; set; }

    [DefaultValue("unknown")]
    public string GST { get; set; }

    [DefaultValue("unknown")]
    public string PAN { get; set; }

    [DefaultValue("unknown")]
    public string BankAccountNo { get; set; }
}