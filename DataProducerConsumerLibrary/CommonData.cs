using System.Collections.Generic;
using System.Threading;
using animationTry2;

namespace DataProducerConsumerLibrary
{
    public class CommonData
    {
        public Animator Animator { get; set; }

        private Queue<int>[] _data = { new(), new(), new() };
        //private int maxQueueLength = 2;

        public void PushData(int partialData, DataType type)
        {
            lock (this._data)
            {
                while (_data[(int)type].Count >= 1)
                    Monitor.Wait(_data);
                this._data[(int)type].Enqueue(partialData);
                Monitor.PulseAll(_data);
            }
        }

        public int[] GetData()
        {
            int[] res = new int[3];
            lock (_data)
            {
                foreach (var q in this._data)
                    while (q.Count == 0)
                        Monitor.Wait(_data);

                for (int i = 0; i < res.Length; i++)
                    res[i] = _data[i].Dequeue();
                
                Monitor.PulseAll(this._data);
            }

            return res;
        }
    }
}