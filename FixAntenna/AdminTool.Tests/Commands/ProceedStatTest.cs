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

using Epam.FixAntenna.Fixicc.Message;
using NUnit.Framework;

namespace Epam.FixAntenna.AdminTool.Tests.Commands
{
	internal class ProceedStatTest : AdminToolHelper
	{
		private ProceedStat _proceedStat;

		[SetUp]
		public void Setup()
		{
			base.Before();
			_proceedStat = new ProceedStat();
			RequestID = GetNextRequest();
			_proceedStat.RequestID = RequestID;

			FixSession = FindAdminSession();
		}

		[Test] public virtual void TestReceivedStat()
		{
			var response = GetReponse(_proceedStat);
			Assert.AreEqual(RequestID, response.RequestID);
			Assert.IsNotNull(response.ProceedStatData);

			var proceedStatData = response.ProceedStatData;
			Assert.IsTrue(proceedStatData.ProceedMessages >= 1);
		}
	}
}