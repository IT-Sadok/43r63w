using Policy.Application.Dtos;


namespace Policy.Application.Mapping;

public static class PolicyMapper
{
    public static PolicyModel ToDto(this Policy.Domain.Entities.Policy entity)
    {
        return new PolicyModel
        {
            PolicyNumber = entity.PolicyNumber,
            PolicyType = entity.PolicyType,
            CoverageAmount = entity.CoverageAmount,
            PremiumAmount = entity.PremiumAmount,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Status = entity.Status,  
        };
    }
}
