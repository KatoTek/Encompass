using System;

#pragma warning disable 1591

namespace Encompass.MatchSync.Concurrent
{
    /// <summary>
    ///     Token that is used to when performing a concurrent sync to uniquely identify instances
    /// </summary>
    [Serializable]
    public class ConcurrencyToken
    {
        #region constructors

        public ConcurrencyToken(string tokenId)
        {
            TokenId = tokenId;
        }

        #endregion

        #region properties

        /// <summary>
        ///     A token used for identification
        /// </summary>
        public string TokenId { get; }

        #endregion

        #region methods

        /// <summary>
        ///     Determines if two ConcurrencyTokens equal each other
        /// </summary>
        /// <param name="obj">A ConcurrencyToken to compare against</param>
        /// <returns>True if the ConcurrencyToken.TokenIds are equal, otherwise false</returns>
        public override bool Equals(object obj)
        {
            var token = obj as ConcurrencyToken;
            return token != null && TokenId.Equals(token.TokenId);
        }

        /// <summary>
        ///     Returns the hash code for a ConcurrencyToken.TokenId
        /// </summary>
        /// <returns>The ConcurrencyToken.TokenId hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = 17;
                result = result*23 + (TokenId?.GetHashCode() ?? 0);
                return result;
            }
        }

        /// <summary>
        ///     Returns the ConcurrencyToken as a string
        /// </summary>
        /// <returns>The TokenId</returns>
        public override string ToString() => TokenId;

        #endregion
    }
}

#pragma warning restore 1591
