using System.Collections.Generic;
using System.Linq;

namespace GtMotive.Estimate.Microservice.Domain.Base;

/// <summary>
/// Value Object base class for implementing value objects in the domain model.
/// View: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Compares two value objects
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        return !(left is null ^ right is null) && (ReferenceEquals(left, right) || left.Equals(right));
    }

    /// <summary>
    /// Checks if two value objects are not equal by negating the result of the EqualOperator method.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// Geterates a sequence of components that are used to determine equality for the value object.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Equals method compares the current object with another object of the same type for equality based on their components.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>comparison result</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Gehashcode method
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}
