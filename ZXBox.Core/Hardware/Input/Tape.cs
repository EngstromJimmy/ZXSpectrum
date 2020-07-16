/// Description: Tape emulator
/// Author: Alex Makeev
/// Date: 16.04.2007
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using ZXMAK.Engine.Loaders;


namespace ZXMAK.Engine.Tape
{
	public enum TapeCommand
	{
		NONE,
		STOP_THE_TAPE,
		STOP_THE_TAPE_48K,
		BEGIN_GROUP,
		END_GROUP,
		SHOW_MESSAGE,

	}
	public class TapeBlock
	{
		public string Description;
		public List<int> Periods = new List<int>();
		public TapeCommand Command = TapeCommand.NONE;
	}

    public class TapeDevice : ZXBox.Hardware.Interfaces.IInput
	{
		#region private data
		private ulong _Z80FQ = 3500000;

		private List<TapeBlock> _blocks = new List<TapeBlock>();
		private int _index = 0;
		private int _playPosition = 0;

		private bool _play = false;

		private int _lastTact = 0;
		private int _waitEdge = 0;
		private int _state = 0;
		#endregion

		public TapeDevice()
		{
			_Z80FQ = 3500000;
		}

		public ulong Z80FQ { get { return _Z80FQ; } }
		public List<TapeBlock> Blocks
		{
			get { return _blocks; }
			set { _blocks = value; }
		}
		public int CurrentBlock
		{
			get
			{
				if (_blocks.Count > 0)
					return _index;
				else
					return -1;
			}
			set
			{
				if (_index >= 0 && _index < _blocks.Count)
				{
					_index = value;
					_playPosition = 0;
					//if(Play)
					//   _currentBlock = _blocks[_index] as TapeBlock;
				}
			}
		}

		public event EventHandler TapeStateChanged;
		private void raiseTapeStateChanged()
		{
			if (TapeStateChanged != null)
				TapeStateChanged(this, new EventArgs());
		}

		public int Position
		{
			get
			{
				if (_playPosition >= _blocks[_index].Periods.Count)
					return 0;
				return _playPosition;
			}
		}

		#region private methods
		private int tape_bit(int globalTact)
		{
			int delta = (int)(globalTact - _lastTact);

			if (!_play)
			{
				_lastTact = globalTact;
				return -1;
			}
			if (_index < 0) //???
			{
				_play = false;
				raiseTapeStateChanged();
				return _state;
			}

			while (delta >= _waitEdge)
			{
				delta -= _waitEdge;
				_state ^= -1;

				_playPosition++;
				if (_playPosition >= _blocks[_index].Periods.Count) // endof block?
				{
					while (_playPosition >= _blocks[_index].Periods.Count)   // skip empty blocks
					{
						_playPosition = 0;
						_index++;
						if (_index >= _blocks.Count) break;
					}
					if (_index >= _blocks.Count)  // end of tape -> rewind & stop
					{
						_lastTact = globalTact;
						_index = 0;
						_play = false;
						raiseTapeStateChanged();
						return _state;
					}
					raiseTapeStateChanged();
				}
				_waitEdge = _blocks[_index].Periods[_playPosition];
			}
			_lastTact = globalTact - delta;
			return _state;
		}
		#endregion

		#region public methods
		public byte GetTapeBit(int globalTact)
		{
			return (byte)(tape_bit(globalTact) & 0x40);
		}

		public bool IsPlay { get { return _play; } }
		//public ulong LastProcessedTact { get { return _lastTact; } }

		public void Reset()  // loaded new image
		{
			_waitEdge = 0;
			_index = -1;
			if (_blocks.Count > 0)
				_index = 0;
			_playPosition = 0;
			_play = false;
			raiseTapeStateChanged();
		}

		public void Rewind(int globalTact)
		{
			_lastTact = globalTact;
			_waitEdge = 0;
			_index = -1;
			if (_blocks.Count > 0)
				_index = 0;
			_playPosition = 0;
			_play = false;
			raiseTapeStateChanged();
		}
		public void Play(int globalTact)
		{
			_lastTact = globalTact;
			if (_blocks.Count > 0 && _index >= 0)
			{
				while (_playPosition >= _blocks[_index].Periods.Count)
				{
					_playPosition = 0;
					_index++;
					if (_index >= _blocks.Count) break;
				}
				if (_index >= _blocks.Count)  // end of tape -> rewind & stop
				{
					_index = -1;
					return;
				}
				//if (_playPosition >= _blocks[_index].Periods.Count)
				//   _playPosition = 0;

				_state ^= -1;
				_waitEdge = _blocks[_index].Periods[_playPosition];
				_play = true;
				raiseTapeStateChanged();
			}
		}
		public void Stop(int globalTact)
		{
			_lastTact = globalTact;
			_play = false;
			raiseTapeStateChanged();
		}
		#endregion



        public int Input(int Port,int tstates)
        {
            int returnvalue = 0xFF;
            if ((Port & 0xFF) == 0xFE)
            {
                return GetTapeBit(tstates);
                
            }
            return 0;
        }
    }
}
