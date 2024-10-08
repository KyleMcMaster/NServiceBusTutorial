﻿using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace NServiceBusTutorial.Core.ContributorAggregate;

public class Contributor : EntityBase, IAggregateRoot
{
  public string Name { get; private set; } = string.Empty;
  public ContributorStatus Status { get; private set; } = ContributorStatus.NotSet;
  public PhoneNumber? PhoneNumber { get; private set; }
  public VerificationStatus Verification { get; private set; } = VerificationStatus.Pending;

  public Contributor(string name, PhoneNumber phoneNumber, ContributorStatus status, VerificationStatus verificationStatus)
  {
    Name = Guard.Against.NullOrEmpty(name, nameof(name));
    PhoneNumber = phoneNumber;
    Status = status;
    Verification = verificationStatus;
  }

  private Contributor() { } // required for EF

  public void UpdateName(string newName)
  {
    Name = Guard.Against.NullOrEmpty(newName, nameof(newName));
  }

  public void Verify()
  {
    Verification = VerificationStatus.Verified;
  }

  public void NotVerify()
  {
    Verification = VerificationStatus.NotVerified;
  }
}

public class PhoneNumber : ValueObject
{
  public string CountryCode { get; private set; } = string.Empty;
  public string Number { get; private set; } = string.Empty;
  public string? Extension { get; private set; } = string.Empty;

  public PhoneNumber(
    string countryCode,
    string number,
    string? extension)
  {
    CountryCode = countryCode;
    Number = number;
    Extension = extension;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return CountryCode;
    yield return Number;
    yield return Extension ?? string.Empty;
  }
}
