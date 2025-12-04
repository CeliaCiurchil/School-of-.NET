namespace HotelListing.API.Middleware
{
    internal class ErrorDetails
    {
        public ErrorDetails()
        {
        }

        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}