﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.Collections.Generic;

using RestSharp;


namespace Rock.Store
{
    /// <summary>
    /// Service class for the store promotions model.
    /// </summary>
    public class PromoService : StoreServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromoService"/> class.
        /// </summary>
        public PromoService() : base()
        { }


        /// <summary>
        /// Gets the promos.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="isTopFree">if set to <c>true</c> [is top free].</param>
        /// <param name="isFeatured">if set to <c>true</c> [is featured].</param>
        /// <param name="isTopPaid">if set to <c>true</c> [is top paid].</param>
        /// <returns></returns>
        public List<Promo> GetPromos( int? categoryId, bool isTopFree = false, bool isFeatured = false, bool isTopPaid = false )
        {
            string error = null;
            return GetPromos( categoryId, out error, isTopFree, isFeatured, isTopPaid );
        }

        /// <summary>
        /// Gets the promos.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="errorResponse">The error response.</param>
        /// <param name="isTopFree">if set to <c>true</c> [is top free].</param>
        /// <param name="isFeatured">if set to <c>true</c> [is featured].</param>
        /// <param name="isTopPaid">if set to <c>true</c> [is top paid].</param>
        /// <returns></returns>
        public List<Promo> GetPromos( int? categoryId, out string errorResponse, bool isTopFree = false, bool isFeatured = false, bool isTopPaid = false )
        {
            errorResponse = string.Empty;

            var requestPath = "Api/Promos/GetNonCategorized";
            if ( categoryId.HasValue )
            {
                requestPath = $"Api/Promos/GetByCategory/{categoryId.Value}";
            }

            var filters = new List<string>();
            if ( isTopFree )
            {
                filters.Add( "IsTopFree eq true" );
            }

            if ( isTopPaid )
            {
                filters.Add( "IsTopPaid eq true" );
            }

            if ( isFeatured )
            {
                filters.Add( "IsFeatured eq true" );
            }

            var queryParameters = new Dictionary<string, List<string>>
            {
                { "$filter", filters }
            };

            // deserialize to list of packages
            var response = ExecuteRestGetRequest<List<Promo>>( requestPath, queryParameters );

            if ( response.ResponseStatus == ResponseStatus.Completed )
            {
                return response.Data;
            }
            else
            {
                errorResponse = response.ErrorMessage;
                return new List<Promo>();
            }
        }
    }
}
