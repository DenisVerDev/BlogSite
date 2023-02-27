namespace BlogSite.Services
{ 
    /// <summary>
    /// Represents message that will be displayed on client's side when page is rendered.
    /// Base class.
    /// </summary>
    public class ServerMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Default constructor creates server error message
        /// </summary>
        public ServerMessage()
        {
            this.Title = "Oops!";
            this.Message = "We are sorry to inform you that a server error has occurred.";
        }

        public ServerMessage(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }
    }
}