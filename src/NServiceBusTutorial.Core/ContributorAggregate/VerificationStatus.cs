using Ardalis.SmartEnum;

namespace NServiceBusTutorial.Core.ContributorAggregate;
public class VerificationStatus : SmartEnum<VerificationStatus>
{
  public static readonly VerificationStatus Pending = new(nameof(Pending), 1);
  public static readonly VerificationStatus Verified = new(nameof(Verified), 2);
  public static readonly VerificationStatus Unverified = new(nameof(Unverified), 3);

  protected VerificationStatus(string name, int value) : base(name, value) { }
}
