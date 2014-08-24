namespace Trains.Map
{
    public interface IRailroad
    {
        /// <summary>
        /// Gets or sets the length of the railroad.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the City origin of the Railroad.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        ICity Origin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the City destination of the railroad.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        ICity Destination
        {
            get;
            set;
        }
    }
}
