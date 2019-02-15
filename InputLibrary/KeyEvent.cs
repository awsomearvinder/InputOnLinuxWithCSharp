using System;
using System.Collections.Generic;
namespace InputLibrary
{
    public class KeyEvent
    {
        public class TimeVal
        {
            public long tv_sec {get;}
            public long tv_usec {get;}
            public TimeVal(long tv_sec, long tv_usec)
            {
                this.tv_sec = tv_sec;
                this.tv_usec = tv_usec;
            }
        }
        public TimeVal time;
        public short type;
        public short code;
        public int value;
        private IDictionary<short, string> dict = new Dictionary<short, string>()
        {
            {1,"ESC"},
            {2,"1"},
            {3,"2"}
        };
        public KeyEvent(byte[] BytesOfKey)
        {
            time = new TimeVal(BitConverter.ToInt64(BytesOfKey,0),BitConverter.ToInt64(BytesOfKey,8));
            type = BitConverter.ToInt16(BytesOfKey,16);
            code = BitConverter.ToInt16(BytesOfKey,18);
            value = BitConverter.ToInt32(BytesOfKey,20);
        }
    }
}