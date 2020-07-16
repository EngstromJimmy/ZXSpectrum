using System;

using ZXMAK.Engine.Tape;


namespace ZXMAK.Engine
{
	public interface ITapeDevice
	{
		TapeDevice Tape { get; }
	}
}
