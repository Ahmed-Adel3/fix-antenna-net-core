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
using Epam.FixAntenna.Constants.Fixt11;
using Epam.FixAntenna.NetCore.Helpers;
using Epam.FixAntenna.NetCore.Message;

namespace Epam.FixAntenna.NetCore.FixEngine.Session.MessageHandler.User
{
	internal class LastProcessedSequenceSyncMessageHandler : AbstractUserGlobalMessageHandler
	{
		private readonly byte[] _logoutMsgType = "5".AsByteArray();

		/// <inheritdoc />
		public override bool ProcessMessage(FixMessage message)
		{
			if (message.HasTagValue(Tags.LastMsgSeqNumProcessed))
			{
				if (message.IsTagValueEqual(Tags.MsgType, _logoutMsgType))
				{
					try
					{
						Session.OutSeqNum = message.GetTagValueAsLong(Tags.LastMsgSeqNumProcessed);
					}
					catch (Exception ex)
					{
						LogErrorToSession("Error to reset seq number for tag 369 in Logout", ex);
					}
				}
			}

			return true;
		}
	}
}