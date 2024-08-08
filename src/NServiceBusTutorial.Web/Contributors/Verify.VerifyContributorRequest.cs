using System.ComponentModel.DataAnnotations;

namespace NServiceBusTutorial.Web.Contributors;

public class VerifyContributorRequest
{
  public const string Route = "/Contributors/{ContributorId:int}/verify";
  public static string BuildRoute(int contributorId) => Route.Replace("{ContributorId:int}", contributorId.ToString());

  public int ContributorId { get; set; }

  [Required]
  public int Id { get; set; }
  [Required]
  public string? Code { get; set; }
}
