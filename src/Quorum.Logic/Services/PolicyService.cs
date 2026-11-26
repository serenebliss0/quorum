
using Quorum.Data.Models;
using Quorum.Data.Storage;

namespace Quorum.Logic.Services;

public class PolicyService
{
    private readonly IDataStore _store;

    public PolicyService(IDataStore store)
    {
        _store = store;
    }

    public AcademicPolicy GetPolicy()
    {
        return _store.LoadPolicy();
    }

    public OperationResult SavePolicy(
        User actor,
        AcademicPolicy policy)
    {

        //handle a bunch of error cases
        if (actor is null ||
            actor.Role != UserRole.Administrator)
        {
            return OperationResult.Failure(
                "Only administrators can update academic policy."
            );
        }

        if (policy is null)
        {
            return OperationResult.Failure(
                "Academic policy cannot be empty."
            );
        }

        if (policy.MinCredits < 0)
        {
            return OperationResult.Failure(
                "Minimum credits cannot be negative."
            );
        }

        if (policy.MaxCredits < policy.MinCredits)
        {
            return OperationResult.Failure(
                "Maximum credits cannot be less than minimum credits."
            );
        }

        if (policy.RegistrationEnd <= policy.RegistrationStart)
        {
            return OperationResult.Failure(
                "Registration end must be after registration start."
            );
        }

        _store.SavePolicy(policy);

        return OperationResult.Success(
            "Academic policy saved successfully."
        );
    }

    public bool IsRegistrationOpen(DateTime now)
    {
        AcademicPolicy policy = _store.LoadPolicy();

        return now >= policy.RegistrationStart &&
               now <= policy.RegistrationEnd;
    }
}
