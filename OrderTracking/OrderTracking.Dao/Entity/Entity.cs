using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderTracking.Dao.Entity
{
    public abstract class Entity
    {
        public static long? Transient = null;

        private long? id = Transient;

        /// <summary>
        /// Creates a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Abstract class, and as such exposes
        /// no publicly visible constructors.
        /// </p>
        /// </remarks>
        protected Entity()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Abstract class, and as such exposes
        /// no publicly visible constructors.
        /// </p>
        /// </remarks>
        /// <param name="id">
        /// The number that uniquely identifies this instance.
        /// </param>
        protected Entity(long id)
        {
            this.id = id;
        }

        /// <summary>
        /// The number that uniquely identifies this instance.
        /// </summary>
        public long? Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Is this instance transient?
        /// </summary>
        /// <remarks>
        /// <p>
        /// That is, does it exist in permanent storage?
        /// </p>
        /// </remarks>
        /// <value>
        /// <see lang="true"/> if this instance is transient.
        /// </value>
        public bool IsTransient
        {
            get { return id == Transient; }
        }
    }
}