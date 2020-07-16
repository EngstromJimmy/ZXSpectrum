using System;
using System.Collections.Generic;

namespace ZXBox.Snapshot
{
    public class MemoryBlock
    {
        private int _MemoryBlockNumber;

        public int MemoryBlockNumber
        {
            get { return _MemoryBlockNumber; }
            set { _MemoryBlockNumber = value; }
        }

        private List<byte> _MemoryData;

        public List<byte> MemoryData
        {
            get { return _MemoryData; }
            set { _MemoryData = value; }
        }

    }
}