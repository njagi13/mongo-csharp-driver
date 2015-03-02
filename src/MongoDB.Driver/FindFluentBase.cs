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

using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.Driver
{
    /// <summary>
    /// Base class for implementors of <see cref="IFindFluent{TDocument, TResult}" />.
    /// </summary>
    /// <typeparam name="TDocument">The type of the document.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class FindFluentBase<TDocument, TResult> : IOrderedFindFluent<TDocument, TResult>
    {
        /// <inheritdoc />
        public abstract Filter<TDocument> Filter { get; set; }

        /// <inheritdoc />
        public abstract FindOptions<TDocument> Options { get; }

        /// <inheritdoc />
        public abstract Projection<TDocument, TResult> Projection { get; }

        /// <inheritdoc />
        public abstract Task<long> CountAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <inheritdoc />
        public abstract IFindFluent<TDocument, TResult> Limit(int? limit);

        /// <inheritdoc />
        public abstract IFindFluent<TDocument, TNewResult> Project<TNewResult>(Projection<TDocument, TNewResult> projection);

        /// <inheritdoc />
        public abstract IFindFluent<TDocument, TResult> Skip(int? skip);

        /// <inheritdoc />
        public abstract IFindFluent<TDocument, TResult> Sort(Sort<TDocument> sort);

        /// <inheritdoc />
        public abstract Task<IAsyncCursor<TResult>> ToCursorAsync(CancellationToken cancellationToken);
    }
}
