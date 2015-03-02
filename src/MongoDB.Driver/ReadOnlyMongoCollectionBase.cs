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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MongoDB.Driver
{
    /// <summary>
    /// Base class for implementors of <see cref="IReadOnlyMongoCollection{TDocument}"/>.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    public abstract class ReadOnlyMongoCollectionBase<TDocument> : IReadOnlyMongoCollection<TDocument>
    {
        /// <inheritdoc />
        public abstract CollectionNamespace CollectionNamespace { get; }

        /// <inheritdoc />
        public abstract IBsonSerializer<TDocument> DocumentSerializer { get; }

        /// <inheritdoc />
        public abstract MongoCollectionSettings Settings { get; }

        /// <inheritdoc />
        public abstract Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(Pipeline<TDocument, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc />
        public abstract Task<long> CountAsync(Filter<TDocument> filter, CountOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc />
        public abstract Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldName<TDocument, TField> fieldName, Filter<TDocument> filter, DistinctOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc />
        public abstract Task<IAsyncCursor<TResult>> FindAsync<TResult>(Filter<TDocument> filter, Projection<TDocument, TResult> projection, FindOptions<TDocument> options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc />
        public abstract Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(BsonJavaScript map, BsonJavaScript reduce, MapReduceOptions<TDocument, TResult> options = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
