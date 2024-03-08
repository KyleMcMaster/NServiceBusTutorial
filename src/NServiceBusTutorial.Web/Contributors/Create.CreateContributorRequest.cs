using System.ComponentModel.DataAnnotations;

namespace NServiceBusTutorial.Web.Contributors;

public class CreateContributorRequest
{
  public const string Route = "/Contributors";

  [Required]
  public string Name { get; set; } = string.Empty;

  [Required]
  public string PhoneNumber { get; set; } = string.Empty;
}
