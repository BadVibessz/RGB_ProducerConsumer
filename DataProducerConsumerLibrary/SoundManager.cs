using System;
using System.Media;
using System.Threading;
using NAudio.Wave;

namespace RGB_ProducerConsumer
{
    public class SoundManager : IDisposable
    {
        private Thread _thread;
        private string _path;
        private WaveOut _player;
        private WaveFileReader _reader;
        
        public SoundManager(string path, float volume = 1)
        {
            _path = path;

            _thread = new(() =>
            {
                _reader = new WaveFileReader(_path);
                _player = new WaveOut();
                this._player.Volume = volume;
            });
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Play()
        {
            _player.Init(_reader);
            _player.Play();
        }

        public void Dispose()
        {
            _thread.Abort();
            _player?.Dispose();
            _reader?.Dispose();
        }
    }
}