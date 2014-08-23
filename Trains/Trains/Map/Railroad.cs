namespace Trains.Map
{
    public class Railroad : IRailroad
    {
        /// <summary>
        /// Gets or sets the length of the railroad.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public virtual int Length
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
        public ICity Origin
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
        public ICity Destination
        {
            get;
            set;
        }
    }
}
