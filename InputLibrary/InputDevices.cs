using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
namespace InputLibrary
{
    public class InputDevices
    {
        protected List<String> FilePaths {get;set;}
        public ConcurrentQueue<KeyEvent> KeyStream {get;set;}
        public List<KeyEvent> PressedKeys{get;}
        public InputDevices()
        {
            //Queue that holds key events.
            KeyStream = new ConcurrentQueue<KeyEvent>();
            //Holds FilePaths of all keyboards.
            FilePaths = new List<String>();
            DirectoryInfo d = new DirectoryInfo("/dev/input/by-path");
            FileInfo[] AllDeviceFiles = d.GetFiles();
            //Makes a List of threads for each found input file, done to get inputs while not blocking main program. 
            List<Thread> ListOfKeyboardThreads = new List<Thread>() ;
            foreach (FileInfo File in AllDeviceFiles)
            {
                if (File.Name.ToLower().Contains("kbd"))
                {
                    Console.WriteLine("Found input file");
                    //Make and start thread for device.
                    Thread KeyboardThread = new Thread(new ParameterizedThreadStart(InputStream));
                    //Starts the thread w/ file name and location as parameter.
                    KeyboardThread.Start(File.FullName);
                    FilePaths.Add(File.FullName);
                    ListOfKeyboardThreads.Add(KeyboardThread);
                }
            }
        }

        public KeyEvent GetLastPressedKey()
        {
            KeyEvent Output;
            if (KeyStream.TryPeek(out Output)){
                return Output;
            }
            else {
                throw new Exception("No Key Was pressed sense program started");
            }
        }

        public KeyEvent CurrentlyHeldKey()
        {
            for (int i = 0; i < FilePaths.Count ; i++ )
            {
                using (FileStream fs = File.Open(FilePaths[i], FileMode.Open,FileAccess.Read)) 
                {
                    List<byte> Bytes = new List<byte>();
                    while(true)
                    {
                        byte ByteBuffer = (byte)fs.ReadByte();
                        Console.Write(ByteBuffer+" ");
                        Bytes.Add(ByteBuffer);
                        if (Bytes.Count == 24){
                            KeyEvent Key = new KeyEvent(Bytes.ToArray());
                            Console.WriteLine("The Key Code is " + Key.code);
                            Bytes.Clear();
                            return Key;
                        }
                    }
                }
            }
            throw new Exception("Do you have a keyboard? What the hell did you do.");
        }
        private void InputStream(object FilePath)
        {
            using (FileStream fs = File.Open((string)FilePath, FileMode.Open,FileAccess.Read)) 
            {
                List<KeyEvent> PressedKeys = new List<KeyEvent>();
                List<byte> Bytes = new List<byte>();
                while(true)
                {
                    byte ByteBuffer = (byte)fs.ReadByte();
                    Bytes.Add(ByteBuffer);
                    if (Bytes.Count == 24)
                    {
                        KeyEvent Key = new KeyEvent(Bytes.ToArray());
                        if (Key.value <= 40000 && Key.code !=0)
                        {
                            //Console.WriteLine("\n" + Key.value+" "+Key.type+" "+Key.code);
                            if (Key.value == 0)
                            {
                                PressedKeys.Remove(Key);
                                Console.WriteLine("Removed " + Key.code);
                            }
                            else if (Key.value == 1)
                            {
                                Console.WriteLine("Added " + Key.code);
                                PressedKeys.Add(Key);
                            }
                            KeyStream.Enqueue(Key);
                        }
                        Bytes.Clear();
                    }
                }
            }
        }
    }
}