namespace BlogSite.Models.ServerValidations
{
    public class ServerExceptionData
    {
        ///<summary>Error message that will be displayed in the console</summary>
        public string ConsoleMessage { get;}

        ///<summary>Information that will be displayed directly to the client</summary>
        public string ClientMessage { get;}

        private readonly string StandartClientMessage = "We are sorry to inform you that a server error has occurred.";
        private readonly string StandartConsoleMessage = "Unknown server exception!";

        public ServerExceptionData()
        {
            ClientMessage = StandartClientMessage;
            ConsoleMessage = StandartConsoleMessage;
        }

        public ServerExceptionData(Exception ex)
        {
            ClientMessage = StandartClientMessage;
            ConsoleMessage = ex.ToString();
        }

        public ServerExceptionData(string ClientMessage, Exception ex)
        {
            this.ClientMessage = ClientMessage;
            ConsoleMessage = ex.ToString();
        }

        public ServerExceptionData(string ClientMessage, string ConsoleMessage)
        {
            this.ClientMessage = ClientMessage;
            this.ConsoleMessage = ConsoleMessage;
        }
    }
}