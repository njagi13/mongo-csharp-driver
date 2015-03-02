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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Extension methods for <see cref="IMongoCollection{T}"/>.
    /// </summary>
    public static class IMongoCollectionExtensions
    {
        /// <summary>
        /// Deletes multiple documents.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result of the delete operation.
        /// </returns>
        public static Task<DeleteResult> DeleteManyAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.DeleteManyAsync(new ExpressionFilter<TDocument>(filter), cancellationToken);
        }

        /// <summary>
        /// Deletes a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result of the delete operation.
        /// </returns>
        public static Task<DeleteResult> DeleteOneAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.DeleteOneAsync(new ExpressionFilter<TDocument>(filter), cancellationToken);
        }

        /// <summary>
        /// Finds a single document and deletes it atomically.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The deleted document if one was deleted.
        /// </returns>
        public static Task<TDocument> FindOneAndDeleteAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Filter<TDocument> filter, 
            FindOneAndDeleteOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            var projection = new IdentityProjection<TDocument>(collection.DocumentSerializer);
            return collection.FindOneAndDeleteAsync(filter, projection, options, cancellationToken);
        }

        /// <summary>
        /// Finds a single document and deletes it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The deleted document if one was deleted.
        /// </returns>
        public static Task<TDocument> FindOneAndDeleteAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            FindOneAndDeleteOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.FindOneAndDeleteAsync(new ExpressionFilter<TDocument>(filter), options, cancellationToken);
        }

        /// <summary>
        /// Finds a single document and deletes it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="projection">The projection.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TResult> FindOneAndDeleteAsync<TDocument, TResult>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            Expression<Func<TDocument, TResult>> projection, 
            FindOneAndDeleteOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");
            Ensure.IsNotNull(projection, "projection");

            return collection.FindOneAndDeleteAsync(
                new ExpressionFilter<TDocument>(filter),
                new FindExpressionProjection<TDocument, TResult>(projection),
                options,
                cancellationToken);
        }

        /// <summary>
        /// Finds a single document and replaces it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TDocument> FindOneAndReplaceAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Filter<TDocument> filter, 
            TDocument replacement, 
            FindOneAndReplaceOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            var projection = new IdentityProjection<TDocument>(collection.DocumentSerializer);
            return collection.FindOneAndReplaceAsync(filter, replacement, projection, options, cancellationToken);
        }

        /// <summary>
        /// Finds a single document and replaces it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TDocument> FindOneAndReplaceAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            TDocument replacement, 
            FindOneAndReplaceOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.FindOneAndReplaceAsync(new ExpressionFilter<TDocument>(filter), replacement, options, cancellationToken);
        }

        /// <summary>
        /// Finds a single document and replaces it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="projection">The projection.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TResult> FindOneAndReplaceAsync<TDocument, TResult>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter,
            TDocument replacement,
            Expression<Func<TDocument, TResult>> projection,
            FindOneAndReplaceOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");
            Ensure.IsNotNull(projection, "projection");

            return collection.FindOneAndReplaceAsync(
                new ExpressionFilter<TDocument>(filter),
                replacement,
                new FindExpressionProjection<TDocument, TResult>(projection),
                options, 
                cancellationToken);
        }

        /// <summary>
        /// Finds a single document and updates it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TDocument> FindOneAndUpdateAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Filter<TDocument> filter, 
            Update2<TDocument> update, 
            FindOneAndUpdateOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(collection, "filter");
            Ensure.IsNotNull(update, "update");

            var projection = new IdentityProjection<TDocument>(collection.DocumentSerializer);
            return collection.FindOneAndUpdateAsync(
                filter,
                projection,
                update,
                options,
                cancellationToken);
        }

        /// <summary>
        /// Finds a single document and updates it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TDocument> FindOneAndUpdateAsync<TDocument>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            Update2<TDocument> update, 
            FindOneAndUpdateOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(collection, "filter");
            Ensure.IsNotNull(update, "update");

            return collection.FindOneAndUpdateAsync(
                new ExpressionFilter<TDocument>(filter),
                update,
                options,
                cancellationToken);
        }

        /// <summary>
        /// Finds a single document and updates it atomically.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="projection">The projection.</param>
        /// <param name="update">The update.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The returned document.
        /// </returns>
        public static Task<TResult> FindOneAndUpdateAsync<TDocument, TResult>(
            this IMongoCollection<TDocument> collection, 
            Expression<Func<TDocument, bool>> filter, 
            Expression<Func<TDocument, TResult>> projection,
            Update2<TDocument> update, 
            FindOneAndUpdateOptions<TDocument> options = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");
            Ensure.IsNotNull(projection, "projection");
            Ensure.IsNotNull(update, "update");

            return collection.FindOneAndUpdateAsync(
                new ExpressionFilter<TDocument>(filter),
                new FindExpressionProjection<TDocument, TResult>(projection),
                update,
                options,
                cancellationToken);
        }

        /// <summary>
        /// Replaces a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="replacement">The replacement.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result of the replacement.
        /// </returns>
        public static Task<ReplaceOneResult> ReplaceOneAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, TDocument replacement, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.ReplaceOneAsync(new ExpressionFilter<TDocument>(filter), replacement, options, cancellationToken);
        }

        /// <summary>
        /// Updates many documents.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result of the update operation.
        /// </returns>
        public static Task<UpdateResult> UpdateManyAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, Update2<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.UpdateManyAsync(new ExpressionFilter<TDocument>(filter), update, options, cancellationToken);
        }

        /// <summary>
        /// Updates a single document.
        /// </summary>
        /// <typeparam name="TDocument">The type of the document.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="update">The update.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The result of the update operation.
        /// </returns>
        public static Task<UpdateResult> UpdateOneAsync<TDocument>(this IMongoCollection<TDocument> collection, Expression<Func<TDocument, bool>> filter, Update2<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.IsNotNull(collection, "collection");
            Ensure.IsNotNull(filter, "filter");

            return collection.UpdateOneAsync(
                new ExpressionFilter<TDocument>(filter),
                update,
                options,
                cancellationToken);
        }
    }
}
