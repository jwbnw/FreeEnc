using System.Diagnostics.CodeAnalysis;

namespace FreeEnc.Infrastructure
{
	[ExcludeFromCodeCoverage]
	public class FileMetadata 
	{
		public FileMetadata(string locationOfFile)
		{
			StartFileLocation = locationOfFile;
		}

		public string Extention { get; set; } 

		public byte LengthOfExtiontionAsByte {get; set;}

		public string NewFileLocation {get; set;}

		public string StartFileLocation {get; set;}
	}
}