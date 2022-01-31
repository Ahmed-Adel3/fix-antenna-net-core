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
using Epam.FixAntenna.NetCore.Message.Format;
using NUnit.Framework;

namespace Epam.FixAntenna.Message.Tests.Format
{
	[TestFixture]
	internal class LocalMktDateFormatterTest : GenericDateFormatterTst
	{
		internal LocalMktDateFormatter FixDateFormatter = new LocalMktDateFormatter();

		[Test]
		public virtual void TestDiffTimezoneCalendarFormat()
		{
			var cal = CalendarHelper.GetLocalShiftedTestCalendar();
			CheckFormat(FixDateFormatter, cal, "20200101");
		}

		[Test]
		public virtual void TestFormattedStringLength()
		{
			Assert.AreEqual("YYYYMMDD".Length, FixDateFormatter.GetFormattedStringLength(DateTime.Now),
				"Wrong format length");
		}

		[Test]
		public virtual void TestLocalCalendarFormat()
		{
			var cal = CalendarHelper.GetLocalTestCalendar();
			CheckFormat(FixDateFormatter, cal, "20191231");
		}
	}
}