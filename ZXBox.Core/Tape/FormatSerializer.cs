using System;
using System.IO;


namespace ZXMAK.Engine.Loaders
{
	public abstract class FormatSerializer
	{
		public abstract string FormatGroup { get; }
		public abstract string FormatName { get; }
		public abstract string FormatExtension { get; }

		public virtual bool CanDeserialize { get { return false; } }
		public virtual bool CanSerialize { get { return false; } }

		public virtual void Deserialize(Stream stream)
		{
			throw new NotImplementedException(this.GetType().ToString() + ".Deserialize is not implemented.");
		}
		public virtual void Serialize(Stream stream)
		{
			throw new NotImplementedException(this.GetType().ToString() + ".Serialize is not implemented.");
		}


		#region utils

		protected static void setUint16(byte[] buf, int offsetIndex, ushort value)
		{
			buf[offsetIndex] = (byte)value;
			buf[offsetIndex + 1] = (byte)(value >> 8);
		}

		protected static ushort getUInt16(byte[] buf, int offsetIndex)
		{
			return (ushort)(buf[offsetIndex] | buf[offsetIndex + 1] << 8);
		}

		protected static int getInt32(byte[] buf, int offsetIndex)
		{
			return buf[offsetIndex] | buf[offsetIndex + 1] << 8 | buf[offsetIndex + 2] << 16 | buf[offsetIndex + 3] << 24;
		}

		protected static byte[] getBytes(int value)
		{
			byte[] buf = new byte[4];
			buf[0] = (byte)value;
			buf[1] = (byte)(value >> 8);
			buf[2] = (byte)(value >> 16);
			buf[3] = (byte)(value >> 24);
			return buf;
		}
		
		#endregion
	}
}
