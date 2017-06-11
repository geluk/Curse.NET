namespace Curse.NET.Model
{
	public class ErrorResponse
	{
		public string Message { get; set; }
		public ErrorType ErrorType { get; set; }
	}

	public enum ErrorType
	{
		NotAccessibleByGuests = 7
	}
}