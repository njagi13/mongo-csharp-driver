﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq.Translators;
using FluentAssertions;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace MongoDB.Driver.Tests.Linq.Translators
{
    [TestFixture]
    public class PredicateTranslatorTests : TranslatorTestBase
    {
        [Test]
        public void Any_without_a_predicate()
        {
            Assert(
                x => x.G.Any(),
                1,
                "{G: {$ne: null, $not: {$size: 0}}}");
        }

        [Test]
        public void Any_with_a_predicate_on_documents()
        {
            Assert(
                x => x.G.Any(g => g.D == "Don't" && g.E.F == 33),
                1,
                "{G: {$elemMatch: {D: \"Don't\", 'E.F': 33}}}");
        }

        [Test]
        public void Any_with_a_predicate_on_scalars()
        {
            Assert(
                x => x.M.Any(m => m == 5),
                1,
                "{M: {$elemMatch: {$eq: 5}}}");

            Assert(
                x => x.M.Any(m => m > 2),
                1,
                "{M: {$elemMatch: {$gt: 2}}}");

            Assert(
                x => x.M.Any(m => m > 2 && m < 6),
                1,
                "{M: {$elemMatch: {$gt: 2, $lt: 6}}}");

            Assert(
                x => x.C.E.I.Any(i => i.StartsWith("ick")),
                1,
                new BsonDocument("C.E.I", new BsonDocument("$elemMatch", new BsonDocument("$regex", new BsonRegularExpression("^ick", "s")))));
        }

        [Test]
        public void LocalIListContains()
        {
            IList<int> local = new[] { 10, 20, 30 };

            Assert(
                x => local.Contains(x.Id),
                1,
                "{_id: {$in: [10, 20, 30]}}");
        }

        [Test]
        public void LocalListContains()
        {
            var local = new List<int> { 10, 20, 30 };

            Assert(
                x => local.Contains(x.Id),
                1,
                "{_id: {$in: [10, 20, 30]}}");
        }

        [Test]
        public void LocalArrayContains()
        {
            var local = new[] { 10, 20, 30 };

            Assert(
                x => local.Contains(x.Id),
                1,
                "{_id: {$in: [10, 20, 30]}}");
        }

        [Test]
        public void ArrayLengthEquals()
        {
            Assert(
                x => x.M.Length == 3,
                1,
                "{M: {$size: 3}}");

            Assert(
                x => 3 == x.M.Length,
                1,
                "{M: {$size: 3}}");
        }

        [Test]
        public void ArrayLengthNotEquals()
        {
            Assert(
                x => x.M.Length != 3,
                0,
                "{M: {$not: {$size: 3}}}");
        }

        [Test]
        public void NotArrayLengthEquals()
        {
            Assert(
                x => !(x.M.Length == 3),
                0,
                "{M: {$not: {$size: 3}}}");
        }

        [Test]
        public void NotArrayLengthNotEquals()
        {
            Assert(
                x => !(x.M.Length != 3),
                1,
                "{M: {$size: 3}}");
        }

        [Test]
        public void ArrayPositionEquals()
        {
            Assert(
                x => x.M[1] == 4,
                1,
                "{'M.1': 4}");
        }

        [Test]
        public void ArrayPositionNotEquals()
        {
            Assert(
                x => x.M[1] != 4,
                0,
                "{'M.1': {$ne: 4}}");
        }

        [Test]
        public void ArrayPositionModEqual()
        {
            Assert(
                x => x.M[1] % 2 == 0,
                1,
                "{'M.1': {$mod: [NumberLong(2), NumberLong(0)]}}");
        }

        [Test]
        public void ArrayPositionModNotEqual()
        {
            Assert(
                x => x.M[1] % 3 != 2,
                1,
                "{'M.1': {$not: {$mod: [NumberLong(3), NumberLong(2)]}}}");
        }

        [Test]
        public void Boolean()
        {
            Assert(
                x => x.K,
                1,
                "{K: true}");
        }

        [Test]
        public void BooleanEqualsTrue()
        {
            Assert(
                x => x.K == true,
                1,
                "{K: true}");
        }

        [Test]
        public void BooleanEqualsMethod()
        {
            Assert(
                x => x.K.Equals(true),
                1,
                "{K: true}");
        }

        [Test]
        public void BooleanEqualsFalse()
        {
            Assert(
                x => x.K == false,
                0,
                "{K: false}");
        }

        [Test]
        public void BooleanNotEqualsTrue()
        {
            Assert(
                x => x.K != true,
                0,
                "{K: {$ne: true}}");
        }

        [Test]
        public void BooleanNotEqualsFalse()
        {
            Assert(
                x => x.K != false,
                1,
                "{K: {$ne: false}}");
        }

        [Test]
        public void NotBoolean()
        {
            Assert(
                x => !x.K,
                0,
                "{K: {$ne: true}}");
        }

        [Test]
        public void ClassEquals()
        {
            Assert(
                x => x.C == new C { D = "Dexter" },
                0,
                "{C: {D: 'Dexter', E: null}}");
        }

        [Test]
        public void ClassEqualsMethod()
        {
            Assert(
                x => x.C.Equals(new C { D = "Dexter" }),
                0,
                "{C: {D: 'Dexter', E: null}}");
        }

        [Test]
        public void ClassNotEquals()
        {
            Assert(
                x => x.C != new C { D = "Dexter" },
                1,
                "{C: {$ne: {D: 'Dexter', E: null}}}");
        }
        
        [Test]
        public void ClassMemberEquals()
        {
            Assert(
                x => x.C.D == "Dexter",
                1,
                "{'C.D': 'Dexter'}");
        }

        [Test]
        public void ClassMemberNotEquals()
        {
            Assert(
                x => x.C.D != "Dexter",
                0,
                "{'C.D': {$ne: 'Dexter'}}");
        }

        [Test]
        public void EnumerableCount()
        {
            Assert(
                x => x.G.Count() == 2,
                1,
                "{'G': {$size: 2}}");
        }

        [Test]
        public void EnumerableElementAtEquals()
        {
            Assert(
                x => x.G.ElementAt(1).D == "Dolphin",
                1,
                "{'G.1.D': 'Dolphin'}");
        }

        [Test]
        public void HashSetCount()
        {
            Assert(
                x => x.L.Count == 3,
                1,
                "{'L': {$size: 3}}");
        }

        [Test]
        public void ListCount()
        {
            Assert(
                x => x.O.Count == 3,
                1,
                "{'O': {$size: 3}}");
        }

        [Test]
        public void ListSubEquals()
        {
            Assert(
                x => x.O[2] == 30,
                1,
                "{'O.2': NumberLong(30)}");
        }

        [Test]
        public void RegexInstanceMatch()
        {
            var regex = new Regex("^Awe");
            Assert(
                x => regex.IsMatch(x.A),
                1,
                "{A: /^Awe/}");
        }

        [Test]
        public void RegexStaticMatch()
        {
            Assert(
                x => Regex.IsMatch(x.A, "^Awe"),
                1,
                "{A: /^Awe/}");
        }

        [Test]
        public void RegexStaticMatch_with_options()
        {
            Assert(
                x => Regex.IsMatch(x.A, "^Awe", RegexOptions.IgnoreCase),
                1,
                "{A: /^Awe/i}");
        }

        [Test]
        public void StringContains()
        {
            Assert(
                x => x.A.Contains("some"),
                1,
                "{A: /some/s}");
        }

        [Test]
        public void StringContains_with_dot()
        {
            Assert(
                x => x.A.Contains("."),
                0,
                "{A: /\\./s}");
        }

        [Test]
        public void StringNotContains()
        {
            Assert(
                x => !x.A.Contains("some"),
                0,
                "{A: {$not: /some/s}}");
        }

        [Test]
        public void StringEndsWith()
        {
            Assert(
                x => x.A.EndsWith("some"),
                1,
                "{A: /some$/s}");
        }

        [Test]
        public void StringStartsWith()
        {
            Assert(
                x => x.A.StartsWith("some"),
                0,
                "{A: /^some/s}");
        }

        [Test]
        public void StringEquals()
        {
            Assert(
                x => x.A == "Awesome",
                1,
                "{A: 'Awesome'}");
        }

        [Test]
        public void StringEqualsMethod()
        {
            Assert(
                x => x.A.Equals("Awesome"),
                1,
                "{A: 'Awesome'}");
        }

        [Test]
        public void NotStringEqualsMethod()
        {
            Assert(
                x => !x.A.Equals("Awesome"),
                0,
                "{A: {$ne: 'Awesome'}}");
        }

        public void Assert(Expression<Func<Root, bool>> filter, int expectedCount, string expectedFilter)
        {
            Assert(filter, expectedCount, BsonDocument.Parse(expectedFilter));
        }

        public void Assert(Expression<Func<Root, bool>> filter, int expectedCount, BsonDocument expectedFilter)
        {
            var serializer = BsonSerializer.SerializerRegistry.GetSerializer<Root>();
            var filterDocument = PredicateTranslator.Translate(filter, serializer, BsonSerializer.SerializerRegistry);

            using (var cursor = _collection.FindAsync(filterDocument).GetAwaiter().GetResult())
            {
                var list = cursor.ToListAsync().GetAwaiter().GetResult();
                filterDocument.Should().Be(expectedFilter);
                list.Count.Should().Be(expectedCount);
            }
        }
    }
}
