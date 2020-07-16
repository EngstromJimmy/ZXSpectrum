using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using ZXMAK.Engine.Tape;



namespace ZXMAK.Engine.Loaders.TapeSerializers
{
	public class TapSerializer : FormatSerializer
	{
		private TapeDevice _tape;

		
		public TapSerializer(TapeDevice tape)
		{
			_tape = tape;
		}


		#region FormatSerializer

		public override string FormatGroup { get { return "Tape images"; } }
		public override string FormatName { get { return "TAP image"; } }
		public override string FormatExtension { get { return "TAP"; } }

		public override bool CanDeserialize { get { return true; } }
		
		public override void Deserialize(Stream stream)
		{
			byte[] bsize = new byte[2];
			while (stream.Position < stream.Length)
			{
				stream.Read(bsize, 0, 2);
				int size = BitConverter.ToUInt16(bsize, 0);
				if (size == 0) break;
				byte[] block = new byte[size];
				stream.Read(block, 0, size);
				TapeBlock tb = new TapeBlock();
				tb.Description = getBlockDescription(block, 0, block.Length);
				tb.Periods = getBlockPeriods(block, 0, block.Length, 2168, 667, 735, 855, 1710, (block[0] < 4) ? 8064 : 3220, 1000, 8);
				_tape.Blocks.Add(tb);
			}
			_tape.Reset();
		}

		#endregion


		#region helpers
		
		#region Comment
		/// <summary>
		/// Make periods
		/// </summary>
		/// <param name="block">data buffer</param>
		/// <param name="pilot_t">Pilot tone period</param>
		/// <param name="s1_t">Synchro pulse 1 length</param>
		/// <param name="s2_t">Synchro pulse 2 length</param>
		/// <param name="zero_t">Period for bit==0</param>
		/// <param name="one_t">Period for bit==1</param>
		/// <param name="pilot_len">Pilot tone pulse count</param>
		/// <param name="pause">Pause after block (ms)</param>
		/// <param name="last">Used bit count in last byte</param>
		/// <returns></returns>
		#endregion
		public static List<int> getBlockPeriods(byte[] block, int indexOffset, int blockLength,
		   int pilot_t, int s1_t, int s2_t, int zero_t, int one_t,
		   int pilot_len, int pause, int last)
		{
			List<int> periods = new List<int>();
			//int[] periods = new int[blockLength * 16 + pilot_len + 3];

			//int pos = 0;
			if (pilot_len > 0)
			{
				for (int i = 0; i < pilot_len; i++)
					periods.Add(pilot_t);
				periods.Add(s1_t);
				periods.Add(s2_t);
			}
			int t0 = zero_t;
			int t1 = one_t;
			int srcptr = indexOffset;
			for (int i = 0; i < blockLength - 1; i++, srcptr++)
				for (byte j = 0x80; j != 0; j >>= 1)
				{
					periods.Add(((block[srcptr] & j) != 0) ? t1 : t0);
					periods.Add(((block[srcptr] & j) != 0) ? t1 : t0);
				}
			for (byte j = 0x80; j != (byte)(0x80 >> last); j >>= 1) // last byte
			{
				periods.Add(((block[srcptr] & j) != 0) ? t1 : t0);
				periods.Add(((block[srcptr] & j) != 0) ? t1 : t0);
			}
			if (pause != 0)
				periods.Add(pause * 3500);

			return periods;
		}

		public static string getBlockDescription(byte[] block, int indexOffset, int blockLength)
		{
			string dst = string.Empty;
			byte crc = 0;
			byte[] prg = new byte[10];
			for (int i = 0; i < blockLength; i++)
				crc ^= block[indexOffset + i];
			if (block[indexOffset + 0] == 0 && blockLength == 19 &&
			   (block[indexOffset + 1] == 0 || block[indexOffset + 1] == 3))
			{
				for (int i = 0; i < 10; i++)
					prg[i] = (block[indexOffset + i + 2] < 0x20 || block[indexOffset + i + 2] >= 0x80) ? (byte)0x3F : block[indexOffset + i + 2]; // 0x3F='?'
				//for (int i = 9; i > 0 && prg[i] == 0x20; i--)
				//   prg[i] = 0;

				string sprg = System.Text.Encoding.UTF8.GetString(prg, 0, 10);
				string styp = (block[indexOffset + 1] != 0) ? "Bytes" : "Program";

				dst = string.Format("{0}: \"{1}\" {2},{3}", styp, sprg,
				   getUInt16(block, indexOffset + 14),
				   getUInt16(block, indexOffset + 12));
			}
			else if (block[indexOffset + 0] == 0xFF)
				dst = string.Format("Data block, {0} bytes", blockLength - 2);
			else
				dst = string.Format("#{0} block, {1} bytes", block[indexOffset + 0].ToString("X2"), blockLength - 2);
			dst += string.Format(", crc {0}", ((crc != 0) ? "bad" : "ok"));

			return dst;
		}

		#endregion
	}
}
