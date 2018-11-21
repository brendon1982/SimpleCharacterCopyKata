using System.Runtime.InteropServices;
using CharacterCopy.Kata.Dependencies;

namespace CharCopyKata
{
    public class Copier
    {
        private readonly ISource _source;
        private readonly IDestination _destination;

        public Copier(ISource source, IDestination destination)
        {
            _source = source;
            _destination = destination;
        }

        public void Copy()
        {
            var read = true;
            while (read)
            {
                var readChar = _source.ReadChar();
                if (readChar != '\n')
                {
                    _destination.WriteChar(readChar);
                }
                else
                {
                    read = false;
                }
            }
        }
    }
}