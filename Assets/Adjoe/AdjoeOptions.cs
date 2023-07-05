using System;
namespace io.adjoe.sdk
{
    /// <summary>
    /// Use this class to pass additional options like the user ID to <c>Adjoe.inheritdoc</c>.
    /// </summary>
    public class AdjoeOptions
    {
        internal bool tosAccepted;
        internal string userId;
        internal AdjoeParams adjoeParams;

        public AdjoeOptions SetTosAccepted(bool tosAccepted)
        {
            this.tosAccepted = tosAccepted;
            return this;
        }

        /// <summary>
        /// Sets the unique user ID to be used by adjoe.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>The instance for chaining.</returns>
        public AdjoeOptions SetUserId(string userId)
        {
            this.userId = userId;
            return this;
        }

        public AdjoeOptions SetAdjoeParams(AdjoeParams adjoeParams)
        {
            this.adjoeParams = adjoeParams;
            return this;
        }
    }
}
