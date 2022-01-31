// Copyright (c) 2021 EPAM Systems
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
using Epam.FixAntenna.NetCore.Common;

namespace Epam.FixAntenna.NetCore.Message.Format
{
	internal abstract class AbstractUtcDateFormatter : IFixDateFormatter
	{
		public abstract int GetFormattedStringLength(DateTimeOffset calendar);

		public virtual int Format(DateTimeOffset calendar, byte[] buff, int offset)
		{
			if (calendar.Offset == DateTimeHelper.UtcOffset)
			{
				return FormatUtcCalendar(calendar, buff, offset);
			}

			var utcCal = calendar.ToUniversalTime();
			return FormatUtcCalendar(utcCal, buff, offset);
		}

		protected internal abstract int FormatUtcCalendar(DateTimeOffset utcCal, byte[] buff, int offset);
	}
}