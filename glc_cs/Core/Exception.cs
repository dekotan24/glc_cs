using System;

namespace glc_cs.Core.glException
{
	public class DataUpdateCancellationException : Exception
	{
		public DataUpdateCancellationException()
		{
		}

		public DataUpdateCancellationException(string message)
			: base(message)
		{
		}

		public DataUpdateCancellationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
