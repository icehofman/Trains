using System.Collections.Generic;

namespace Trains.Map
{
    public interface IRailroadMap
    {
        /// <summary>
        /// Gets the rail roads read from the file stream.
        /// </summary>
        IEnumerable<IRailroad> Railroads
        {
            get;
        }

        /// <summary>
        /// Gets the cities read from the file stream.
        /// </summary>
        IEnumerable<ICity> Cities
        {
            get;
        }
    }
}
