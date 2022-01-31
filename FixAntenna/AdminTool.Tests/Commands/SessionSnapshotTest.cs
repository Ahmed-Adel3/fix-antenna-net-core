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
using Epam.FixAntenna.NetCore.FixEngine;
using Epam.FixAntenna.NetCore.FixEngine.Manager;
using NUnit.Framework;

namespace Epam.FixAntenna.AdminTool.Tests.Commands
{
	/// <summary>
	/// Test class for SessionSnapshot command
	/// </summary>
	internal class SessionSnapshotTest : AdminToolHelper
	{
		private SessionsSnapshot _sessionsSnapshot;

		[SetUp]
		public void Setup()
		{
			base.Before();
			_sessionsSnapshot = new SessionsSnapshot();
			RequestID = GetNextRequest();
			_sessionsSnapshot.RequestID = RequestID;

			FixSession = FindAdminSession();
		}

		[Test]
		public void TestSessionsSnapshotStatus()
		{
			_sessionsSnapshot.View = View.STATUS;

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);
			Assert.IsNotNull(response.SessionsSnapshotData);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			foreach (var session in receivedStatData.Session)
			{
				var statusData = session.StatusData;
				Assert.IsNotNull(statusData);
				Assert.IsTrue(statusData.InSeqNum > 0);
				Assert.IsTrue(statusData.OutSeqNum > 0);
				Assert.IsNotNull(statusData.BackupState);
				Assert.AreEqual(BackupState.PRIMARY, statusData.BackupState);
				Assert.AreEqual(SessionState.Connected.ToString(), statusData.Status);
			}
		}

		[Test]
		public void TestSessionsSnapshotStatusParams()
		{
			_sessionsSnapshot.View = View.STATUS_PARAMS;
			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);
			Assert.IsNotNull(response.SessionsSnapshotData);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			foreach (var session in receivedStatData.Session)
			{
				var paramsData = session.ParamsData;
				Assert.IsNotNull(paramsData);
				Assert.AreEqual(FIXVersion.FIX44.ToString(), paramsData.Version);
				Assert.IsNotNull(paramsData.ExtraSessionParams);
				var sessionParams = paramsData.ExtraSessionParams;
				Assert.IsNotNull(paramsData.Role);
				if (paramsData.Role == SessionRole.INITIATOR)
				{
					Assert.IsNotNull(paramsData.RemoteHost);
					Assert.IsNotNull(paramsData.RemotePort);
					Assert.IsNull(paramsData.Backup);
					Assert.IsNotNull(sessionParams.HBI);
					Assert.AreEqual(new int?(30), sessionParams.HBI);
				}
				Assert.IsNotNull(sessionParams.InSeqNum);
				Assert.IsTrue(sessionParams.InSeqNum > 0);
				Assert.IsNotNull(sessionParams.OutSeqNum);
				Assert.IsTrue(sessionParams.OutSeqNum > 0);
	//            assertNotNull(sessionParams.getPassword());
	//            assertEquals(StorageType.TRANSIENT, sessionParams.getStorageType());
			}
		}

		[Test]
		public void TestSessionsSnapshotStatParams()
		{
			_sessionsSnapshot.View = View.STATUS_PARAMS_STAT;

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			foreach (var session in receivedStatData.Session)
			{
				var statData = session.StatData;
				Assert.IsNotNull(statData);
				Assert.IsNotNull(statData.LastReceivedMessage);
				Assert.IsNotNull(statData.LastSentMessage);
				Assert.IsNotNull(statData.Established);
			}
		}

		[Test]
		public void TestSessionsSnapshotStatusWithStatParams()
		{
			_sessionsSnapshot.View = View.STATUS;

			var sessionView = new SessionsSnapshotSessionView();
			sessionView.View = View.STATUS_PARAMS_STAT;
			sessionView.SenderCompID = FixSession.Parameters.SenderCompId;
			sessionView.TargetCompID = FixSession.Parameters.TargetCompId;
			_sessionsSnapshot.SessionView.Add(sessionView);

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			StatData statData = null;
			foreach (var session in receivedStatData.Session)
			{
				Assert.IsNotNull(session.StatusData);
				if (session.SenderCompID.Equals(FixSession.Parameters.SenderCompId)
						&& session.TargetCompID.Equals(FixSession.Parameters.TargetCompId))
				{
					statData = session.StatData;
					Assert.IsNull(session.ParamsData);
				}
			}
			Assert.IsNotNull(statData);
		}

		[Test]
		public void TestSessionsSnapshotStatusWithStatParamsSessionWithQualifier()
		{
			_sessionsSnapshot.View = View.STATUS;

			var sessionView = new SessionsSnapshotSessionView();
			sessionView.View = View.STATUS_PARAMS_STAT;
			sessionView.SenderCompID = TestSessionIdQualifier.Sender;
			sessionView.TargetCompID = TestSessionIdQualifier.Target;
			sessionView.SessionQualifier = TestSessionIdQualifier.Qualifier;
			_sessionsSnapshot.SessionView.Add(sessionView);

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			StatData statData = null;
			foreach (var session in receivedStatData.Session)
			{
				Assert.IsNotNull(session.StatusData);
				if (session.SenderCompID.Equals(TestSessionIdQualifier.Sender)
						&& session.TargetCompID.Equals(TestSessionIdQualifier.Target)
						&& IsEquals(session.SessionQualifier, TestSessionIdQualifier.Qualifier))
				{
					statData = session.StatData;
					Assert.IsNull(session.ParamsData);
				}
			}
			Assert.IsNotNull(statData);
		}

		[Test]
		public void TestSessionsSnapshotStatusParamsWithStatData()
		{
			_sessionsSnapshot.View = View.STATUS_PARAMS;

			var sessionView = new SessionsSnapshotSessionView();
			sessionView.View = View.STATUS_PARAMS_STAT;
			sessionView.SenderCompID = FixSession.Parameters.SenderCompId;
			sessionView.TargetCompID = FixSession.Parameters.TargetCompId;
			_sessionsSnapshot.SessionView.Add(sessionView);

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			StatData statData = null;
			foreach (var session in receivedStatData.Session)
			{
				Assert.IsNotNull(session.ParamsData);
				if (session.SenderCompID.Equals(FixSession.Parameters.SenderCompId)
						&& session.TargetCompID.Equals(FixSession.Parameters.TargetCompId))
				{
					statData = session.StatData;
					Assert.IsNull(session.StatusData);
				}
			}
			Assert.IsNotNull(statData);
		}

		[Test]
		public void TestSessionsSnapshotStatusParamsWithStatus()
		{
			_sessionsSnapshot.View = View.STATUS_PARAMS;

			var sessionView = new SessionsSnapshotSessionView();
			sessionView.View = View.STATUS;
			sessionView.SenderCompID = FixSession.Parameters.SenderCompId;
			sessionView.TargetCompID = FixSession.Parameters.TargetCompId;
			_sessionsSnapshot.SessionView.Add(sessionView);

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			StatusData statusData = null;
			foreach (var session in receivedStatData.Session)
			{
				Assert.IsNotNull(session.ParamsData);
				if (session.SenderCompID.Equals(FixSession.Parameters.SenderCompId)
						&& session.TargetCompID.Equals(FixSession.Parameters.TargetCompId))
				{
					statusData = session.StatusData;
					Assert.IsNull(session.StatData);
				}
			}
			Assert.IsNotNull(statusData);
		}

		[Test]
		public void TestSessionsSnapshotStatDataWithStatusData()
		{
			_sessionsSnapshot.View = View.STATUS_PARAMS_STAT;

			var sessionView = new SessionsSnapshotSessionView();
			sessionView.View = View.STATUS;
			sessionView.SenderCompID = FixSession.Parameters.SenderCompId;
			sessionView.TargetCompID = FixSession.Parameters.TargetCompId;
			_sessionsSnapshot.SessionView.Add(sessionView);

			var response = GetReponse(_sessionsSnapshot);
			Assert.AreEqual(RequestID, response.RequestID);

			var receivedStatData = response.SessionsSnapshotData;
			Assert.IsNotNull(receivedStatData.Session);
			Assert.AreEqual(FixSessionManager.Instance.SessionsCount, receivedStatData.Session.Count);
			StatusData statusData = null;
			foreach (var session in receivedStatData.Session)
			{
				Assert.IsNotNull(session.StatData);
				if (session.SenderCompID.Equals(FixSession.Parameters.SenderCompId)
						&& session.TargetCompID.Equals(FixSession.Parameters.TargetCompId))
				{
					statusData = session.StatusData;
					Assert.IsNull(session.ParamsData);
				}
			}
			Assert.IsNotNull(statusData);
		}

		private bool IsEquals(string s1, string s2)
		{
			return (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2)) ? string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2) : s1.Equals(s2);
		}
	}
}