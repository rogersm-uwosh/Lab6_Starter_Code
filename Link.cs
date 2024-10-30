namespace Lab6_Starter
{
	// This class represents a link with a description and URL
	public class Link
	{
		// Property to hold the description of the link
		public string Description { get; set; }

		// Property to hold the URL of the link
		public string Url { get; set; }

		// Constructor to initialize the Link object with a description and a URL
		public Link(string description, string url)
		{
			Description = description;
			Url = url;
		}
	}
}
