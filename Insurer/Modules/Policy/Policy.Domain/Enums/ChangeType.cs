namespace Policy.Domain.Enums;

public enum ChangeType
{
    PolicyCreated,
    PolicyUpdated,
    PolicyExpired,
    PolicyCancelled,
    DocumentAdded,
    ClaimAdded,
    ClaimUpdated,
    ClaimResolved,
    ClaimRejected,
    ClaimApproved,
}
