namespace NServiceBusTutorial.Web.Contributors;

public class VerifyContributorResponse(ContributorRecord contributor)
{
  public ContributorRecord Contributor { get; set; } = contributor;
}
