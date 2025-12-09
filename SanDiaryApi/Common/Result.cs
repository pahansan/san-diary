namespace SanDiaryApi.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public IEnumerable<string> Errors { get; private set; } = [];

        public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
        public static Result<T> Fail(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors };
        public static Result<T> Fail(string error) => Fail([error]);
    }
}
