using FreeEnc.Infrastructure;
using System;
using System.IO;
using Xunit;

namespace FreeEnc.Tests.StoreTests
{
	public class ConsoleWriterTests
	{
		public ConsoleWriterTests()
		{
			_consoleWriter = new ConsoleWriter();
		}

		[Fact]
		public void ShouldWriteConsoleLineThatMatchesInput()
		{
			using(StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);
				_consoleWriter.WriteConsoleLine("test");

				string expected = string.Format("test{0}", Environment.NewLine);

				Assert.Equal(expected, sw.ToString());
			}
		} 

		[Fact]
		public void ShouldWriteConsoleOutputthatMatchesInput()
		{
			using(StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);
				_consoleWriter.WriteConsole("test");

				string expected = string.Format("test");

				Assert.Equal(expected, sw.ToString());
			}
		}
		
		[Fact]
		public void ShouldWriteHelpCommands()
		{
			using(StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);
				_consoleWriter.WriteHelpCommands();

				string expected = string.Format(_consoleWriter.HelpCommand +"{0}",Environment.NewLine);

				Assert.Equal(expected, sw.ToString());
			}
		}

		[Fact]
		public void ShouldWriteIncorrectHelpTips()
		{
			using(StringWriter sw = new StringWriter())
			{
				Console.SetOut(sw);
				_consoleWriter.WriteIncorrectHelpTips();

				string expected = string.Format(_consoleWriter.HelpTips +"{0}",Environment.NewLine);

				Assert.Equal(expected, sw.ToString());
			}
		}
		private ConsoleWriter _consoleWriter;
	}
}