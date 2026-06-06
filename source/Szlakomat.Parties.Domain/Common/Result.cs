namespace Szlakomat.Parties.Domain.Common;

public abstract record Result<TF, TS>
{
    public sealed record Success(TS Value) : Result<TF, TS>;
    public sealed record Failure(TF Value) : Result<TF, TS>;

    public bool IsSuccess() => this is Success;
    public bool IsFailure() => this is Failure;

    public TS SuccessValue => this is Success s ? s.Value
        : throw new InvalidOperationException("Result is not Success");
    public TF FailureValue => this is Failure f ? f.Value
        : throw new InvalidOperationException("Result is not Failure");

    public TS? GetSuccess() => this is Success s ? s.Value : default;
    public TF? GetFailure() => this is Failure f ? f.Value : default;

    public Result<TF, TR> Map<TR>(Func<TS, TR> mapper)
    {
        Guard.IsNotNull(mapper);
        return this switch
        {
            Success s => new Result<TF, TR>.Success(mapper(s.Value)),
            Failure f => new Result<TF, TR>.Failure(f.Value),
            _ => throw new InvalidOperationException()
        };
    }

    public Result<TF, TR> FlatMap<TR>(Func<TS, Result<TF, TR>> mapping)
    {
        Guard.IsNotNull(mapping);
        return this switch
        {
            Success s => mapping(s.Value),
            Failure f => new Result<TF, TR>.Failure(f.Value),
            _ => throw new InvalidOperationException()
        };
    }

    public Result<TF, TS> PeekSuccess(Action<TS> consumer)
    {
        Guard.IsNotNull(consumer);
        if (this is Success s) consumer(s.Value);
        return this;
    }

    public Result<TF, TS> PeekFailure(Action<TF> consumer)
    {
        Guard.IsNotNull(consumer);
        if (this is Failure f) consumer(f.Value);
        return this;
    }

    public TR Fold<TR>(Func<TF, TR> onFailure, Func<TS, TR> onSuccess)
    {
        Guard.IsNotNull(onFailure);
        Guard.IsNotNull(onSuccess);
        return this switch
        {
            Success s => onSuccess(s.Value),
            Failure f => onFailure(f.Value),
            _ => throw new InvalidOperationException()
        };
    }

    public static Result<TF, TS> SuccessOf(TS value) => new Success(value);
    public static Result<TF, TS> FailureOf(TF value) => new Failure(value);
}
