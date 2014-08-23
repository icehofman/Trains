﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Map
{
    public interface ICity
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>
        /// The name of the city.
        /// </value>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the outgoing railroads.
        /// </summary>
        /// <value>
        /// The outgoing railroads.
        /// </value>
        IList<IRailroad> Outgoing
        {
            get;
        }
    }
}
