﻿// Copyright (c) 2021 EPAM Systems
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Epam.FixAntenna.NetCore.Message;
using NUnit.Framework;

namespace Epam.FixAntenna.Message.Tests.Validators
{
	internal class FixMessageDoubleOperationsTest : AbstractFixMessageGetAddSetValidator
	{
		private static readonly string[] Values = { "23", "24.", "25.0", "00026.123" };
		private static readonly string[] WriteValues = { "23", "24", "25", "26.123" };
		private static readonly double[] DoubleValues = { 23, 24, 25, 26.123 };
		private static readonly int[] DoublePrecision = { 0, 0, 1, 3 };

		public override int GetMaxOccurrence(int messageMaxOccurrence)
		{
			return Math.Min(Values.Length, messageMaxOccurrence);
		}

		public override string PrepareTagValueForRead(string ffl, int tagId, int occurrence)
		{
			return ReplaceFieldValue(tagId, occurrence, Values[occurrence - 1], ffl);
		}

		public override string PrepareTagValueForCheckAfterWrite(string ffl, int tagId, int occurrence)
		{
			return ReplaceFieldValue(tagId, occurrence, WriteValues[occurrence - 1], ffl);
		}

		[ExpectedExceptionOnFail(typeof(FieldNotFoundException))]
		public override void CheckGetter(FixMessage ffl, int tagId)
		{
			var actual = ffl.GetTagValueAsDouble(tagId);
			Assert.AreEqual(DoubleValues[0], actual,
				GetValidatorName() + "invalid value for getTagValueAsDouble(" + tagId + ")");
		}

		[ExpectedExceptionOnFail(typeof(FieldNotFoundException))]
		public override void CheckGetterWithOccurrence(FixMessage ffl, int tagId, int occurrence)
		{
			var actual = ffl.GetTagValueAsDouble(tagId, occurrence);
			Assert.AreEqual(DoubleValues[occurrence - 1], actual,
				GetValidatorName() + "invalid value for getTagValueAsDouble(" + tagId + ", " + occurrence + ")");
		}

		[ExpectedExceptionOnFail(typeof(IndexOutOfRangeException))]
		public override void CheckGetterAtIndex(FixMessage ffl, int occurrence, int firstTagIndex)
		{
			var actual = ffl.GetTagValueAsDoubleAtIndex(firstTagIndex);
			Assert.AreEqual(DoubleValues[occurrence - 1], actual,
				GetValidatorName() + "invalid value for getTagValueAsDoubleAtIndex(" + firstTagIndex + ")");
		}

		public override FixMessage AddTag(FixMessage msg, int tagId, int occurrence)
		{
			msg.AddTag(tagId, DoubleValues[occurrence - 1], DoublePrecision[occurrence - 1]);
			return msg;
		}

		[ExpectedExceptionOnFail(typeof(IndexOutOfRangeException))]
		public override FixMessage AddTagAtIndex(FixMessage msg, int index, int tagId, int occurrence)
		{
			msg.AddTagAtIndex(index, tagId, DoubleValues[occurrence - 1], DoublePrecision[occurrence - 1]);
			return msg;
		}

		public override FixMessage SetTag(FixMessage msg, int tagId, int occurrence)
		{
			msg.Set(tagId, DoubleValues[occurrence - 1], DoublePrecision[occurrence - 1]);
			return msg;
		}

		public override FixMessage SetTagWithOccurrence(FixMessage msg, int tagId, int occurrence)
		{
			msg.Set(tagId, occurrence, DoubleValues[occurrence - 1], DoublePrecision[occurrence - 1]);
			return msg;
		}

		[ExpectedExceptionOnFail(typeof(IndexOutOfRangeException))]
		public override FixMessage SetTagAtIndex(FixMessage msg, int index, int occurrence)
		{
			msg.SetAtIndex(index, DoubleValues[occurrence - 1], DoublePrecision[occurrence - 1]);
			return msg;
		}
	}
}