﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq.Utils;

namespace MongoDB.Driver
{
    /// <summary>
    /// Extension methods for <see cref="IReadOnlyMongoCollection{T}"/>.
    /// </summary>
    public static class IReadOnlyMongoCollectionExtensions
    {
        /// <summary>
        /// Begins a fluent aggregation interface.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A fluent aggregate interface.
        /// </returns>
        public static IAggregateFluent<TDocument> Aggregate<TDocument>(this IReadOnlyMongoCollection<TDocument> collection, AggregateOptions options = null)
        {
            return new AggregateFluent<TDocument, TDocument>(collection, Enumerable.Empty<IPipelineStage>(), options ?? new AggregateOptions());
        }

        /// <summary>
        /// Counts the number of documents in the collection.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of documents in the collection.
        /// </returns>
        public static Task<long> CountAsync<TDocument>(this IReadOnlyMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.CountAsync(new ExpressionFilter<TDocument>(filter), options, cancellationToken);
        }

        /// <summary>
        /// Gets the distinct values for a specified field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="fieldName">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The distinct values for the specified field.
        /// </returns>
        public static Task<IAsyncCursor<TField>> DistinctAsync<TDocument, TField>(this IReadOnlyMongoCollection<TDocument> collection, Expression<Func<TDocument, TField>> fieldName, Filter<TDocument> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(fieldName, "fieldName");
            Ensure.IsNotNull(filter, "filter");

            return collection.DistinctAsync<TField>(
                new ExpressionFieldName<TDocument, TField>(fieldName),
                filter,
                options,
                cancellationToken);
        }

        /// <summary>
        /// Gets the distinct values for a specified field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="fieldName">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The distinct values for the specified field.
        /// </returns>
        public static Task<IAsyncCursor<TField>> DistinctAsync<TDocument, TField>(this IReadOnlyMongoCollection<TDocument> collection, FieldName<TDocument, TField> fieldName, Expression<Func<TDocument, bool>> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(fieldName, "fieldName");
            Ensure.IsNotNull(filter, "filter");

            return collection.DistinctAsync<TField>(
                fieldName,
                new ExpressionFilter<TDocument>(filter),
                options,
                cancellationToken);
        }

        /// <summary>
        /// Gets the distinct values for a specified field.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TField">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="fieldName">The field.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The distinct values for the specified field.
        /// </returns>
        public static Task<IAsyncCursor<TField>> DistinctAsync<TDocument, TField>(this IReadOnlyMongoCollection<TDocument> collection, Expression<Func<TDocument, TField>> fieldName, Expression<Func<TDocument, bool>> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(fieldName, "fieldName");
            Ensure.IsNotNull(filter, "filter");

            return collection.DistinctAsync<TField>(
                new ExpressionFieldName<TDocument, TField>(fieldName),
                new ExpressionFilter<TDocument>(filter),
                options,
                cancellationToken);
        }

        /// <summary>
        /// Begins a fluent find interface.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A fluent find interface.
        /// </returns>
        public static IFindFluent<TDocument, TDocument> Find<TDocument>(
            this IReadOnlyMongoCollection<TDocument> collection, 
            Filter<TDocument> filter, 
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");
            
            FindOptions<TDocument> genericOptions;
            if (options == null)
            {
                genericOptions = new FindOptions<TDocument>();
            }
            else
            {
                genericOptions = new FindOptions<TDocument>
                {
                    AllowPartialResults = options.AllowPartialResults,
                    BatchSize = options.BatchSize,
                    Comment = options.Comment,
                    CursorType = options.CursorType,
                    MaxTime = options.MaxTime,
                    Modifiers = options.Modifiers,
                    NoCursorTimeout = options.NoCursorTimeout
                };
            }

            var projection = new IdentityProjection<TDocument>(collection.DocumentSerializer);
            return new FindFluent<TDocument, TDocument>(collection, filter, projection, genericOptions);
        }

        /// <summary>
        /// Begins a fluent find interface.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// A fluent interface.
        /// </returns>
        public static IFindFluent<TDocument, TDocument> Find<TDocument>(
            this IReadOnlyMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            FindOptions options = null)
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.Find(new ExpressionFilter<TDocument>(filter), options);
        }

        /// <summary>
        /// Finds the documents matching the filter.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task whose result is a cursor.</returns>
        public static Task<IAsyncCursor<TDocument>> FindAsync<TDocument>(
            this IReadOnlyMongoCollection<TDocument> collection, 
            Filter<TDocument> filter, 
            FindOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");
            
            var projection = new IdentityProjection<TDocument>(collection.DocumentSerializer);
            return collection.FindAsync(filter, projection, options, cancellationToken);
        }
    }
}
